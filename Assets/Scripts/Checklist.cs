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
    private void Awake()
    {
        UpdateChecklistContents();
    }

    //////////////////////////////////////////////////////////////////////////////
    public void CalculateScoreBasedOnAnswers()
    {
        if (entryShouldBeAllowed == allowEntryCheckbox.GetComponent<ChecklistCheckbox>().ticked)
        {
            GameManager.instance.score += scoreForCorrectEntryAllowance;
        }
        else
        {
            GameManager.instance.score -= penaltyForIncorrectEntryAllowance;
        }    
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
        Debug.Log(GameManager.instance.score);
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

        allowEntryCheckbox.GetComponent<ChecklistCheckbox>().Untick();
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ResetList()
    {
        foreach (Transform child in checkboxesParent.transform)
        {
            child.GetComponent<ChecklistCheckbox>().Untick();
        }

        allowEntryCheckbox.GetComponent<ChecklistCheckbox>().Untick();
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
