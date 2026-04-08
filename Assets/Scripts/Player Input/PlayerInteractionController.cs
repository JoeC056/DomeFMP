using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class PlayerInteractionController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private TextMeshProUGUI textOnInteractHover;

    [Header("Parameters")]
    [SerializeField] private float interactionRange;

    //Reference to hit object when checking for interactable object
    private RaycastHit hit;

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (CheckForInteractableObject())
        {
            if (hit.transform.gameObject.GetComponent<InteractableObject>().textOnInteractionHover != "")
            {
                textOnInteractHover.text = "[E] " + hit.transform.gameObject.GetComponent<InteractableObject>().textOnInteractionHover;
            }
        }
        else
        {
            textOnInteractHover.text = "";
        }
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
        Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange);
        if (hit.transform != null)
        {
            if (hit.transform.GetComponent<InteractableObject>() != null)
            {
                if (hit.transform.GetComponent<InteractableObject>().interactable)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //////////////////////////////////////////////////////////////////////////////
    private void Interact()
    {
        //Halts player movement
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        //Calls interact function of object being interacted with 
        hit.transform.GetComponent<InteractableObject>().OnInteract();
    }

    //////////////////////////////////////////////////////////////////////////////

}

//////////////////////////////////////////////////////////////////////////////
