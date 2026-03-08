using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////////
public class ComputerManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject taskbar;
    [SerializeField] private GameObject applicationsParent;
    [SerializeField] private GameObject desktopIconsParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject emptyTaskbarIconPrefab;


    //Instance of ComputerManager
    public static ComputerManager instance { get; private set; }

    //Lists of open apps and apps with open windows
    public List<ApplicationSO> openApplications;
    public LinkedList<ApplicationSO> openWindowsStack;


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
        //Displays only the app at the top of the stack
        foreach (Transform child in applicationsParent.transform)
        {
            if (openWindowsStack.Count > 0)
            {
                desktopIconsParent.SetActive(false);

                if (child.GetComponent<Application>().application == openWindowsStack.First.Value)
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
            appIcon.GetComponent<Image>().sprite = openApp.TaskbarIconImage;

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
}
