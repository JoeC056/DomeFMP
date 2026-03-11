using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class DocumentMessage : MonoBehaviour
{
    [Header("Document Opened on click")]
    public GameObject respectiveDocument;

    //////////////////////////////////////////////////////////////////////////////
    public void OnClick()
    {
        ApplicationSO docViewingApplication = DocumentViewingApplication.instance.gameObject.GetComponent<Application>().application;

        //Focuses app if already open, otherwise opens it
        if (!ComputerManager.instance.openApplications.Contains(docViewingApplication))
        {
            ComputerManager.instance.OpenApplication(docViewingApplication);
            DocumentViewingApplication.instance.UpdateViewedDocument(respectiveDocument);
        }
        else
        {
            ComputerManager.instance.FocusApplication(docViewingApplication);
            DocumentViewingApplication.instance.UpdateViewedDocument(respectiveDocument);
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
