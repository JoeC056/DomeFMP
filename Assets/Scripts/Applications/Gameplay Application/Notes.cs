using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

//////////////////////////////////////////////////////////////////////////////
public class Notes : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI mainNotes;
    [SerializeField] private TextMeshProUGUI subNotes;

    [Header("Days Notes")]
    [SerializeField] private string day1Notes;
    [SerializeField] private string day2Notes;
    [SerializeField] private string day2Half2Notes;
    [SerializeField] private string day3Notes;
    [SerializeField] private string day3Half2Notes;
    [SerializeField] private string day4Notes;
    [SerializeField] private string day4Half2Notes;

    [Header("Unlockable Subnotes")]
    [SerializeField] private string day2SubNotes;
    [SerializeField] private string day3SubNotes;
    [SerializeField] private string day4SubNotes;

    [Header("Parameters")]
    [SerializeField] private float amountToMoveYToExpandNotes;


    private RectTransform rectTransform;
    private bool isExpanded;

    //Instance of notes 
    public static Notes instance;


    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

    }

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
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isExpanded)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + amountToMoveYToExpandNotes);
            isExpanded = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isExpanded)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - amountToMoveYToExpandNotes);
            isExpanded = false;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        UpdateNotesContent();
    }

    //////////////////////////////////////////////////////////////////////////////
    private void UpdateNotesContent()
    {
        if (GameManager.instance.dayNo == 1)
        {
            mainNotes.text = day1Notes;
        }
        if (GameManager.instance.dayNo == 2)
        {
            if (GameManager.instance.secondHalfStarted)
            {
                mainNotes.text = day2Half2Notes;
            }
            else
            {
                mainNotes.text = day2Notes;
            }
        }
        if (GameManager.instance.dayNo == 3)
        {
            if (GameManager.instance.secondHalfStarted)
            {
                mainNotes.text = day3Half2Notes;
            }
            else
            {
                mainNotes.text = day3Notes;
            }
        }
        if (GameManager.instance.dayNo == 4)
        {
            if (GameManager.instance.secondHalfStarted)
            {
                mainNotes.text = day4Half2Notes;
            }
            else
            {
                mainNotes.text = day4Notes;
            }
        }
    }


    //////////////////////////////////////////////////////////////////////////////
    public void UnlockDaysSubNotes()
    {
        if (GameManager.instance.dayNo == 2)
        {
            subNotes.text = day2SubNotes;
        }
        else if (GameManager.instance.dayNo == 3)
        {
            subNotes.text = day3SubNotes;
        }
        else if (GameManager.instance.dayNo == 4)
        {
            subNotes.text = day4SubNotes;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ResetSubNotes()
    {
        subNotes.text = "";
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
