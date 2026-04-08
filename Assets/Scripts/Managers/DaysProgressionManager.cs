using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//////////////////////////////////////////////////////////////////////////////
public class DaysProgressionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Bed bedScript;
    [SerializeField] private Radio radioScript;
    [SerializeField] private MessagingApplication messagingApplicationScript;
    [SerializeField] private WebBrowser webBrowserScript;
    [SerializeField] private FrontDoor frontDoorScript;
    [SerializeField] private GameObject virusInfoWebsite;
    [SerializeField] private GameObject gameplayApplicationIcon;
    [SerializeField] private GameObject sirenSoundText;


    [Header("Day 0 Content")]
    [SerializeField] private GameObject conversationNotificationTrigger;
    [SerializeField] private Vector3 conversationNotificationTriggerPosition;
    [SerializeField] private GameObject day0NarrationGuidanceTrigger;
    [SerializeField] private Vector3 day0NarrationGuidanceTriggerPosition;
    [SerializeField] private MessagingDialogueSO day0MessagesWithDaughter;


    [Header("Day 1 Content")]
    [SerializeField] private GameObject sirenTrigger;
    [SerializeField] private Vector3 sirenTriggerPosition;
    [SerializeField] private RadioTransmissionSO day1RadioTransmission;
    [SerializeField] private MessagingDialogueSO day1MessagesWithDaughter;
    [SerializeField] private MessagingDialogueSO day1MessageFromGovernment;
    [SerializeField] private MessagingDialogueSO day1MessagesWithCoworker;


    [Header("Day 2 Content")]
    [SerializeField] private RadioTransmissionSO day2RadioTransmission;
    [SerializeField] private MessagingDialogueSO day2MessagesWithCoworker;
    [SerializeField] private MessagingDialogueSO day2MessagesWithFactionMember;


    [Header("Day 3 Content")]
    [SerializeField] private RadioTransmissionSO day3RadioTransmission;
    [SerializeField] private MessagingDialogueSO day3MessagesWithCoworker;
    [SerializeField] private MessagingDialogueSO day3MessagesWithFactionMember;


    [Header("Day 4 Content")]
    [SerializeField] private RadioTransmissionSO day4RadioTransmission;
    [SerializeField] private MessagingDialogueSO day4MessagesWithDaughter;
    [SerializeField] private MessagingDialogueSO day4MessagesWithSpy;
    [SerializeField] private MessagingDialogueSO day4MessageFromGovernment;


    [Header("Day 5 Content")]
    [SerializeField] private MessagingDialogueSO day5MessagesWithFactionMember;
    [SerializeField] private MessagingDialogueSO day5SecondMessagesWithFactionMember;
    [SerializeField] private MessagingDialogueSO day5MessagesWithSpy;
    [SerializeField] private MessagingDialogueSO day5MessagesWithGovernment;


    [Header("Unlockable Websites")]
    [SerializeField] private WebsiteSO domeDevelopmentInfoWebsite;
    [SerializeField] private WebsiteSO spyInvestigationWebsite1;
    [SerializeField] private WebsiteSO spyInvestigationWebsite2;
    [SerializeField] private WebsiteSO spyInvestigationWebsite3;
    [SerializeField] private WebsiteSO cultistPlanWebsite;

    [Header("Subtitles Content")]
    [SerializeField] private string readyForBedMessage;
    [SerializeField] private float readyForBedMessageLifetime;


    //Instance of DaysProgressionManager
    public static DaysProgressionManager instance { get; private set; }

    [HideInInspector] public int daysProgressIndex = 0;


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

        virusInfoWebsite.SetActive(false);
        gameplayApplicationIcon.SetActive(false);
        sirenSoundText.SetActive(false);
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ProgressDay0()
    {
        UnityEvent respectiveEvent = new UnityEvent();
        respectiveEvent.AddListener(ProgressDay0);

        switch (daysProgressIndex)
        {
            case 0:
                Instantiate(day0NarrationGuidanceTrigger, day0NarrationGuidanceTriggerPosition, Quaternion.Euler(Vector3.zero));
                GameObject convoTrigger = Instantiate(conversationNotificationTrigger, conversationNotificationTriggerPosition, Quaternion.Euler(Vector3.zero));
                convoTrigger.GetComponent<ConversationNotificationTrigger>().EventOnCollision.AddListener(ProgressDay0);
                break;
            case 1:
                messagingApplicationScript.AddNewAvailableConversation(day0MessagesWithDaughter);
                messagingApplicationScript.AddEventOnDialogueCompletion(day0MessagesWithDaughter, respectiveEvent);
                break;
            case 2:
                List<string> messagesToDisplay = new List<string>();
                messagesToDisplay.Add(readyForBedMessage);

                Subtitles.instance.DisplaySubtitles(messagesToDisplay, readyForBedMessageLifetime);
                bedScript.EnableInteractionAllowance();
                break;

        }

        daysProgressIndex++;
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ProgressDay1()
    {
        UnityEvent respectiveEvent = new UnityEvent();
        respectiveEvent.AddListener(ProgressDay1);

        switch (daysProgressIndex)
        {
            case 0:
                GameObject sirenEvent = Instantiate(sirenTrigger, sirenTriggerPosition, Quaternion.Euler(Vector3.zero));
                sirenEvent.GetComponent<SirenEvent>().EventOnCollision.AddListener(ProgressDay1);
                sirenEvent.GetComponent<SirenEvent>().sirenSoundText = sirenSoundText;

                List<string> messagesToDisplay = new List<string>();
                messagesToDisplay.Add("Time to head out to work");

                Subtitles.instance.DisplaySubtitles(messagesToDisplay, 3);
                break;
            case 1:
                radioScript.AddNewTransmission(day1RadioTransmission);
                radioScript.AddEventOnTransmissionCompletion(day1RadioTransmission, respectiveEvent);

                List<string> messagesToDisplay2 = new List<string>();
                messagesToDisplay2.Add("!?!?");
                messagesToDisplay2.Add("The radio's going off...");

                Subtitles.instance.DisplaySubtitles(messagesToDisplay2, 2);
                break;
            case 2:
                messagingApplicationScript.AddNewAvailableConversation(day1MessagesWithDaughter);

                messagingApplicationScript.AddNewAvailableConversation(day1MessageFromGovernment);
                messagingApplicationScript.AddEventOnDialogueCompletion(day1MessageFromGovernment, respectiveEvent);
                break;
            case 3:
                virusInfoWebsite.SetActive(true);
                gameplayApplicationIcon.SetActive(true);

                GameManager.instance.daysGameplayAvailable = true;
                GameManager.instance.AssignEventOnDaysGameplayCompletion(respectiveEvent);
                break;
            case 4:
                messagingApplicationScript.AddNewAvailableConversation(day1MessagesWithCoworker);
                messagingApplicationScript.AddEventOnDialogueCompletion(day1MessagesWithCoworker, respectiveEvent);
                break;
            case 5:
                List<string> messagesToDisplay3 = new List<string>();
                messagesToDisplay3.Add(readyForBedMessage);

                Subtitles.instance.DisplaySubtitles(messagesToDisplay3, readyForBedMessageLifetime);
                bedScript.EnableInteractionAllowance();
                break;

        }

        daysProgressIndex++;
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ProgressDay2()
    {
        UnityEvent respectiveEvent = new UnityEvent();
        respectiveEvent.AddListener(ProgressDay2);

        UnityEvent updateNotesEvent = new UnityEvent();
        updateNotesEvent.AddListener(Notes.instance.UnlockDaysSubNotes);

        switch (daysProgressIndex)
        {
            case 0:
                radioScript.AddNewTransmission(day2RadioTransmission);
                messagingApplicationScript.AddNewAvailableConversation(day2MessagesWithCoworker);
                messagingApplicationScript.AddEventOnDialogueCompletion(day2MessagesWithCoworker,updateNotesEvent);

                GameManager.instance.daysGameplayAvailable = true;
                GameManager.instance.AssignEventOnDaysGameplayCompletion(respectiveEvent);
                break;
            case 1:
                messagingApplicationScript.AddNewAvailableConversation(day2MessagesWithFactionMember);
                messagingApplicationScript.AddEventOnDialogueCompletion(day2MessagesWithFactionMember, respectiveEvent);
                break;
            case 2:
                webBrowserScript.UnlockNewWebsite(domeDevelopmentInfoWebsite);

                List<string> messagesToDisplay = new List<string>();
                messagesToDisplay.Add(readyForBedMessage);

                Subtitles.instance.DisplaySubtitles(messagesToDisplay, readyForBedMessageLifetime);
                bedScript.EnableInteractionAllowance();
                break;

        }
        daysProgressIndex++;
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ProgressDay3()
    {
        UnityEvent respectiveEvent = new UnityEvent();
        respectiveEvent.AddListener(ProgressDay3);

        UnityEvent updateNotesEvent = new UnityEvent();
        updateNotesEvent.AddListener(Notes.instance.UnlockDaysSubNotes);

        switch (daysProgressIndex)
        {
            case 0:
                radioScript.AddNewTransmission(day3RadioTransmission);
                messagingApplicationScript.AddNewAvailableConversation(day3MessagesWithCoworker);
                messagingApplicationScript.AddEventOnDialogueCompletion(day3MessagesWithCoworker, updateNotesEvent);

                GameManager.instance.daysGameplayAvailable = true;
                GameManager.instance.AssignEventOnDaysGameplayCompletion(respectiveEvent);
                break;
            case 1:
                messagingApplicationScript.AddNewAvailableConversation(day3MessagesWithFactionMember);
                messagingApplicationScript.AddEventOnDialogueCompletion(day3MessagesWithFactionMember, respectiveEvent);
                break;
            case 2:
                webBrowserScript.UnlockNewWebsite(spyInvestigationWebsite1);

                List<string> messagesToDisplay = new List<string>();
                messagesToDisplay.Add(readyForBedMessage);

                Subtitles.instance.DisplaySubtitles(messagesToDisplay, readyForBedMessageLifetime);
                bedScript.EnableInteractionAllowance();
                break;

        }

        daysProgressIndex++;
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ProgressDay4()
    {
        UnityEvent respectiveEvent = new UnityEvent();
        respectiveEvent.AddListener(ProgressDay4);

        UnityEvent updateNotesEvent = new UnityEvent();
        updateNotesEvent.AddListener(Notes.instance.UnlockDaysSubNotes);

        switch (daysProgressIndex)
        {
            case 0:
                radioScript.AddNewTransmission(day4RadioTransmission);

                messagingApplicationScript.AddNewAvailableConversation(day4MessagesWithDaughter);
                messagingApplicationScript.AddEventOnDialogueCompletion(day4MessagesWithDaughter, updateNotesEvent);

                GameManager.instance.daysGameplayAvailable = true;
                GameManager.instance.AssignEventOnDaysGameplayCompletion(respectiveEvent);
                break;
            case 1:
                messagingApplicationScript.AddNewAvailableConversation(day4MessagesWithSpy);
                messagingApplicationScript.AddEventOnDialogueCompletion(day4MessagesWithSpy, respectiveEvent);
                break;
            case 2:
                webBrowserScript.UnlockNewWebsite(spyInvestigationWebsite2);
                webBrowserScript.AddEventOnWebsiteLoad(spyInvestigationWebsite2, respectiveEvent);
                break;
            case 3:
                messagingApplicationScript.AddNewAvailableConversation(day4MessageFromGovernment);
                messagingApplicationScript.AddEventOnDialogueCompletion(day4MessageFromGovernment, respectiveEvent);
                break;
            case 4:
                List<string> messagesToDisplay = new List<string>();
                messagesToDisplay.Add(readyForBedMessage);

                Subtitles.instance.DisplaySubtitles(messagesToDisplay, readyForBedMessageLifetime);
                bedScript.EnableInteractionAllowance();
                break;

        }

        daysProgressIndex++;
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ProgressDay5()
    {
        UnityEvent respectiveEvent = new UnityEvent();
        respectiveEvent.AddListener(ProgressDay5);

        switch (daysProgressIndex)
        {
            case 0:
                messagingApplicationScript.AddNewAvailableConversation(day5MessagesWithFactionMember);
                messagingApplicationScript.AddEventOnDialogueCompletion(day5MessagesWithFactionMember, respectiveEvent);
                break;
            case 1:
                webBrowserScript.UnlockNewWebsite(spyInvestigationWebsite3);
                WebBrowser.instance.AddEventOnWebsiteLoad(spyInvestigationWebsite3, respectiveEvent);
                break;
            case 2:
                messagingApplicationScript.AddNewAvailableConversation(day5SecondMessagesWithFactionMember);
                messagingApplicationScript.AddEventOnDialogueCompletion(day5SecondMessagesWithFactionMember, respectiveEvent);
                break;
            case 3:
                messagingApplicationScript.AddNewAvailableConversation(day5MessagesWithSpy);
                messagingApplicationScript.AddEventOnDialogueCompletion(day5MessagesWithSpy, respectiveEvent);
                break;
            case 4:
                webBrowserScript.UnlockNewWebsite(cultistPlanWebsite);
                WebBrowser.instance.AddEventOnWebsiteLoad(cultistPlanWebsite, respectiveEvent);
                break;
            case 5:
                messagingApplicationScript.AddNewAvailableConversation(day5MessagesWithGovernment);
                messagingApplicationScript.AddEventOnDialogueCompletion(day5MessagesWithGovernment, respectiveEvent);
                break;
            case 6:
                List<string> messagesToDisplay = new List<string>();
                messagesToDisplay.Add("*Knock Knock*");

                Subtitles.instance.DisplaySubtitles(messagesToDisplay, 4);
                frontDoorScript.ToggleInteractionAllowance();
                break;
        }

        daysProgressIndex++;
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
