using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class Document : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////////////
    public void OnClick()
    {
        ApplicationSO docViewingApplication = DocumentViewingApplication.instance.gameObject.GetComponent<Application>().application;

        //Focuses app if already open, otherwise opens it
        if (!ComputerManager.instance.openApplications.Contains(docViewingApplication))
        {
            ComputerManager.instance.OpenApplication(docViewingApplication);
            DocumentViewingApplication.instance.UpdateViewedDocument(this.gameObject);
        }
        else
        {
            ComputerManager.instance.FocusApplication(docViewingApplication);
            DocumentViewingApplication.instance.UpdateViewedDocument(this.gameObject);
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
