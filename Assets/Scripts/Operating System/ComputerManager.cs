using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class ComputerManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject taskbar;
    [SerializeField] private GameObject applicationsParent;
    [SerializeField] private GameObject desktopIconsParent;
    [SerializeField] private DocumentViewingApplication docViewingApp;
    [SerializeField] private WebBrowser webBrowserApp;

    [Header("Prefabs")]
    [SerializeField] private GameObject emptyTaskbarIconPrefab;


    //Instance of ComputerManager
    public static ComputerManager instance { get; private set; }

    //Lists of open apps and apps with open windows
    [HideInInspector] public List<ApplicationSO> openApplications;
    [HideInInspector] public LinkedList<ApplicationSO> openWindowsStack;


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
        foreach (Transform child in applicationsParent.transform)
        {
            child.gameObject.SetActive(false);
        }
        docViewingApp.CreateSingleton();
        webBrowserApp.CreateSingleton();

        openWindowsStack = new LinkedList<ApplicationSO>();
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        UpdateDisplay();
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void UpdateDisplay()
    {
        UpdateDisplayedWindows();
        ReorderWindowHierarchy();
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void UpdateDisplayedWindows()
    {
        //Displays only windows that are open 
        foreach (Transform child in applicationsParent.transform)
        {
            if (openWindowsStack.Count > 0)
            {
                desktopIconsParent.SetActive(false);

                if (openWindowsStack.Contains(child.GetComponent<Application>().application))
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
            else
            {
                desktopIconsParent.SetActive(true);
                child.gameObject.SetActive(false);
            }

        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void ReorderWindowHierarchy()
    {
        if (openWindowsStack.Count > 0)
        {
            foreach (Transform child in applicationsParent.transform)
            {
                if (openWindowsStack.Contains(child.GetComponent<Application>().application))
                {
                    //Essentially flips index
                    int indexToSet = (openWindowsStack.Count - 1) - openWindowsStack.ToList().IndexOf(child.GetComponent<Application>().application);

                    child.SetSiblingIndex(indexToSet);
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void UpdateTaskbar()
    {
        //First empties the taskbar
        foreach (Transform child in taskbar.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        //Reassigns each icon for taskbar
        foreach (ApplicationSO openApp in openApplications)
        {
            GameObject appIcon = Instantiate(emptyTaskbarIconPrefab, taskbar.transform);
            appIcon.GetComponent<TaskbarIcon>().AssignApplicationRef(openApp);
            appIcon.GetComponent<TaskbarIcon>().AssignImage(openApp.TaskbarIconImage);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void OpenApplication(ApplicationSO application)
    {
        //Opens app and focuses it
        openApplications.Add(application);
        openWindowsStack.AddFirst(application);
        UpdateTaskbar();
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void CloseApplication(ApplicationSO application)
    {
        //Closes app
        openApplications.Remove(application);
        openWindowsStack.Remove(application);
        UpdateTaskbar();
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void FocusApplication(ApplicationSO application)
    {
        //Focuses the app, adding it to the top of the stack
        if (openWindowsStack.Contains(application))
        {
            openWindowsStack.Remove(application);
        }
        openWindowsStack.AddFirst(application);
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void UnfocusApplication(ApplicationSO application)
    {
        //Unfocuses the app, removing it from the stack
        openWindowsStack.Remove(application);
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void ReturnToDesktop()
    {
        if (!GameManager.instance.gameplayInProgress)
        {
            LinkedList<ApplicationSO> openWindows = new LinkedList<ApplicationSO>();

            foreach (ApplicationSO application in openWindowsStack)
            {
                openWindows.AddLast(application);
            }

            foreach (ApplicationSO application in openWindows)
            {
                UnfocusApplication(application);
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////
}
