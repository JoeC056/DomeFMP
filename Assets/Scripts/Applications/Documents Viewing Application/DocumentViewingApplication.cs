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

    //////////////////////////////////////////////////////////////////////////////
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

    }

    //////////////////////////////////////////////////////////////////////////////
    public void UpdateViewedDocument(GameObject newDocument)
    {
        GameObject displayedDocument = Instantiate(newDocument, transform);
        displayedDocument.GetComponent<RectTransform>().sizeDelta = documentSize;
        appNameText.text = "Document Viewer (" + displayedDocument.name + ") READ-ONLY";
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
