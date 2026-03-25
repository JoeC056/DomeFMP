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


    [Header("Day 0 Content")]
    [SerializeField] private GameObject conversationNotificationTrigger;
    [SerializeField] private Vector3 conversationNotificationTriggerPosition;
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
    [SerializeField] private MessagingDialogueSO day2MessagesWithDaughter;
    [SerializeField] private MessagingDialogueSO day2MessagesWithFactionMember;


    [Header("Day 3 Content")]
    [SerializeField] private RadioTransmissionSO day3RadioTransmission;
    [SerializeField] private MessagingDialogueSO day3MessagesWithCoworker;
    [SerializeField] private MessagingDialogueSO day3MessagesWithDaughter;
    [SerializeField] private MessagingDialogueSO day3MessagesWithFactionMember;


    [Header("Day 4 Content")]
    [SerializeField] private RadioTransmissionSO day4RadioTransmission;
    [SerializeField] private MessagingDialogueSO day4MessagesWithDaughter;
    [SerializeField] private MessagingDialogueSO day4MessageFromGovernment;


    [Header("Day 5 Content")]
    [SerializeField] private MessagingDialogueSO day5MessagesWithFactionMember;
    [SerializeField] private MessagingDialogueSO day5MessagesWithSpy;
    [SerializeField] private MessagingDialogueSO day5MessagesWithGovernment;


    [Header("Unlockable Websites")]
    [SerializeField] private WebsiteSO virusInfoWebsite;
    [SerializeField] private WebsiteSO domeDevelopmentInfoWebsite;
    [SerializeField] private WebsiteSO spyInvestigationWebsite1;
    [SerializeField] private WebsiteSO spyInvestigationWebsite2;
    [SerializeField] private WebsiteSO spyInvestigationWebsite3;
    [SerializeField] private WebsiteSO cultistPlanWebsite;


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
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ProgressDay0()
    {
        UnityEvent respectiveEvent = new UnityEvent();
        respectiveEvent.AddListener(ProgressDay0);

        switch (daysProgressIndex)
        {
            case 0:
                GameObject convoTrigger = Instantiate(conversationNotificationTrigger, conversationNotificationTriggerPosition, Quaternion.Euler(Vector3.zero));
                convoTrigger.GetComponent<ConversationNotificationTrigger>().EventOnCollision.AddListener(ProgressDay0);
                break;
            case 1:
                messagingApplicationScript.AddNewAvailableConversation(day0MessagesWithDaughter);
                messagingApplicationScript.AddEventOnDialogueCompletion(day0MessagesWithDaughter, respectiveEvent);
                break;
            case 2:
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
                break;
            case 1:
                radioScript.AddNewTransmission(day1RadioTransmission);
                radioScript.AddEventOnTransmissionCompletion(day1RadioTransmission, respectiveEvent);
                break;
            case 2:
                webBrowserScript.UnlockNewWebsite(virusInfoWebsite);

                messagingApplicationScript.AddNewAvailableConversation(day1MessagesWithDaughter);

                messagingApplicationScript.AddNewAvailableConversation(day1MessageFromGovernment);
                messagingApplicationScript.AddEventOnDialogueCompletion(day1MessageFromGovernment, respectiveEvent);
                break;
            case 3:
                GameManager.instance.daysGameplayAvailable = true;
                GameManager.instance.AssignEventOnDaysGameplayCompletion(respectiveEvent);
                break;
            case 4:
                messagingApplicationScript.AddNewAvailableConversation(day1MessagesWithCoworker);
                messagingApplicationScript.AddEventOnDialogueCompletion(day1MessagesWithCoworker, respectiveEvent);
                break;
            case 5:
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

        switch (daysProgressIndex)
        {
            case 0:
                radioScript.AddNewTransmission(day2RadioTransmission);
                messagingApplicationScript.AddNewAvailableConversation(day2MessagesWithCoworker);

                GameManager.instance.daysGameplayAvailable = true;
                GameManager.instance.AssignEventOnDaysGameplayCompletion(respectiveEvent);
                break;
            case 1:
                messagingApplicationScript.AddNewAvailableConversation(day2MessagesWithDaughter);

                messagingApplicationScript.AddNewAvailableConversation(day2MessagesWithFactionMember);
                messagingApplicationScript.AddEventOnDialogueCompletion(day2MessagesWithFactionMember, respectiveEvent);
                break;
            case 2:
                webBrowserScript.UnlockNewWebsite(domeDevelopmentInfoWebsite);
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

        switch (daysProgressIndex)
        {
            case 0:
                radioScript.AddNewTransmission(day3RadioTransmission);
                messagingApplicationScript.AddNewAvailableConversation(day3MessagesWithCoworker);

                GameManager.instance.daysGameplayAvailable = true;
                GameManager.instance.AssignEventOnDaysGameplayCompletion(respectiveEvent);
                break;
            case 1:
                messagingApplicationScript.AddNewAvailableConversation(day3MessagesWithDaughter);

                messagingApplicationScript.AddNewAvailableConversation(day3MessagesWithFactionMember);
                messagingApplicationScript.AddEventOnDialogueCompletion(day3MessagesWithFactionMember, respectiveEvent);
                break;
            case 2:
                webBrowserScript.UnlockNewWebsite(spyInvestigationWebsite1);
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

        switch (daysProgressIndex)
        {
            case 0:
                radioScript.AddNewTransmission(day4RadioTransmission);

                GameManager.instance.daysGameplayAvailable = true;
                GameManager.instance.AssignEventOnDaysGameplayCompletion(respectiveEvent);
                break;
            case 1:
                webBrowserScript.UnlockNewWebsite(spyInvestigationWebsite2);

                messagingApplicationScript.AddNewAvailableConversation(day4MessagesWithDaughter);

                messagingApplicationScript.AddNewAvailableConversation(day4MessageFromGovernment);
                messagingApplicationScript.AddEventOnDialogueCompletion(day4MessageFromGovernment, respectiveEvent);
                break;
            case 2:
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
                messagingApplicationScript.AddNewAvailableConversation(day5MessagesWithSpy);
                messagingApplicationScript.AddEventOnDialogueCompletion(day5MessagesWithSpy, respectiveEvent);
                break;
            case 3:
                webBrowserScript.UnlockNewWebsite(cultistPlanWebsite);
                WebBrowser.instance.AddEventOnWebsiteLoad(cultistPlanWebsite, respectiveEvent);
                break;
            case 4:
                messagingApplicationScript.AddNewAvailableConversation(day5MessagesWithGovernment);
                //Branch to ending here somehow
                break;
        }

        daysProgressIndex++;
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
