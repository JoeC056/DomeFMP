using UnityEngine;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////////
public class ScrollbarHandle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject rightBorderSecondLayer;
    [SerializeField] private GameObject bottomBorderSecondLayer;
    [SerializeField] private GameObject topBorder;
    [SerializeField] private GameObject leftBorder;
    [SerializeField] private GameObject rightBorder;
    [SerializeField] private GameObject bottomBorder;
    [SerializeField] private Scrollbar scrollbar;

    [Header("Parameters")]
    [SerializeField] private float pixelSize;
    [SerializeField] private float scrollbarWidth;


    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        UpdateBordersOfHandle();
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void UpdateBordersOfHandle()
    {
        //rightBorderSecondLayer.GetComponent<RectTransform>().sizeDelta = new Vector2(pixelSize, scrollbar.size);
        //leftBorder.GetComponent<RectTransform>().sizeDelta = new Vector2(pixelSize, scrollbar.size);
        //rightBorder.GetComponent<RectTransform>().sizeDelta = new Vector2(pixelSize, scrollbar.size);

        //pixelSize *= 100;

        //rightBorderSecondLayer.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

        //bottomBorderSecondLayer.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

        //topBorder.transform.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

        //leftBorder.transform.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

        //rightBorder.transform.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

        //bottomBorder.transform.GetComponent<RectTransform>().position =
        //    GetComponent<RectTransform>().position;
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
