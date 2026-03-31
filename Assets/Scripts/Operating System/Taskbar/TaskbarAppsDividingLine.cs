using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class TaskbarAppsDividingLine : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject tasksParent;
    [SerializeField] private GameObject homeButtonDividingLine;

    [Header("Parameters")]
    [SerializeField] private Vector3 displacementFromRightmostTaskbarAppIcon;

    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (tasksParent.transform.childCount > 0)
        {
            transform.position = tasksParent.transform.GetChild(tasksParent.transform.childCount -1).transform.position + displacementFromRightmostTaskbarAppIcon;
        }
        else
        {
            transform.position = homeButtonDividingLine.transform.position;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
