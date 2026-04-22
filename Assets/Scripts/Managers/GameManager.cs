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
    [SerializeField] private MessagingApplication messagingApplication;
    [SerializeField] private TextMeshProUGUI encounterInfoText;
    [SerializeField] private GameObject player;
    [SerializeField] private Notes notes;
    [SerializeField] private TextMeshProUGUI endOfDayRatingText;
    [SerializeField] private ComputerInteractable computerInteractable;
    [SerializeField] private IncorrectAnswersNotice incorrectAnswersNotice;
    [SerializeField] private GameObject daysStartUI;
    [SerializeField] private Subtitles subtitles;
    [SerializeField] private GameObject characterRenderNAText;
    [SerializeField] private GameObject documentsNAText;

    [Header("Parameters")]
    [SerializeField] private float delayBetweenEncounters;
    [SerializeField] private float dayStartAnimationDelayBeforeFadeIn;
    [SerializeField] private float dayStartAnimationTextFadeInOutSpeed;
    [SerializeField] private float dayStartAnimationTextDuration;
    public float messageGrowthSpeed;
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

    [Header("Day 4 2nd Half Gameplay")]
    [SerializeField] private List<EncounterSO> day4SecondHalfListOfAvailableEncounters;
    [SerializeField] private List<string> day4SecondHalfAspectsToCheck;
    [SerializeField] private Vector2 day4SecondHalfCharacterRenderRotationClamp;

    [Header("Midway through day update messages")]
    [SerializeField] private MessagingDialogueSO day2Half2Update;
    [SerializeField] private MessagingDialogueSO day3Half2Update;
    [SerializeField] private MessagingDialogueSO day4Half2Update;

    [Header("Spawn points for each day")]
    [SerializeField] private Vector3 bedSpawnPoint;


    //Instance of GameManager
    public static GameManager instance { get; private set; }

    //Current state of game
    public enum States
    {
        InGame,
        UsingComputer,
        InteractingWithObject,
        InteractingWithCalendar,
        WatchingEndingSequence,
        InDaysStartAnimation
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
    private float maxScoreForDay;
    [HideInInspector] public int dayNo;


    //Event to be called on completion of day's gameplay 
    private UnityEvent eventOnDaysCompletion;

    [HideInInspector] public bool secondHalfStarted;

    private bool inDaysStartAnimation;
    private bool waitedDurationOfText;
    private bool waitingDurationOfText;


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
        notes.CreateSingleton();
        StartCoroutine(StartDaysStartAnimation());

        characterRenderNAText.SetActive(false);
        documentsNAText.SetActive(false);
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
    private void FixedUpdate()
    {
        if (inDaysStartAnimation)
        {
            //waitedDurationOfText
            if (!waitedDurationOfText)
            {
                if (!waitingDurationOfText)
                {
                    if (daysStartUI.transform.GetChild(0).GetComponent<CanvasGroup>().alpha == 1)
                    {
                        StartCoroutine(WaitDurationOfText());  
                    }
                    else
                    {
                        daysStartUI.transform.GetChild(0).GetComponent<CanvasGroup>().alpha += dayStartAnimationTextFadeInOutSpeed;
                    }
                }
            }
            else
            {
                if (daysStartUI.transform.GetComponent<CanvasGroup>().alpha <= 0)
                {
                    daysStartUI.SetActive(false);
                    inDaysStartAnimation = false;
                    stateOfGame = States.InGame;
                }
                else
                {
                    daysStartUI.transform.GetChild(0).GetComponent<CanvasGroup>().alpha -= dayStartAnimationTextFadeInOutSpeed;
                    if (daysStartUI.transform.GetChild(0).GetComponent<CanvasGroup>().alpha <= 0.5)
                    {
                        daysStartUI.transform.GetComponent<CanvasGroup>().alpha -= dayStartAnimationTextFadeInOutSpeed;
                    }
                }
            }
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
        score += checklistScript.CalculateScoreToAddBasedOnAnswers(todaysAspectsToCheck,currentEncounter.correctAnswers, currentEncounter.entryShouldBeAllowed);
        maxScoreForDay += scoreForCorrectEntryAllowance + (scoreForCorrectReasonAllowance * todaysAspectsToCheck.Count);
        endOfDayRatingText.text = ((score / maxScoreForDay)*100) + "%";
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
        daysGameplayCompleted = false;
        dayNo++;
        DaysProgressionManager.instance.daysProgressIndex = 0;

        Notes.instance.ResetSubNotes();
        messagingApplication.ClearAvailableConversations();
        AssignSpawnPointForDay();
        computerInteractable.TurnOffComputer();

        score = 0;
        maxScoreForDay = 0;
        incorrectAnswersNotice.HideNotice();
        subtitles.ClearSubtitles();

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

        StartCoroutine(StartDaysStartAnimation());
    }

    //////////////////////////////////////////////////////////////////////////////
    private void AssignSpawnPointForDay()
    {
        player.transform.position = bedSpawnPoint;
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
            characterRenderScript.AddNewCharacterRender(encounter.render, todaysCharacterRenderRotationClamp.x, todaysCharacterRenderRotationClamp.y, encounter);
        }
        if (encounter.dialogue != null)
        {
            transcriptScript.AssignNewTranscriptDialogue(encounter.dialogue);
        }
        

        if (encounter.documents.Count > 0)
        {
            List<GameObject> documents = new List<GameObject>();
            foreach (GameObject document in encounter.documents)
            {
                documents.Add(document);
            }
            documentsScript.AddNewDocumentsToDisplay(documents, encounter);
        }

        characterRenderNAText.SetActive(encounter.render == null);
        documentsNAText.SetActive(encounter.documents.Count <= 0);

        checklistScript.ResetList();

        currentEncounter = encounter;
        todaysEncounters.Remove(encounter);
        encounterInfoText.text = encounter.encounterName + "\n" + encounter.height + "cm \n" + (encounter.weight + encounter.changeInWeight) + "kg";
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
        encounterInfoText.text = "";
    }

    //////////////////////////////////////////////////////////////////////////////
    private IEnumerator StartNewEncounterAfterDelay(float duration)
    {
        if (!currentEncounter.mandatory)
        {
            AddScore();
        }
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
        ClearUI();

        if (dayNo >= 2 && !secondHalfStarted)
        {
            ProgressToSecondHalf(); //StartCoroutine(ProgressToSecondHalf())
        }
        else
        {
            secondHalfStarted = false;
            gameplayInProgress = false;
            daysGameplayCompleted = true;
            eventOnDaysCompletion.Invoke();
            gameplayApplicationMenuScript.ConcludeGameplay();
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    private void ProgressToSecondHalf()
    {
        secondHalfStarted = true;

        AssignGameplayValuesForDay();
        RandomizeEncounters();


        if (dayNo == 2)
        {
            messagingApplication.AddNewAvailableConversation(day2Half2Update);
        }
        else if (dayNo == 3)
        {
            messagingApplication.AddNewAvailableConversation(day3Half2Update);
        }
        else if (dayNo == 4)
        {
            messagingApplication.AddNewAvailableConversation(day4Half2Update);
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void StartSecondHalf()
    {
        StartCoroutine(CommenceSecondHalf());
    }

    //////////////////////////////////////////////////////////////////////////////
    private IEnumerator CommenceSecondHalf()
    {
        yield return new WaitForSeconds(delayBetweenEncounters);

        incorrectAnswersNotice.HideNotice();
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
        System.Random rnGenerator = new System.Random();
        int count = list.Count;
        while (count > 1)
        {
            count--;
            int nextVal = rnGenerator.Next(count + 1);
            T value = list[nextVal];
            list[nextVal] = list[count];
            list[count] = value;
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
    private IEnumerator StartDaysStartAnimation()
    {
        stateOfGame = States.InDaysStartAnimation;
        daysStartUI.SetActive(true);
        daysStartUI.GetComponent<CanvasGroup>().alpha = 1;
        daysStartUI.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 0;
        daysStartUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Day " + dayNo;
        waitedDurationOfText = false;
        waitingDurationOfText = false;
        yield return new WaitForSeconds(dayStartAnimationDelayBeforeFadeIn);
        inDaysStartAnimation = true;
    }

    //////////////////////////////////////////////////////////////////////////////
    private IEnumerator WaitDurationOfText()
    {
        waitingDurationOfText = true;
        yield return new WaitForSeconds(dayStartAnimationTextDuration);
        waitedDurationOfText = true;
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
