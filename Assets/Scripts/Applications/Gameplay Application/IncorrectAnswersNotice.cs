using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

//////////////////////////////////////////////////////////////////////////////
public class IncorrectAnswersNotice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI mistakesText;
    [SerializeField] private TextMeshProUGUI correctEntryAllowance;

    [Header("Parameters")]
    [SerializeField] private float yPositionWhenHiding;
    [SerializeField] private float yPositionWhenShowing;
    [SerializeField] private float yPositionWhenPeeking;
    [SerializeField] private float yPositionWhenExpanded;

    private RectTransform rectTransform;

    private bool isPeeking;
    private bool isExpanded;

    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPositionWhenHiding);
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ToggleExpand()
    {
        if (isExpanded)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPositionWhenShowing);
            isExpanded = false;
        }
        else
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPositionWhenExpanded);
            isExpanded = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPeeking && !isExpanded)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPositionWhenPeeking);
            isPeeking = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isPeeking && !isExpanded)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPositionWhenShowing);
            isPeeking = false;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void DisplayNotice(List<string> missedAnswers, List<string> incorrectAnswers, bool correctEntryAllowanceGiven, bool shouldHaveBeenAllowed)
    {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPositionWhenPeeking);
        isPeeking = true;
        correctEntryAllowance.text = "";
        mistakesText.text = "";

        if (!correctEntryAllowanceGiven)
        {
            if (shouldHaveBeenAllowed)
            {
                correctEntryAllowance.text = "SHOULD HAVE ALLOWED ENTRY";
            }
            else
            {
                correctEntryAllowance.text = "SHOULD NOT HAVE ALLOWED ENTRY";
            }
        }
        else
        {
            correctEntryAllowance.text = "";
        }


        foreach (string mistake in missedAnswers)
        {
            mistakesText.text += mistake.ToUpper() + "\n";
        }
        foreach (string mistake in incorrectAnswers)
        {
            mistakesText.text += "NO " + mistake.ToUpper() + "\n";
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void HideNotice()
    {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPositionWhenHiding);
    }


    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
