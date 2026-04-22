using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////////
public class WebBrowser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject homePageUI;
    [SerializeField] private GameObject webPageUI;
    [SerializeField] private TextMeshProUGUI urlText;
    [SerializeField] private GameObject websiteContentParent;
    [SerializeField] private GameObject linkButtonsParent;
    [SerializeField] private GameObject homePage;
    [SerializeField] private GameObject homePageRectMask;
    [SerializeField] private GameObject webPageRectMask;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private TextMeshProUGUI appNameText;

    [Header("Prefabs")]
    [SerializeField] private GameObject linkButton;

    [Header("Parameters")]
    [SerializeField] private float delayBeforeWebsiteOpen;
    [SerializeField] private float scrollbarSizeScalerValue;
    [SerializeField] private int noOfStepsToLoadPage;
    [SerializeField] private float delayBetweenSteps;
    [SerializeField] private float homePageDefaultYPosition;
    public Vector2 defaultPageSize;
    public float defaultPageYDisplacement;

    [Header("Unlocked Websites")]
    public List<WebsiteSO> unlockedWebsites;


    //Temporarily stored values
    private float websiteSize;
    private float lastScrollBarValue = 0;


    //Instance of WebBrowser
    public static WebBrowser instance { get; private set; }

    private struct onFirstLoadEvent
    {
        public WebsiteSO respectiveWebsite;
        public UnityEvent eventOnLoad;
    }

    private List<onFirstLoadEvent> onFirstLoadEvents;

    private bool loadingHomePage;
    public bool loadingWebsite;
    private bool openingWebsite;
    private WebsiteSO websiteOpening;
    private bool waiting;


    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        //Disables website view by default
        webPageUI.SetActive(false);
        urlText.text = "";

        UpdateHomePage();

        appNameText.text = "Web Browser (Home Page)";
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        scrollbar.enabled = !loadingWebsite;

        if (loadingHomePage && !waiting)
        {
            homePageRectMask.GetComponent<RectTransform>().anchoredPosition -= new Vector2(webPageRectMask.GetComponent<RectTransform>().anchoredPosition.x, 258.5938f / noOfStepsToLoadPage);
            homePage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -homePageRectMask.GetComponent<RectTransform>().anchoredPosition.y + homePageDefaultYPosition);

            if (homePageRectMask.GetComponent<RectTransform>().anchoredPosition.y <= homePageDefaultYPosition)
            {
                homePageRectMask.GetComponent<RectTransform>().anchoredPosition = new Vector2(homePageRectMask.GetComponent<RectTransform>().anchoredPosition.x, homePageDefaultYPosition);
                homePage.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                loadingHomePage = false;
            }
            else
            {
                StartCoroutine(Wait());
            }
        }

        if (loadingWebsite && !waiting) 
        {
            webPageRectMask.GetComponent<RectTransform>().anchoredPosition -= new Vector2(webPageRectMask.GetComponent<RectTransform>().anchoredPosition.x, 258.5938f / noOfStepsToLoadPage);
            websiteContentParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -((websiteSize - defaultPageSize.y) / 2) - webPageRectMask.GetComponent<RectTransform>().anchoredPosition.y);

            if (webPageRectMask.GetComponent<RectTransform>().anchoredPosition.y <= 0)
            {
                webPageRectMask.GetComponent<RectTransform>().anchoredPosition = new Vector2(webPageRectMask.GetComponent<RectTransform>().anchoredPosition.x, 0);
                websiteContentParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -((websiteSize - defaultPageSize.y) / 2));
                loadingWebsite = false;
            }
            else
            {
                StartCoroutine(Wait());
            }
        }
    }
    
    //////////////////////////////////////////////////////////////////////////////////
    public void CreateSingleton()
    {
        //Ensures singleton nature of instance variable
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        onFirstLoadEvents = new List<onFirstLoadEvent>();
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void UpdateHomePage()
    {
        List<WebsiteSO> websitesToAdd = new List<WebsiteSO>();

        foreach (WebsiteSO website in unlockedWebsites)
        {
            websitesToAdd.Add(website);
        }


        //First removes previous buttons
        foreach (Transform child in linkButtonsParent.transform)
        {
            if (unlockedWebsites.Contains(child.GetComponent<LinkButton>().websiteLinkedTo))
            {
                websitesToAdd.Remove(child.GetComponent<LinkButton>().websiteLinkedTo);
            }
        }

        //Adds buttons for each available website 
        foreach (WebsiteSO website in websitesToAdd)
        {
            GameObject link = Instantiate(linkButton, linkButtonsParent.transform);
            link.GetComponent<LinkButton>().AssignWebsiteLinkedTo(website);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void LoadWebsite(WebsiteSO websiteToLoad)
    {
        if (websiteToLoad != websiteOpening)
        {
            if (openingWebsite)
            {
                StopAllCoroutines();
            }
            StartCoroutine(DisplayWebsite(websiteToLoad));
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private IEnumerator DisplayWebsite(WebsiteSO websiteToLoad)
    {
        websiteOpening = websiteToLoad;
        openingWebsite = true;
        yield return new WaitForSeconds(delayBeforeWebsiteOpen);
        openingWebsite = false;
        websiteOpening = null;

        //First removes previous websites
        if (websiteContentParent.transform.childCount > 0)
        {
            Destroy(websiteContentParent.transform.GetChild(0).gameObject);
        }
        //Hides home page
        homePageUI.SetActive(false);
        webPageUI.SetActive(true);

        //Scales position of website content with it's size 
        websiteSize = websiteToLoad.websiteContent.GetComponent<RectTransform>().sizeDelta.y;
        websiteContentParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -((websiteSize - defaultPageSize.y) / 2));

        //Updates contents of webpage from respective link
        Instantiate(websiteToLoad.websiteContent, websiteContentParent.transform);
        urlText.text = websiteToLoad.websiteUrl;
        AssignRangeOfScrollbar(websiteToLoad.websiteContent);

        appNameText.text = "Web Browser (" + websiteToLoad.websiteName + ")";

        if (GetRespectiveEventForWebsiteLoad(websiteToLoad) != null)
        {
            GetRespectiveEventForWebsiteLoad(websiteToLoad).Invoke();

            onFirstLoadEvent loadEventToRemove = new onFirstLoadEvent();

            foreach (onFirstLoadEvent loadEvent in onFirstLoadEvents)
            {
                if (loadEvent.respectiveWebsite == websiteToLoad)
                {
                    loadEventToRemove = loadEvent;
                }
            }

            onFirstLoadEvents.Remove(loadEventToRemove);
        }

        Refresh();
    }


    //////////////////////////////////////////////////////////////////////////////////
    public void HomeButton()
    {
        //Takes player back to home page
        homePageUI.SetActive(true);
        webPageUI.SetActive(false);
        urlText.text = "";
        appNameText.text = "Web Browser (Home Page)";
        Refresh();
    }


    //////////////////////////////////////////////////////////////////////////////////
    private void AssignRangeOfScrollbar(GameObject websiteContent)
    {
        ResetScrollbar();

        //Displays scrollbar only if page large enough to warrant scrolling being required
        if (websiteSize > defaultPageSize.y)
        {
            scrollbar.gameObject.SetActive(true);
        }
        else
        {
            scrollbar.gameObject.SetActive(false);
        }

        //Scales size of scrollbar to size of page 
        scrollbar.size = (defaultPageSize.y / websiteSize) * scrollbarSizeScalerValue;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void ResetScrollbar()
    {
        lastScrollBarValue = 0;
        scrollbar.value = 0;
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void Scroll()
    {
        //Determines amount scrolled
        float amountScrolled = scrollbar.value - lastScrollBarValue;
        lastScrollBarValue = scrollbar.value;

        //Moves website contents based on amount scrolled 
        websiteContentParent.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, (websiteSize - defaultPageSize.y) * amountScrolled);
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void UnlockNewWebsite(WebsiteSO websiteToUnlock)
    {
        unlockedWebsites.Add(websiteToUnlock);
        UpdateHomePage();
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void AddEventOnWebsiteLoad(WebsiteSO website, UnityEvent newEvent)
    {
        onFirstLoadEvent diaCompEvent = new onFirstLoadEvent();
        diaCompEvent.respectiveWebsite = website;
        diaCompEvent.eventOnLoad = newEvent;

        onFirstLoadEvents.Add(diaCompEvent);
    }

    //////////////////////////////////////////////////////////////////////////////////
    private UnityEvent GetRespectiveEventForWebsiteLoad(WebsiteSO websiteLoaded)
    {
        foreach (onFirstLoadEvent loadEvent in onFirstLoadEvents)
        {
            if (loadEvent.respectiveWebsite == websiteLoaded)
            {
                return loadEvent.eventOnLoad;
            }
        }
        return null;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private IEnumerator Wait()
    {
        waiting = true;
        yield return new WaitForSeconds(delayBetweenSteps);
        waiting = false;
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void Refresh()
    {
        if (!openingWebsite)
        {
            if (homePageUI.activeSelf)
            {
                loadingHomePage = true;
                homePageRectMask.GetComponent<RectTransform>().anchoredPosition = new Vector2(homePageRectMask.GetComponent<RectTransform>().anchoredPosition.x, 258.5938f + homePageDefaultYPosition);
                homePage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -homePageRectMask.GetComponent<RectTransform>().anchoredPosition.y + homePageDefaultYPosition);
            }
            else if (webPageUI.activeSelf)
            {
                loadingWebsite = true;
                webPageRectMask.GetComponent<RectTransform>().anchoredPosition = new Vector2(webPageRectMask.GetComponent<RectTransform>().anchoredPosition.x, 258.5938f);
                websiteContentParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -((websiteSize - defaultPageSize.y) / 2) - webPageRectMask.GetComponent<RectTransform>().anchoredPosition.y);
            }

            StopAllCoroutines();
            waiting = false;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
