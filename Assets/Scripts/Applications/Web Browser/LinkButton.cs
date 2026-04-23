using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

//////////////////////////////////////////////////////////////////////////////////
public class LinkButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Parameters")]
    [SerializeField] private Color defaultColour;
    [SerializeField] private Color colourOnHover;

    public WebsiteSO websiteLinkedTo;

    private TextMeshProUGUI text;

    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        if (websiteLinkedTo != null)
        {
            AssignWebsiteLinkedTo(websiteLinkedTo);
        }

        text = GetComponent<TextMeshProUGUI>();
        text.color = defaultColour;
        text.fontStyle = FontStyles.Normal;
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = colourOnHover;
        text.fontStyle = FontStyles.Underline;
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = defaultColour;
        text.fontStyle = FontStyles.Normal;
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void OnClick()
    {
        WebBrowser.instance.LoadWebsite(websiteLinkedTo);
        text.color = defaultColour;
        text.fontStyle = FontStyles.Normal;
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void AssignWebsiteLinkedTo(WebsiteSO valueToAssign)
    {
        //Called on instantation
        websiteLinkedTo = valueToAssign;
        GetComponent<TextMeshProUGUI>().text = valueToAssign.websiteUrl;
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
