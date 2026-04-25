using UnityEngine;
using UnityEngine.Events;

//////////////////////////////////////////////////////////////////////////////////
public class FrontDoor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PauseMenu pauseMenuScript;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject endingChoiceSelectUI;


    private InteractableObject interactableObjectScript;


    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        interactableObjectScript = GetComponent<InteractableObject>();
        interactableObjectScript.interactable = false;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (!pauseMenuScript.paused)
        {
            endingChoiceSelectUI.SetActive(false);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void InteractWithDoor()
    {
        pauseMenuScript.TogglePause();
        GameManager.instance.stateOfGame = GameManager.States.WatchingEndingSequence;
        pauseMenuUI.SetActive(false);
        hud.SetActive(true);
        endingChoiceSelectUI.SetActive(true);
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void ToggleInteractionAllowance()
    {
        interactableObjectScript.interactable = !interactableObjectScript.interactable;
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////


