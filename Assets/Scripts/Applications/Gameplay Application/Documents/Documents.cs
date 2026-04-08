using System;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class Documents : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject switchDocumentButtons;

    [Header("Parameters")]
    [SerializeField] private Vector2 documentSize;

    //Contents of what is currently displayed 
    private List<GameObject> documentsToDisplay;
    private GameObject currentlyDisplayedDocument;
    private int indexOfCurrentlyDisplayedDocument;

    //Instance of GameManager
    public static Documents instance { get; private set; }


    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        documentsToDisplay = new List<GameObject>();
        CheckToDisplayButtons();

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
    public void AddNewDocumentToDisplay(GameObject newDocument)
    {
        //Assigns variable values for new documents
        documentsToDisplay.Clear();
        documentsToDisplay.Add(newDocument);


        indexOfCurrentlyDisplayedDocument = 0;
        currentlyDisplayedDocument = Instantiate(documentsToDisplay[indexOfCurrentlyDisplayedDocument], transform);
        Vector2 sizeDelta = currentlyDisplayedDocument.GetComponent<RectTransform>().sizeDelta;
        currentlyDisplayedDocument.transform.localScale = new Vector3(documentSize.x / sizeDelta.x, documentSize.y / sizeDelta.y, 1);
        CheckToDisplayButtons();
    }

    //////////////////////////////////////////////////////////////////////////////
    public void AddNewDocumentsToDisplay(List<GameObject> newDocuments, EncounterSO ownerOfDocuments)
    {
        //Assigns variable values for new documents
        documentsToDisplay.Clear();
        documentsToDisplay = newDocuments;

        //Updates documents to match respective encounter
        foreach (GameObject document in documentsToDisplay)
        {
            document.GetComponent<GameplayDocument>().UpdateTextForEncounterData(ownerOfDocuments);
        }

        indexOfCurrentlyDisplayedDocument = 0;
        currentlyDisplayedDocument = Instantiate(documentsToDisplay[indexOfCurrentlyDisplayedDocument], transform);
        Vector2 sizeDelta = currentlyDisplayedDocument.GetComponent<RectTransform>().sizeDelta;
        currentlyDisplayedDocument.transform.localScale = new Vector3(documentSize.x / sizeDelta.x, documentSize.y / sizeDelta.y, 1);
        CheckToDisplayButtons();
    }

    //////////////////////////////////////////////////////////////////////////////
    public void RemoveCurrentDocuments()
    {
        //Removes displayed documents and resets variable values
        Destroy(currentlyDisplayedDocument);
        currentlyDisplayedDocument = null;
        documentsToDisplay = null;
        documentsToDisplay = new List<GameObject>();
        CheckToDisplayButtons();
    }

    //////////////////////////////////////////////////////////////////////////////
    private void CheckToDisplayButtons()
    {
        switchDocumentButtons.SetActive(documentsToDisplay.Count > 1);
    }

    //////////////////////////////////////////////////////////////////////////////
    public void SwitchDisplayedDocument(int amountToIncrementListBy)
    {
        //Checks if incrementing list would be out of bounds
        if (indexOfCurrentlyDisplayedDocument != Math.Clamp(indexOfCurrentlyDisplayedDocument + amountToIncrementListBy, 0, documentsToDisplay.Count - 1))
        {
            //Removes currently displayed document, adds new based on increment
            indexOfCurrentlyDisplayedDocument = Math.Clamp(indexOfCurrentlyDisplayedDocument + amountToIncrementListBy, 0, documentsToDisplay.Count - 1);
            Destroy(currentlyDisplayedDocument);
            currentlyDisplayedDocument = null;
            currentlyDisplayedDocument = Instantiate(documentsToDisplay[indexOfCurrentlyDisplayedDocument],transform);

            Vector2 sizeDelta = currentlyDisplayedDocument.GetComponent<RectTransform>().sizeDelta;
            currentlyDisplayedDocument.transform.localScale = new Vector3(documentSize.x / sizeDelta.x, documentSize.y / sizeDelta.y, 1);
            //currentlyDisplayedDocument.GetComponent<RectTransform>().sizeDelta = documentSize;

            Debug.Log(indexOfCurrentlyDisplayedDocument);
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
