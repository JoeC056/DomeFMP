using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////////
public class AvailableConversation : MonoBehaviour
{
    [Header("References")]
    private MessagingApplication messagingApplication;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI mostRecentMessageText;
    [SerializeField] private GameObject notificationIcon;
    [SerializeField] private Image profilePicture;
    [SerializeField] private GameObject mandatoryIcon;


    //Respective Person Conversed With
    [HideInInspector] public MessagingRecipientSO respectiveRecipient;

    public List<string> messageHistoryMessageTypes;
    public List<string> messageHistoryMessageContents;
    public List<GameObject> messageDocumentsHistory;

    private bool goingToGreen;
    private float currentIndex;

    public bool convoPresent;
    public bool mandatoryConversationPresent;

    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        messageHistoryMessageTypes = new List<string>();
        messageHistoryMessageContents = new List<string>();
        messageDocumentsHistory = new List<GameObject>();
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        UpdateValues();
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void AssignValues(MessagingRecipientSO me, MessagingApplication messAppRef, MessagingDialogueSO availConvo)
    {
        respectiveRecipient = me;
        messagingApplication = messAppRef;
        nameText.text = me.recipientName;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void UpdateValues()
    {
        if (messagingApplication.GetAvailableConversationForRecipient(respectiveRecipient) != null)
        {
            string messageToShow = messagingApplication.GetAvailableConversationForRecipient(respectiveRecipient).lines[0];

            if (messageToShow.Length > 10)
            {
                mostRecentMessageText.text = messageToShow.Substring(0,10) + "...";
            }
            else
            {
                mostRecentMessageText.text = messageToShow + "...";
            }
        }
        else if (messageHistoryMessageContents.Count > 0)
        {
            if (messageHistoryMessageContents[messageHistoryMessageContents.Count - 1].Length > 10)
            {
                mostRecentMessageText.text = messageHistoryMessageContents[messageHistoryMessageContents.Count - 1].Substring(0, 10) + "...";
            }
            else
            {
                mostRecentMessageText.text = messageHistoryMessageContents[messageHistoryMessageContents.Count - 1] + "...";
            }
        }
        else
        {
            mostRecentMessageText.text = "";
        }

        CheckToDisplayNotificationIcon();
    }

    //////////////////////////////////////////////////////////////////////////////////s
    private void CheckToDisplayNotificationIcon()
    {
        mandatoryConversationPresent = false;
        convoPresent = false;

        if (messagingApplication.availableConversations.Count > 0)
        {

            foreach (MessagingDialogueSO conversation in messagingApplication.availableConversations)
            {
                if (conversation.personSpokenTo == respectiveRecipient)
                {
                    convoPresent = true;

                    if (conversation.mandatory)
                    {
                        mandatoryConversationPresent = true;
                    }
                }

            }

            if (mandatoryConversationPresent)
            {
                mandatoryIcon.SetActive(true);
                notificationIcon.SetActive(false);
            }
            else if (convoPresent)
            {
                mandatoryIcon.SetActive(false);
                notificationIcon.SetActive(true);
            }
            else
            {
                mandatoryIcon.SetActive(false);
                notificationIcon.SetActive(false);
            }
        }
        else
        {
            mandatoryIcon.SetActive(false);
            notificationIcon.SetActive(false);
        }

        if (mandatoryIcon.activeSelf)
        {
            FlashMandatoryIcon();
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void SetProfilePicture(Sprite pictureToSetTo)
    {
        profilePicture.sprite = pictureToSetTo;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void FlashMandatoryIcon()
    {
        if (goingToGreen)
        {
            currentIndex -= Time.deltaTime * 100;
            if (currentIndex <= 0)
            {
                currentIndex = 0;
                goingToGreen = false;
            }
        }
        else
        {
            currentIndex += Time.deltaTime * 100;
            if (currentIndex >= 123)
            {
                currentIndex = 123;
                goingToGreen = true;
            }
        }

        mandatoryIcon.GetComponent<Image>().color = new Color((0 + currentIndex) / 255, (255 - currentIndex) / 255, (0 + currentIndex) / 255, 1);

    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
