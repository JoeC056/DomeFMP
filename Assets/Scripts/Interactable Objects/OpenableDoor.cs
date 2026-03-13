using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class OpenableDoor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject doorHingePivot;

    [Header("Parameters")]
    [SerializeField] private float dir;

    private bool doorOpen;

    //////////////////////////////////////////////////////////////////////////////
    public void ToggleDoor()
    {
        doorOpen = !doorOpen;
        if (doorOpen)
        {
            doorHingePivot.transform.Rotate(0,-90 * dir,0);
        }
        else
        {
            doorHingePivot.transform.Rotate(0, 90 * dir, 0);
        }

    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
