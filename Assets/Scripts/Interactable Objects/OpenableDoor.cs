using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class OpenableDoor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject doorHingePivot;

    [Header("Parameters")]
    [SerializeField] private float startRot;
    [SerializeField] private float openRot;

    private bool doorOpen;

    //////////////////////////////////////////////////////////////////////////////
    public void ToggleDoor()
    {
        doorOpen = !doorOpen;
        if (doorOpen)
        {
            doorHingePivot.transform.rotation = Quaternion.Euler(0,openRot,0);
        }
        else
        {
            doorHingePivot.transform.rotation = Quaternion.Euler(0, startRot, 0);
        }

    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
