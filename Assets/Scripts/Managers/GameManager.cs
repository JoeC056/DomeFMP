using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private TextMeshProUGUI encounterInfoText;

    [Header("Parameters")]
    [SerializeField] private float delayBetweenEncounters;
    public float scoreForCorrectEntryAllowance;
    public float penaltyForIncorrectEntryAllowance;
    public float scoreForCorrectReasonAllowance;
    public float penaltyForIncorrectReasonAllowance;


    [Header("Day 1 Gameplay")]
    [SerializeField] private List<EncounterSO> day1ListOfAvailableEncounters;
    [SerializeField] private float day1GameplaySegmentDurationMinutes;
    [SerializeField] private List<string> day1AspectsToCheck;
    [SerializeField] private Vector2 day1CharacterRenderRotationClamp;

    [Header("Day 2 Gameplay")]
    [SerializeField] private List<EncounterSO> day2ListOfAvailableEncounters;
    [SerializeField] private float day2GameplaySegmentDurationMinutes;
    [SerializeField] private List<string> day2AspectsToCheck;
    [SerializeField] private Vector2 day2CharacterRenderRotationClamp;

    [Header("Day 2 2nd Half Gameplay")]
    [SerializeField] private List<EncounterSO> day2SecondHalfListOfAvailableEncounters;
    [SerializeField] private List<string> day2SecondHalfAspectsToCheck;
    [SerializeField] private Vector2 day2SecondHalfCharacterRenderRotationClamp;

    [Header("Day 3 Gameplay")]
    [SerializeField] private List<EncounterSO> day3ListOfAvailableEncounters;
    [SerializeField] private float day3GameplaySegmentDurationMinutes;
    [SerializeField] private List<string> day3AspectsToCheck;
    [SerializeField] private Vector2 day3CharacterRenderRotationClamp;

    [Header("Day 3 2nd Half Gameplay")]
    [SerializeField] private List<EncounterSO> day3SecondHalfListOfAvailableEncounters;
    [SerializeField] private List<string> day3SecondHalfAspectsToCheck;
    [SerializeField] private Vector2 day3SecondHalfCharacterRenderRotationClamp;

    [Header("Day 4 Gameplay")]
    [SerializeField] private List<EncounterSO> day4ListOfAvailableEncounters;
    [SerializeField] private float day4GameplaySegmentDurationMinutes;
    [SerializeField] private List<string> day4AspectsToCheck;
    [SerializeField] private Vector2 day4CharacterRenderRotationClamp;

    [Header("Day 2 2nd Half Gameplay")]
    [SerializeField] private List<EncounterSO> day4SecondHalfListOfAvailableEncounters;
    [SerializeField] private List<string> day4SecondHalfAspectsToCheck;
    [SerializeField] private Vector2 day4SecondHalfCharacterRenderRotationClamp;


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
    [HideInInspector] public bool gameplayInProgress;
    [HideInInspector] public bool daysGameplayCompleted;
    [HideInInspector] public bool daysGameplayAvailable;

    //Temporarily held variables for gameplay of a given day
    private List<EncounterSO> todaysEncounters;
    private float todaysGameplaySegmentDurationMinutes;
    private List<string> todaysAspectsToCheck;
    private Vector2 todaysCharacterRenderRotationClamp;
    private EncounterSO currentEncounter;
    private float timer;


    //Variables kept over the course of multiple days
    [HideInInspector] public float score;
    [HideInInspector] public int dayNo;


    //Event to be called on completion of day's gameplay 
    private UnityEvent eventOnDaysCompletion;

    private bool secondHalfStarted;

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
    private void Start()
    {
        BeginGame();
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
        //Checks that there is an ongoing encounter 
        if (currentEncounter != null)
        {
            //If all encounters met (even before timer expires)
            if (todaysEncounters.Count <= 0)
            {
                EndDaysGameplay();
                gameplayApplicationMenuScript.ConcludeGameplay();
            }

            //Checks whether there are remaining mandatory encounters to be had, and how far away they are
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
            //If timer expired and all mandatory encounters met
            else if (CheckForTimerExpired() && !CheckForRemainingMandatoryEncounters(todaysEncounters) && !currentEncounter.mandatory)
            {
                EndDaysGameplay();
                gameplayApplicationMenuScript.ConcludeGameplay();
            }
            //If more encounters to be had
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
    }

    //////////////////////////////////////////////////////////////////////////////
    private void BeginGame()
    {
        DaysProgressionManager.instance.ProgressDay0();
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ProgressToNextDay()
    {
        //Updates respective variables for new day
        daysGameplayAvailable = false;
        dayNo++;
        DaysProgressionManager.instance.daysProgressIndex = 0;
        
        //Progesses through first event of given day 
        switch (dayNo)
        {
            case 1:
                DaysProgressionManager.instance.ProgressDay1();
                break;
            case 2:
                DaysProgressionManager.instance.ProgressDay2();
                break;
            case 3:
                DaysProgressionManager.instance.ProgressDay3();
                break;
            case 4:
                DaysProgressionManager.instance.ProgressDay4();
                break;
            case 5:
                DaysProgressionManager.instance.ProgressDay5();
                break;
        }
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
            documentsScript.AddNewDocumentsToDisplay(encounter.documents, encounter);
        }

        checklistScript.ResetList();

        currentEncounter = encounter;
        todaysEncounters.Remove(encounter);
        encounterInfoText.text = encounter.encounterName + "\n" + encounter.height + "cm \n" + encounter.weight + "kg";
    }

    //////////////////////////////////////////////////////////////////////////////
    private void AssignGameplayValuesForDay()
    {
        if (dayNo >= 2 && secondHalfStarted)
        {
            switch (dayNo)
            {
                case 2:
                    todaysEncounters = day2SecondHalfListOfAvailableEncounters;
                    todaysAspectsToCheck = day2SecondHalfAspectsToCheck;
                    todaysCharacterRenderRotationClamp = day2SecondHalfCharacterRenderRotationClamp;
                    break;
                case 3:
                    todaysEncounters = day3SecondHalfListOfAvailableEncounters;
                    todaysAspectsToCheck = day3SecondHalfAspectsToCheck;
                    todaysCharacterRenderRotationClamp = day3SecondHalfCharacterRenderRotationClamp;
                    break;
                case 4:
                    todaysEncounters = day4SecondHalfListOfAvailableEncounters;
                    todaysAspectsToCheck = day4SecondHalfAspectsToCheck;
                    todaysCharacterRenderRotationClamp = day4SecondHalfCharacterRenderRotationClamp;
                    break;
            }
        }
        else
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
        if (dayNo >= 2)
        {
            if (timer >= todaysGameplaySegmentDurationMinutes * 60 /2)
            {
                return true;
            }
        }
        else
        {
            if (timer >= todaysGameplaySegmentDurationMinutes * 60)
            {
                return true;
            }
        }
        return false;
    }

    //////////////////////////////////////////////////////////////////////////////
    private void EndDaysGameplay()
    {
        if (dayNo >= 2 && !secondHalfStarted)
        {
            ProgressToSecondHalf();
        }
        else
        {
            secondHalfStarted = false;
            gameplayInProgress = false;
            daysGameplayCompleted = true;
            eventOnDaysCompletion.Invoke();
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    private void ProgressToSecondHalf()
    {
        secondHalfStarted = true;

        AssignGameplayValuesForDay();
        RandomizeEncounters();

        checklistScript.ClearList();
        checklistScript.AssignNewChecklistValues(todaysAspectsToCheck);

        AssignValuesFromSO(todaysEncounters[0]);
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
        List<EncounterSO> randomizedEncounters = new List<EncounterSO>();
        List<EncounterSO> mandatoryEncounters = new List<EncounterSO>();

        foreach (EncounterSO encounter in todaysEncounters)
        {
            if (encounter.mandatory)
            {
                mandatoryEncounters.Add(encounter);
            }
            else
            {
                randomizedEncounters.Add(encounter);
            }
        }
        RandomizeList(randomizedEncounters);

        System.Random random = new System.Random();
        foreach (EncounterSO encounter in mandatoryEncounters)
        {
            randomizedEncounters.Insert(encounter.minEncountersBeforeAppearance, encounter);
        }

        todaysEncounters = randomizedEncounters;
    }


    //////////////////////////////////////////////////////////////////////////////
    private void RandomizeList<T>(IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
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
    public void AssignEventOnDaysGameplayCompletion(UnityEvent eventOnCompletion)
    {
        eventOnDaysCompletion = eventOnCompletion;
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
