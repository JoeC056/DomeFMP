using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;

//////////////////////////////////////////////////////////////////////////////
public class Checklist : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private GameObject textParent;
    [SerializeField] private GameObject checkboxesParent;
    [SerializeField] private GameObject allowEntryCheckbox;

    [Header("Prefabs")]
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private GameObject checkboxPrefab;

    [Header("Parameters")]
    [SerializeField] private float amountToMoveYToExpandChecklist;

    [Header("Data")]
    private List<string> aspectsToCheck;

    //Components
    private RectTransform rectTransform;

    private bool isExpanded;

    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - amountToMoveYToExpandChecklist);
        isExpanded = false;
    }
    //////////////////////////////////////////////////////////////////////////////
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isExpanded)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + amountToMoveYToExpandChecklist);
            isExpanded = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isExpanded)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - amountToMoveYToExpandChecklist);
            isExpanded = false;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public float CalculateScoreToAddBasedOnAnswers(List<bool> correctAnswers, bool entryShouldBeAllowed)
    {
        float scoreToAdd = 0;

        //Gain points if entry allowance correct, lose if incorrect
        if (entryShouldBeAllowed == allowEntryCheckbox.GetComponent<ChecklistCheckbox>().ticked)
        {
            scoreToAdd += GameManager.instance.scoreForCorrectEntryAllowance;
        }
        else
        {
            scoreToAdd -= GameManager.instance.penaltyForIncorrectEntryAllowance;
        }
        //Checks all reasons given against all correct reasons and assigns values respectively 
        for (int i = 0; i < aspectsToCheck.Count; i++)
        {
            if (checkboxesParent.transform.GetChild(i).GetComponent<ChecklistCheckbox>().ticked == correctAnswers[i])
            {
                scoreToAdd += GameManager.instance.scoreForCorrectReasonAllowance;
            }
            else
            {
                scoreToAdd -= GameManager.instance.penaltyForIncorrectReasonAllowance;
            }
        }

        return scoreToAdd;
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
    public void AssignNewChecklistValues(List<string> toCheck)
    {
        //Assigns values based on given parameters
        aspectsToCheck = toCheck;

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
