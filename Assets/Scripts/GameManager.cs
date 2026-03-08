using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Checklist checklistScript;
    [SerializeField] private CharacterRender characterRenderScript;
    [SerializeField] private Transcript transcriptScript;
    [SerializeField] private Documents documentsScript;
    [SerializeField] private SubmitButton submitButtonScript;

    //Instance of GameManager
    public static GameManager instance { get; private set; }

    //Current state of game
    public enum States
    {
        InGame,
        UsingComputer,
    }
    public States stateOfGame;

    //Score shared between days
    public int score;


    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        //Ensures singleton nature of instance variable
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        UpdateDisplayedMouse();
    }

    //////////////////////////////////////////////////////////////////////////////
    private void UpdateDisplayedMouse()
    {
        //Hidden when exploring level, visible when using computer or paused
        if (stateOfGame == States.InGame && Time.timeScale != 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void Submit()
    {

    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
