using TMPro;
using UnityEngine;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////
public class DocumentViewingApplication : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI appNameText;

    [Header("Parameters")]
    [SerializeField] private Vector2 documentSize;

    [Header("Prefabs")]
    [SerializeField] private GameObject gameplayDocumentTemplate;


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
    public void OpenDocument(GameObject documentToOpen)
    {
        if (documentToOpen.GetComponent<GameplayDocument>() != null)
        {
            if (displayedDocument != null)
            {
                Destroy(displayedDocument);
            }
            displayedDocument = Instantiate(gameplayDocumentTemplate, transform);

            //Assigns the name of the document and displays it appropiately
            displayedDocument.name = documentToOpen.name;
            displayedDocument.name.Replace("(Clone)", "");
            appNameText.text = "Document Viewer (" + displayedDocument.name + ") READ-ONLY";

            displayedDocument.GetComponent<GameplayDocument>().UpdateTextForEncounterData(documentToOpen.GetComponent<GameplayDocument>().respectiveEncounter);


            Vector2 sizeDelta = displayedDocument.GetComponent<RectTransform>().sizeDelta;
            displayedDocument.transform.localScale = new Vector3(documentSize.x / sizeDelta.x, documentSize.y / sizeDelta.y, 1);

            displayedDocument.GetComponent<Button>().enabled = false;

        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
