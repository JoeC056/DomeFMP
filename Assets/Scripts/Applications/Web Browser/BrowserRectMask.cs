using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class BrowserRectMask : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WebBrowser webBrowserScript;

    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        //Assigns size and placement based on respective values from web browser
        //transform.GetComponent<RectTransform>().sizeDelta = webBrowserScript.defaultPageSize;
        //transform.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, webBrowserScript.defaultPageYDisplacement);
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
