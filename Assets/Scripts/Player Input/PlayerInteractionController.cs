using System.Collections;
using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class PlayerInteractionController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private TextMeshProUGUI textOnInteractHover;
    [SerializeField] private GameObject interactionTutorialisationText;

    [Header("Parameters")]
    [SerializeField] private float interactionRange;
    [SerializeField] private float tutorialisationTextDuration;

    //Reference to hit object when checking for interactable object
    private RaycastHit hit;

    public bool firstInteractionDone;

    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        interactionTutorialisationText.SetActive(false);
    }

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
        if (!firstInteractionDone && hit.transform.GetComponent<OpenableDoor>() == null)
        {
            firstInteractionDone = true;
            if (hit.transform.GetComponent<ComputerInteractable>() == null)
            {
                StartCoroutine(DisplayInteractionTutorialisation());
            }
        }

        //Halts player movement
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        //Calls interact function of object being interacted with 
        hit.transform.GetComponent<InteractableObject>().OnInteract();
    
    }

    //////////////////////////////////////////////////////////////////////////////
    private IEnumerator DisplayInteractionTutorialisation()
    {
        interactionTutorialisationText.SetActive(true);
        yield return new WaitForSeconds(tutorialisationTextDuration);
        interactionTutorialisationText.SetActive(false);
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
