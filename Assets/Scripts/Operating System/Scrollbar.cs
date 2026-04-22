using UnityEngine;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////////
public class ScrollbarScript : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float mouseScrollSpeed;
    [SerializeField] private WebBrowser webBrowser;

    private Scrollbar scrollbar;

    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        scrollbar = GetComponent<Scrollbar>();
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        CheckToScroll();    
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void CheckToScroll()
    {
        bool canScroll = true;

        if (webBrowser != null)
        {
            if (webBrowser.loadingWebsite)
            {
                canScroll = false;
            }
        }

        float input = Input.GetAxis("Mouse ScrollWheel");

        if (input != 0 && canScroll)
        {
            scrollbar.value += -input * mouseScrollSpeed;
            scrollbar.value = Mathf.Clamp(scrollbar.value, 0, 1);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
