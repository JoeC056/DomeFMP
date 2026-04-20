using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class CameraScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject computer;
    [SerializeField] private GameObject calendar;

    [Header("Parameters")]
    [SerializeField] private float fovWhenUsingComputer;
    [SerializeField] private Vector3 displacementFromPlayer;
    [SerializeField] private Vector3 displacementFromComputer;
    [SerializeField] private Vector3 displacementFromCalendar;
    [SerializeField] private Vector3 rotationForComputer;
    [SerializeField] private Vector3 rotationForCalendar;


    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        UpdateCameraLocation();
        UpdateCameraFov();
    }

    //////////////////////////////////////////////////////////////////////////////
    private void UpdateCameraLocation()
    {
        //Follows player if exploring level, focuses on computer during its use
        if (GameManager.instance.stateOfGame == GameManager.States.InGame || GameManager.instance.stateOfGame == GameManager.States.InteractingWithObject || GameManager.instance.stateOfGame == GameManager.States.InDaysStartAnimation)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position + displacementFromPlayer, Time.deltaTime * 15);
        }
        else if (GameManager.instance.stateOfGame == GameManager.States.UsingComputer)
        {
            transform.position = computer.transform.position + displacementFromComputer;
            transform.rotation = Quaternion.Euler(rotationForComputer);
        }
        else if (GameManager.instance.stateOfGame == GameManager.States.InteractingWithCalendar)
        {
            transform.position = calendar.transform.position + displacementFromCalendar;
            transform.rotation = Quaternion.Euler(rotationForCalendar);
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    private void UpdateCameraFov()
    {
        if (GameManager.instance.stateOfGame == GameManager.States.UsingComputer)
        {
            GetComponent<Camera>().fieldOfView = fovWhenUsingComputer;
        }
        else
        {
            GetComponent<Camera>().fieldOfView = SettingsManager.instance.FieldOfView;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
