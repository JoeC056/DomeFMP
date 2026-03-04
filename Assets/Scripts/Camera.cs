using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class Camera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject computer;

    [Header("Parameters")]
    [SerializeField] private Vector3 displacementFromPlayer;
    [SerializeField] private Vector3 displacementFromComputer;
    [SerializeField] private Vector3 rotationForComputer;


    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        UpdateCameraLocation();
    }

    //////////////////////////////////////////////////////////////////////////////
    private void UpdateCameraLocation()
    {
        //Follows player if exploring level, focuses on computer during its use
        if (GameManager.instance.stateOfGame == GameManager.States.InGame)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position + displacementFromPlayer, Time.deltaTime * 15);
        }
        else if (GameManager.instance.stateOfGame == GameManager.States.UsingComputer)
        {
            transform.position = computer.transform.position + displacementFromComputer;
            transform.rotation = Quaternion.Euler(rotationForComputer);
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
