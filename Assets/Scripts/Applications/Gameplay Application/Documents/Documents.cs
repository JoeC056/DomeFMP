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


    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        documentsToDisplay = new List<GameObject>();
        CheckToDisplayButtons();
    }


    //////////////////////////////////////////////////////////////////////////////
    public void AddNewDocumentsToDisplay(List<GameObject> newDocuments)
    {
        //Assigns variable values for new documents
        documentsToDisplay = newDocuments;
        indexOfCurrentlyDisplayedDocument = 0;
        currentlyDisplayedDocument = Instantiate(documentsToDisplay[indexOfCurrentlyDisplayedDocument], transform);
        currentlyDisplayedDocument.GetComponent<RectTransform>().sizeDelta = documentSize;
        CheckToDisplayButtons();
    }

    //////////////////////////////////////////////////////////////////////////////
    public void RemoveCurrentDocuments()
    {
        //Removes displayed documents and resets variable values
        Destroy(currentlyDisplayedDocument);
        currentlyDisplayedDocument = null;
        documentsToDisplay = null;
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
            currentlyDisplayedDocument = Instantiate(documentsToDisplay[indexOfCurrentlyDisplayedDocument], transform.position, Quaternion.Euler(Vector3.zero), transform);
            currentlyDisplayedDocument.GetComponent<RectTransform>().sizeDelta = documentSize;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
