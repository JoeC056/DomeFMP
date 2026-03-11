using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class AvailableConversation : MonoBehaviour
{
    [Header("References")]
    private MessagingApplication messagingApplication;
    private MessagingRecipientSO recipientOfThis;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI mostRecentMessageText;
    [SerializeField] private GameObject notificationIcon;

    //Currently used dialogue
    private MessagingDialogueSO currentDialogue;

    //Content of the dialogue
    private List<string> remainingDialogueMessages;

    //Whether waiting to ammend new text or start new line
    private bool waiting;

    public List<GameObject> messageHistory;

    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        messageHistory = new List<GameObject>();
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void AssignValues(MessagingRecipientSO me, MessagingApplication messAppRef, MessagingDialogueSO availConvo)
    {
        nameText.text = me.recipientName;
        if (availConvo != null)
        {
            mostRecentMessageText.text = availConvo.lines[0];
        }
        else if (messageHistory.Count > 0)
        {
            //Last message
        }
        else
        {
            mostRecentMessageText.text = "";
        }
        messagingApplication = messAppRef;
        recipientOfThis = me;
        currentDialogue = availConvo;

        messagingApplication.respectiveAvailableConversation = this;

        CheckToDisplayNotificationIcon();
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void CheckToDisplayNotificationIcon()
    {
        notificationIcon.SetActive(false);

        foreach (MessagingDialogueSO conversation in messagingApplication.availableConversations)
        {
            if (conversation.personSpokenTo == recipientOfThis)
            {
                notificationIcon.SetActive(true);
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////
    private void DisplayDialogue()
    {
        //Starts a new line if current one complete
        if (remainingDialogueMessages.Count > 0)
        {
            if (!waiting)
            {
                //Instantiate receving message w/ text
                StartCoroutine(Wait(currentDialogue.delayBetweenMessages));
                remainingDialogueMessages.Remove(remainingDialogueMessages[0]);
            }
        }
        //Displays option select if reached end of dialogue and branching is present
        else if (!waiting && remainingDialogueMessages.Count <= 0 && currentDialogue.bridgeAfterMessages)
        {
            //Instantiate message options w/text
            //Those messages instantiate a sent message on completion 
        }
        else if (!waiting && remainingDialogueMessages.Count <= 0 && currentDialogue.documentAfterMessages)
        {
            //Instantiate document
        }
        //Ends dialogue if dialogue complete branching not present 
        else if (!currentDialogue.bridgeAfterMessages)
        {
            //EndDialogue();
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////
    //private void AssignNewTranscriptDialogue(Dialogue newDialogue)
    //{
    //    //Assigns variable values for new dialogue
    //    dialogue = newDialogue;
    //    foreach (string line in dialogue.lines)
    //    {
    //        remainingTranscriptDialogue.Add(line);
    //    }
    //    remainingLineDialogue = dialogue.lines[0];

    //    //Hides dialogue button if dialogue requires it 
    //    if (dialogue.dialogueHidesSubmit)
    //    {
    //        submitButton.SetActive(false);
    //    }

    //    waiting = false;
    //}

    //////////////////////////////////////////////////////////////////////////////////
    private IEnumerator Wait(float waitingDuration)
    {
        waiting = true;
        yield return new WaitForSeconds(waitingDuration);
        waiting = false;
    }

    //////////////////////////////////////////////////////////////////////////////////
    //public void BranchDialogue(int branchToGoTo)
    //{
    //    //Branches dialogue based on pressed button 
    //    optionSelectButtonParent.SetActive(false);

    //    if (branchToGoTo == 1)
    //    {
    //        AssignNewTranscriptDialogue(dialogue.bridgedDialogue1);
    //    }
    //    if (branchToGoTo == 2)
    //    {
    //        AssignNewTranscriptDialogue(dialogue.bridgedDialogue2);
    //    }
    //}

    ////////////////////////////////////////////////////////////////////////////////////
    //private void EndDialogue()
    //{
    //waiting = true;
    //    optionSelectButtonParent.SetActive(false);
    //    if (!submitButton.activeSelf)
    //    {
    //        submitButton.SetActive(true);
    //    }
    //}

    ///////////////////////////////////////////////////////////////////////////////////
    //public void ClearDialogue()
    //{
    //    EndDialogue();
    //    dialogue = null;
    //    textSpace.text = "";
    //}

    ///////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
