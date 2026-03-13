using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
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
        InteractingWithObject,
        InteractingWithCalendar
    }
    [HideInInspector] public States stateOfGame;

    //Score shared between days
    [HideInInspector] public int score;

    public int dayNo;

    //For testing only
    public GameObject testModel;
    public Dialogue testDialogue;
    public List<GameObject> testDocuments;
    public List<string> testStuffToCheck;
    public bool testShouldBeAllowed;
    public List<bool> testAnswers;


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

        //For testing only
        characterRenderScript.AddNewCharacterRender(testModel, -3600, 3600);
        documentsScript.AddNewDocumentsToDisplay(testDocuments);
        checklistScript.AssignNewChecklistValues(testStuffToCheck, testShouldBeAllowed, testAnswers);


    }

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        UpdateDisplayedMouse();

        //For testing only
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transcriptScript.AssignNewTranscriptDialogue(testDialogue);
        }

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
        Debug.Log("Data submitted....");
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ProgressToNextDay()
    {
        Debug.Log("Next day incoming...");
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
