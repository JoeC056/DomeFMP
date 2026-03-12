using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class AvailableConversation : MonoBehaviour
{
    [Header("References")]
    private MessagingApplication messagingApplication;
    public MessagingRecipientSO recipientOfThis;
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
}

//////////////////////////////////////////////////////////////////////////////////
