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
    [SerializeField] private IncorrectAnswersNotice incorrectAnswersNotice;

    [Header("Prefabs")]
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private GameObject checkboxPrefab;

    [Header("Parameters")]
    [SerializeField] private float amountToMoveYToPeekChecklist;
    [SerializeField] private float amountToMoveYToExpandChecklist;

    [Header("Data")]
    private List<string> aspectsToCheck;

    //Components
    private RectTransform rectTransform;

    private bool isExpanded = false;
    private bool isPeeking = false;

    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - amountToMoveYToExpandChecklist);
    }

    //////////////////////////////////////////////////////////////////////////////
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPeeking && !isExpanded)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + amountToMoveYToPeekChecklist);
            isPeeking = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isPeeking && !isExpanded)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - amountToMoveYToPeekChecklist);
            isPeeking = false;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ToggleExpansion()
    {
        if (isExpanded)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - amountToMoveYToExpandChecklist);
            isExpanded = false;
        }
        else
        {
            if (isPeeking)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + amountToMoveYToExpandChecklist - amountToMoveYToPeekChecklist);
                isPeeking = false;
            }
            else
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + amountToMoveYToExpandChecklist);
            }

            isExpanded = true;
        }
    }
    //////////////////////////////////////////////////////////////////////////////
    public float CalculateScoreToAddBasedOnAnswers(List<string> thingsToCheck, List<bool> correctAnswers, bool entryShouldBeAllowed)
    {
        float scoreToAdd = 0;
        bool correctEntryAllowance = false;
        List<string> elementsMissed = new List<string>();
        List<string> elementsIncorrectlyLabeled = new List<string>();

        //Gain points if entry allowance correct, lose if incorrect
        if (entryShouldBeAllowed == allowEntryCheckbox.GetComponent<ChecklistCheckbox>().ticked)
        {
            scoreToAdd += GameManager.instance.scoreForCorrectEntryAllowance;
            correctEntryAllowance = true;
        }
        else
        {
            scoreToAdd -= GameManager.instance.penaltyForIncorrectEntryAllowance;
            correctEntryAllowance = false;
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
                if (correctAnswers[i] == true)
                {
                    elementsMissed.Add(aspectsToCheck[i]);
                }
                else
                {
                    elementsIncorrectlyLabeled.Add(aspectsToCheck[i]);
                }
            }
        }

        if (elementsMissed.Count > 0 || elementsIncorrectlyLabeled.Count > 0 || !correctEntryAllowance)
        {
            incorrectAnswersNotice.DisplayNotice(elementsMissed, elementsIncorrectlyLabeled, correctEntryAllowance, entryShouldBeAllowed);
        }
        else
        {
            incorrectAnswersNotice.HideNotice();
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
