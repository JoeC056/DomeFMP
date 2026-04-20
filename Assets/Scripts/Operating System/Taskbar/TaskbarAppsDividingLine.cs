using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class TaskbarAppsDividingLine : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject tasksParent;
    [SerializeField] private GameObject homeButtonDividingLine;

    [Header("Parameters")]
    [SerializeField] private float displacementFromRightmostTaskbarAppIcon;

    private RectTransform rectTransform;
    private float defaultXPosition;


    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        defaultXPosition = GetComponent<RectTransform>().anchoredPosition.x;
    }
    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (tasksParent.transform.childCount > 0)
        {
            rectTransform.anchoredPosition = new Vector2(defaultXPosition + displacementFromRightmostTaskbarAppIcon, 0);
        }
        else
        {
            rectTransform.anchoredPosition = new Vector2(defaultXPosition + displacementFromRightmostTaskbarAppIcon, 0);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
