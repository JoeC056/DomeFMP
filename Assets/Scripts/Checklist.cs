using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

//////////////////////////////////////////////////////////////////////////////
public class Checklist : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject textParent;
    [SerializeField] private GameObject checkboxesParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private GameObject checkboxPrefab;


    public List<string> aspectsToCheck;
    public List<bool> correctAnswers;
    public List<bool> givenAnswers;

    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        UpdateChecklistContents();
    }

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        
    }

    //////////////////////////////////////////////////////////////////////////////
    private void UpdateChecklistContents()
    {
        //First empties the checklist
        foreach (Transform child in textParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in checkboxesParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (string aspectToCheck in aspectsToCheck)
        {
            GameObject text = Instantiate(textPrefab, textParent.transform);
            text.GetComponent<TextMeshProUGUI>().text = aspectToCheck;
            Instantiate(checkboxPrefab,checkboxesParent.transform);
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ResetList()
    {
        foreach (Transform child in checkboxesParent.transform)
        {
            child.GetComponent<ChecklistCheckbox>().Untick();
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
