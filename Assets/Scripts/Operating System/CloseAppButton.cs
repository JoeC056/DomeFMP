using UnityEngine;

public class CloseAppButton : MonoBehaviour
{
    //Ref to application button closes
    private ApplicationSO application;

    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        //Assigns value of application
        application = transform.parent.GetComponent<Application>().application;
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void CloseApp()
    {
        ComputerManager.instance.CloseApplication(application);

        //Returns to home page of respective applications on close where applicable 
        if (application.applicationName == "Web Browser")
        {
            WebBrowser.instance.HomeButton();
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
