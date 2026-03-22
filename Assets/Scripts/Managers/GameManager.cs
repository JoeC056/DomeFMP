using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

//////////////////////////////////////////////////////////////////////////////
public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Checklist checklistScript;
    [SerializeField] private CharacterRender characterRenderScript;
    [SerializeField] private Transcript transcriptScript;
    [SerializeField] private Documents documentsScript;
    [SerializeField] private SubmitButton submitButtonScript;
    [SerializeField] private GameplayApplicationMenu gameplayApplicationMenuScript;

    [Header("Parameters")]
    [SerializeField] private float delayBetweenEncounters;
    public float scoreForCorrectEntryAllowance;
    public float penaltyForIncorrectEntryAllowance;
    public float scoreForCorrectReasonAllowance;
    public float penaltyForIncorrectReasonAllowance;

    [Header("Day 1 Content")]
    [SerializeField] private List<EncounterSO> day1ListOfAvailableEncounters;
    [SerializeField] private float day1GameplaySegmentDurationMinutes;
    [SerializeField] private List<string> day1AspectsToCheck;
    [SerializeField] private Vector2 day1CharacterRenderRotationClamp;

    [Header("Day 2 Content")]
    [SerializeField] private List<EncounterSO> day2ListOfAvailableEncounters;
    [SerializeField] private float day2GameplaySegmentDurationMinutes;
    [SerializeField] private List<string> day2AspectsToCheck;
    [SerializeField] private Vector2 day2CharacterRenderRotationClamp;

    [Header("Day 3 Content")]
    [SerializeField] private List<EncounterSO> day3ListOfAvailableEncounters;
    [SerializeField] private float day3GameplaySegmentDurationMinutes;
    [SerializeField] private List<string> day3AspectsToCheck;
    [SerializeField] private Vector2 day3CharacterRenderRotationClamp;

    [Header("Day 4 Content")]
    [SerializeField] private List<EncounterSO> day4ListOfAvailableEncounters;
    [SerializeField] private float day4GameplaySegmentDurationMinutes;
    [SerializeField] private List<string> day4AspectsToCheck;
    [SerializeField] private Vector2 day4CharacterRenderRotationClamp;

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
    [HideInInspector] public float score;

    public int dayNo;


    public bool gameplayInProgress;
    private float timer;

    private List<EncounterSO> todaysEncounters;
    private float todaysGameplaySegmentDurationMinutes;
    private List<string> todaysAspectsToCheck;
    private Vector2 todaysCharacterRenderRotationClamp;

    private EncounterSO currentEncounter;
    public bool daysGameplayCompleted;


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
        if (gameplayInProgress)
        {
            UpdateTimer();
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
        if (currentEncounter != null)
        {
            if (todaysEncounters.Count <= 0)
            {
                EndDaysGameplay();
                gameplayApplicationMenuScript.ConcludeGameplay();
            }
            else if (CheckForTimerExpired() && CheckForRemainingMandatoryEncounters(todaysEncounters))
            {
                ConciseListForMandatoryEncounters();
                StartCoroutine(StartNewEncounterAfterDelay(delayBetweenEncounters));
            }
            else if (CheckForTimerExpired() && !CheckForRemainingMandatoryEncounters(todaysEncounters) && currentEncounter.mandatory)
            {
                if (todaysEncounters.Count > 1)
                {
                    todaysEncounters.RemoveRange(1, todaysEncounters.Count - 1);
                }
                StartCoroutine(StartNewEncounterAfterDelay(delayBetweenEncounters));
            }
            else if (CheckForTimerExpired() && !CheckForRemainingMandatoryEncounters(todaysEncounters) && !currentEncounter.mandatory)
            {
                EndDaysGameplay();
                gameplayApplicationMenuScript.ConcludeGameplay();
            }
            else
            {
                StartCoroutine(StartNewEncounterAfterDelay(delayBetweenEncounters));
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    private void AddScore()
    {
        score += checklistScript.CalculateScoreToAddBasedOnAnswers(currentEncounter.correctAnswers, currentEncounter.entryShouldBeAllowed);
        Debug.Log(score);
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ProgressToNextDay()
    {
        Debug.Log("Next day incoming...");
    }

    //////////////////////////////////////////////////////////////////////////////
    private void UpdateTimer()
    {
        timer += Time.deltaTime;
    }

    //////////////////////////////////////////////////////////////////////////////
    public void StartDaysGameplay()
    {
        timer = 0;
        gameplayInProgress = true;

        AssignGameplayValuesForDay();
        RandomizeEncounters();

        checklistScript.ClearList();
        checklistScript.AssignNewChecklistValues(todaysAspectsToCheck);

        AssignValuesFromSO(todaysEncounters[0]);
    }

    //////////////////////////////////////////////////////////////////////////////
    private void AssignValuesFromSO(EncounterSO encounter)
    {
        if (encounter.render != null)
        {
            characterRenderScript.AddNewCharacterRender(encounter.render, todaysCharacterRenderRotationClamp.x, todaysCharacterRenderRotationClamp.y);
        }
        if (encounter.dialogue != null)
        {
            transcriptScript.AssignNewTranscriptDialogue(encounter.dialogue);
        }
        if (encounter.documents.Count > 0)
        {
            documentsScript.AddNewDocumentsToDisplay(encounter.documents);
        }

        checklistScript.ResetList();

        currentEncounter = encounter;
        todaysEncounters.Remove(encounter);
    }

    //////////////////////////////////////////////////////////////////////////////
    private void AssignGameplayValuesForDay()
    {
        switch (dayNo)
        {
            case 0:
                break;
            case 1:
                todaysEncounters = day1ListOfAvailableEncounters;
                todaysGameplaySegmentDurationMinutes = day1GameplaySegmentDurationMinutes;
                todaysAspectsToCheck = day1AspectsToCheck;
                todaysCharacterRenderRotationClamp = day1CharacterRenderRotationClamp;
                break;
            case 2:
                todaysEncounters = day2ListOfAvailableEncounters;
                todaysGameplaySegmentDurationMinutes = day2GameplaySegmentDurationMinutes;
                todaysAspectsToCheck = day2AspectsToCheck;
                todaysCharacterRenderRotationClamp = day2CharacterRenderRotationClamp;
                break;
            case 3:
                todaysEncounters = day3ListOfAvailableEncounters;
                todaysGameplaySegmentDurationMinutes = day3GameplaySegmentDurationMinutes;
                todaysAspectsToCheck = day3AspectsToCheck;
                todaysCharacterRenderRotationClamp = day3CharacterRenderRotationClamp;
                break;
            case 4:
                todaysEncounters = day4ListOfAvailableEncounters;
                todaysGameplaySegmentDurationMinutes = day4GameplaySegmentDurationMinutes;
                todaysAspectsToCheck = day4AspectsToCheck;
                todaysCharacterRenderRotationClamp = day4CharacterRenderRotationClamp;
                break;
            case 5:
                break;
        }
    }
    //////////////////////////////////////////////////////////////////////////////
    private void ClearUI()
    {
        characterRenderScript.RemoveCurrentlyRenderedCharacter();
        transcriptScript.ClearDialogue();
        documentsScript.RemoveCurrentDocuments();
        checklistScript.ResetList();
    }

    //////////////////////////////////////////////////////////////////////////////
    private IEnumerator StartNewEncounterAfterDelay(float duration)
    {
        AddScore();
        ClearUI();
        currentEncounter = null;
        yield return new WaitForSeconds(duration);
        AssignValuesFromSO(todaysEncounters[0]);
    }


    //////////////////////////////////////////////////////////////////////////////
    private bool CheckForTimerExpired()
    {
        if (timer >= todaysGameplaySegmentDurationMinutes * 60)
        {
            return true;
        }
        return false;
    }

    //////////////////////////////////////////////////////////////////////////////
    private void EndDaysGameplay()
    {
        gameplayInProgress = false;
        daysGameplayCompleted = true;
    }

    //////////////////////////////////////////////////////////////////////////////
    private bool CheckForRemainingMandatoryEncounters(List<EncounterSO> listToCheck)
    {
        foreach (EncounterSO encounter in listToCheck)
        {
            if (encounter.mandatory)
            {
                return true;
            }
        }
        return false;
    }

    //////////////////////////////////////////////////////////////////////////////
    private void RandomizeEncounters()
    {
        //List<EncounterSO> randomizedEncounters = new List<EncounterSO>();
        //List<EncounterSO> mandatoryEncounters = new List<EncounterSO>();

        //foreach (EncounterSO encounter in todaysEncounters)
        //{
        //    if (encounter.mandatory)
        //    {
        //        mandatoryEncounters.Add(encounter);
        //    }
        //    else
        //    {
        //        randomizedEncounters.Add(encounter);
        //    }
        //}
    }

    //////////////////////////////////////////////////////////////////////////////
    private void ConciseListForMandatoryEncounters()
    {
        List<EncounterSO> newList = new List<EncounterSO>();

        foreach (EncounterSO encounter in todaysEncounters)
        {
            newList.Add(encounter);
        }

        bool gapPassed = !currentEncounter.mandatory;



        foreach (EncounterSO encounter in todaysEncounters)
        {
            if (gapPassed)
            {
                if (encounter.mandatory)
                {
                    gapPassed = false;
                }
                else
                {
                    newList.Remove(encounter);
                }
            }
            else if (!encounter.mandatory)
            {
                gapPassed = true;
            }
        }

        todaysEncounters = newList;
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
