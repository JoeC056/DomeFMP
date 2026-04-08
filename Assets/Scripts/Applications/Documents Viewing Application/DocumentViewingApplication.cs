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
    [SerializeField] private GameObject virusTestResultsDocument;
    [SerializeField] private GameObject vaccineDocument;
    [SerializeField] private GameObject heightWeightDocument;
    [SerializeField] private GameObject irregularSpeechTestDocument;

    [SerializeField] private GameObject domeConstructionInformationNewspaper;
    [SerializeField] private GameObject emergingCultInformationNewspaper;
    [SerializeField] private GameObject groupOccupanceOfDomeNewspaper;
    [SerializeField] private GameObject groupPlanDocument1;
    [SerializeField] private GameObject groupPlanDocument2;
    [SerializeField] private GameObject symptomInformationDocument1;
    [SerializeField] private GameObject symptomInformationDocument2;


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
            Debug.Log(documentToOpen.name);
            if (displayedDocument != null)
            {
                Destroy(displayedDocument);
            }
            if (documentToOpen.GetComponent<VirusTestResultsDocument>() != null)
            {
                displayedDocument = Instantiate(virusTestResultsDocument, transform);
            }
            else if (documentToOpen.GetComponent<VaccineDocument>() != null)
            {
                displayedDocument = Instantiate(vaccineDocument, transform);
            }
            else if (documentToOpen.GetComponent<HeightWeightDocument>() != null)
            {
                displayedDocument = Instantiate(heightWeightDocument, transform);
            }
            else if (documentToOpen.GetComponent<IrregularSpeechTestDocument>() != null)
            {
                displayedDocument = Instantiate(irregularSpeechTestDocument, transform);
            }
            else
            {
                displayedDocument = Instantiate(gameplayDocumentTemplate, transform);
            }

            displayedDocument.GetComponent<GameplayDocument>().UpdateTextForEncounterData(documentToOpen.GetComponent<GameplayDocument>().respectiveEncounter);
        }
        if (documentToOpen.name.Contains(domeConstructionInformationNewspaper.name))
        {
            displayedDocument = Instantiate(domeConstructionInformationNewspaper, transform);
        }
        else if (documentToOpen.name.Contains(emergingCultInformationNewspaper.name))
        {
            displayedDocument = Instantiate(emergingCultInformationNewspaper, transform);
        }
        else if (documentToOpen.name.Contains(groupOccupanceOfDomeNewspaper.name))
        {
            displayedDocument = Instantiate(groupOccupanceOfDomeNewspaper, transform);
        }
        else if (documentToOpen.name.Contains(groupPlanDocument1.name))
        {
            displayedDocument = Instantiate(groupPlanDocument1, transform);
        }
        else if (documentToOpen.name.Contains(groupPlanDocument2.name))
        {
            displayedDocument = Instantiate(groupPlanDocument2, transform);
        }
        else if (documentToOpen.name.Contains(symptomInformationDocument1.name))
        {
            displayedDocument = Instantiate(symptomInformationDocument1, transform);
        }
        else if (documentToOpen.name.Contains(symptomInformationDocument2.name))
        {
            displayedDocument = Instantiate(symptomInformationDocument2, transform);
        }
        else if (displayedDocument == null)
        {
            displayedDocument = Instantiate(gameplayDocumentTemplate, transform);
        }


        //Assigns the name of the document and displays it appropiately
        displayedDocument.name = documentToOpen.name;
        displayedDocument.name = displayedDocument.name.Replace("(Clone)", "");
        appNameText.text = "Document Viewer (" + displayedDocument.name + ") READ-ONLY";



        Vector2 sizeDelta = displayedDocument.GetComponent<RectTransform>().sizeDelta;
        displayedDocument.transform.localScale = new Vector3(documentSize.x / sizeDelta.x, documentSize.y / sizeDelta.y, 1);

        displayedDocument.GetComponent<Button>().enabled = false;

    }
}

//////////////////////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////////////////////
