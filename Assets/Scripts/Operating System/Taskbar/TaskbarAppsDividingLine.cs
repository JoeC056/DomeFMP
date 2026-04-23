using Unity.Multiplayer.Center.Common;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class TaskbarAppsDividingLine : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject tasksParent;
    [SerializeField] private GameObject homeButtonDividingLine;

    [Header("Parameters")]
    [SerializeField] private float defaultDisplacement;
    [SerializeField] private float displacementPerTask;

    private RectTransform rectTransform;
    private float defaultXPosition;


    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        defaultXPosition = rectTransform.anchoredPosition.x;
    }
    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (defaultXPosition != 0)
        {
            if (tasksParent.transform.childCount > 0)
            {
                rectTransform.anchoredPosition = new Vector2(defaultXPosition + defaultDisplacement + (displacementPerTask * tasksParent.transform.childCount), 0);
            }
            else
            {
                rectTransform.anchoredPosition = new Vector2(defaultXPosition, 0);
            }

        }

    }

    //////////////////////////////////////////////////////////////////////////////////
}
//////////////////////////////////////////////////////////////////////////////////
