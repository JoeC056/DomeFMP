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
    [SerializeField] private GameObject allowEntryCheckbox;

    [Header("Prefabs")]
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private GameObject checkboxPrefab;

    [Header("Parameters")]
    public int scoreForCorrectEntryAllowance;
    public int penaltyForIncorrectEntryAllowance;
    public int scoreForCorrectReasonAllowance;
    public int penaltyForIncorrectReasonAllowance;

    [Header("Data")]
    public List<string> aspectsToCheck;
    public bool entryShouldBeAllowed;
    public List<bool> correctAnswers;



    //////////////////////////////////////////////////////////////////////////////
    public void CalculateScoreBasedOnAnswers()
    {
        //Gain points if entry allowance correct, lose if incorrect
        if (entryShouldBeAllowed == allowEntryCheckbox.GetComponent<ChecklistCheckbox>().ticked)
        {
            GameManager.instance.score += scoreForCorrectEntryAllowance;
        }
        else
        {
            GameManager.instance.score -= penaltyForIncorrectEntryAllowance;
        }
        //Checks all reasons given against all correct reasons and assigns values respectively 
        for (int i = 0; i < aspectsToCheck.Count; i++)
        {
            if (checkboxesParent.transform.GetChild(i).GetComponent<ChecklistCheckbox>().ticked == correctAnswers[i])
            {
                GameManager.instance.score += scoreForCorrectReasonAllowance;
            }
            else
            {
                GameManager.instance.score -= penaltyForIncorrectEntryAllowance;
            }
        }
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

        //Assigns text and tickboxes for all aspects to check 
        foreach (string aspectToCheck in aspectsToCheck)
        {
            GameObject text = Instantiate(textPrefab, textParent.transform);
            text.GetComponent<TextMeshProUGUI>().text = aspectToCheck;
            Instantiate(checkboxPrefab,checkboxesParent.transform);
        }

        allowEntryCheckbox.GetComponent<ChecklistCheckbox>().Untick();
    }

    //////////////////////////////////////////////////////////////////////////////
    public void AssignNewChecklistValues(List<string> toCheck, bool entryAllowed,List<bool> answers)
    {
        //Assigns values based on given parameters
        aspectsToCheck = toCheck;
        entryShouldBeAllowed = entryAllowed;
        correctAnswers = answers;

        UpdateChecklistContents();
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ClearList()
    {
        //Empties contents of checklist and its respective values 
        foreach (Transform child in textParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in checkboxesParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        aspectsToCheck = null;
        correctAnswers = null;
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ResetList()
    {
        //Unticks all values 
        foreach (Transform child in checkboxesParent.transform)
        {
            child.GetComponent<ChecklistCheckbox>().Untick();
        }

        allowEntryCheckbox.GetComponent<ChecklistCheckbox>().Untick();
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
