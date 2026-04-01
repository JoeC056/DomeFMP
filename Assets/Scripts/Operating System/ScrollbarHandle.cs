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


    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        UpdateBordersOfHandle();
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void UpdateBordersOfHandle()
    {
        //Debug.Log(scrollbar.size);
        //rightBorderSecondLayer.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, pixelSize);
        //rightBorderSecondLayer.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

        //bottomBorderSecondLayer.GetComponent<RectTransform>().sizeDelta = new Vector2(pixelSize, transform.GetComponent<RectTransform>().sizeDelta.y);
        //bottomBorderSecondLayer.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

        //topBorder.GetComponent<RectTransform>().sizeDelta = new Vector2(pixelSize, transform.GetComponent<RectTransform>().sizeDelta.y);
        //topBorder.transform.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

        //leftBorder.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, pixelSize);
        //leftBorder.transform.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

        //rightBorder.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x, pixelSize);
        //rightBorder.transform.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

        //bottomBorder.GetComponent<RectTransform>().sizeDelta = new Vector2(pixelSize, transform.GetComponent<RectTransform>().sizeDelta.y);
        //bottomBorder.transform.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
