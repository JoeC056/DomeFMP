using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class PlayerInteractionController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;

    [Header("Parameters")]
    [SerializeField] private float interactionRange;

    //Reference to hit object when checking for interactable object
    private RaycastHit hit;

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        GetInput();
    }

    //////////////////////////////////////////////////////////////////////////////
    private void GetInput()
    {
        //Interacts with object if it is an interactable object
        if (Input.GetKeyDown(InputManager.instance.interactKey) && CheckForInteractableObject() && GameManager.instance.stateOfGame == GameManager.States.InGame)
        {
            Interact();
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    private bool CheckForInteractableObject()
    {
        //Returns true if interactable object in range
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange))
        {
            return true;
        }
        return false;
    }

    //////////////////////////////////////////////////////////////////////////////
    private void Interact()
    {
        //Calls interact function of object being interacted with 
        hit.transform.GetComponent<InteractableObject>().OnInteract();
    }

    //////////////////////////////////////////////////////////////////////////////

}

//////////////////////////////////////////////////////////////////////////////
