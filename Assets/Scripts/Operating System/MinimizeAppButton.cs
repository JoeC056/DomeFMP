using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class MinimizeAppButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transcript transcript;

    //Ref to application button minimizes
    private ApplicationSO application;

    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        //Assigns value of application
        application = transform.parent.GetComponent<Application>().application;
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void MinimizeApp()
    {
        if (application.applicationName == "Document Viewing Application")
        {
            if (transcript.waitingToStopLookingAtDocument)
            {
                transcript.waitingToStopLookingAtDocument = false;
            }
        }

        ComputerManager.instance.UnfocusApplication(application);
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
