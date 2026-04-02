using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class DocumentViewingApplication : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI appNameText;

    [Header("Parameters")]
    [SerializeField] private Vector2 documentSize;


    //Instance of DocumentViewingApplication
    public static DocumentViewingApplication instance { get; private set; }

    private GameObject displayedDocument;

    //////////////////////////////////////////////////////////////////////////////
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
    }

    //////////////////////////////////////////////////////////////////////////////
    public void UpdateViewedDocument(GameObject newDocument)
    {
        if (displayedDocument != null)
        {
            Debug.Log("Goodbye");
            Destroy(displayedDocument);
            displayedDocument = null;
        }
        Debug.Log("Making a new one");
        displayedDocument = Instantiate(newDocument, transform);
        displayedDocument.GetComponent<RectTransform>().sizeDelta = documentSize;
        appNameText.text = "Document Viewer (" + displayedDocument.name + ") READ-ONLY";
        Debug.Log("Made a new one");
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
