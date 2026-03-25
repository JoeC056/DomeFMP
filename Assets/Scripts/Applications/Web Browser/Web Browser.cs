using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField] private Scrollbar scrollbar;

    [Header("Prefabs")]
    [SerializeField] private GameObject linkButton;

    [Header("Parameters")]
    [SerializeField] private float scrollbarSizeScalerValue;
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

    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
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

        //Disables website view by default
        webPageUI.SetActive(false);
        urlText.text = "";

        UpdateHomePage();
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void UpdateHomePage()
    {
        //First removes previous buttons
        foreach (Transform child in linkButtonsParent.transform)
        {
            Destroy(child);
        }

        //Adds buttons for each available website 
        foreach (WebsiteSO website in unlockedWebsites)
        {
            GameObject link = Instantiate(linkButton, linkButtonsParent.transform);
            link.GetComponent<LinkButton>().AssignWebsiteLinkedTo(website);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void LoadWebsite(WebsiteSO websiteToLoad)
    {
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
        websiteContentParent.GetComponent<RectTransform>().anchoredPosition = new Vector2 (0,-((websiteSize - defaultPageSize.y)/2));

        //Updates contents of webpage from respective link
        Instantiate(websiteToLoad.websiteContent, websiteContentParent.transform);
        urlText.text = websiteToLoad.websiteUrl;
        AssignRangeOfScrollbar(websiteToLoad.websiteContent);

        if (GetRespectiveEventForWebsiteLoad(websiteToLoad) != null)
        {
            GetRespectiveEventForWebsiteLoad(websiteToLoad).Invoke();

            foreach (onFirstLoadEvent loadEvent in onFirstLoadEvents)
            {
                if (loadEvent.respectiveWebsite == websiteToLoad)
                {
                    onFirstLoadEvents.Remove(loadEvent);
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void HomeButton()
    {
        //Takes player back to home page
        homePageUI.SetActive(true);
        webPageUI.SetActive(false);
        urlText.text = "";
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
}

//////////////////////////////////////////////////////////////////////////////////
