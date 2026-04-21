using UnityEngine;

public class CloseAppButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transcript transcript;
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

        if (application.applicationName == "Document Viewing Application")
        {
            if (transcript.waitingToStopLookingAtDocument)
            {
                transcript.waitingToStopLookingAtDocument = false;
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
