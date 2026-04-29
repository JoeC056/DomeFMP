using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////////
public class MessagingApplication : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject messagingAppUI;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject conversationUI;
    [SerializeField] private GameObject availableConversationsParent;
    [SerializeField] private GameObject messagesParent;
    [SerializeField] private Scrollbar menuScrollbar;
    [SerializeField] private Scrollbar conversationScrollbar;
    [SerializeField] private Image contactProfilePicture;
    [SerializeField] private GameObject gameplayUpdateMessageIcon;
    [SerializeField] private GameObject notificationIcon;
    [SerializeField] private GameObject mandatoryIcon;
    [SerializeField] private TextMeshProUGUI contactNameText;
    [SerializeField] private AudioSource computerAudioSource;
    [SerializeField] private AudioClip notificationSound;

    [Header("Prefabs")]
    [SerializeField] private GameObject availableConversationPrefab;
    [SerializeField] private GameObject receivingMessagePrefab;
    [SerializeField] private GameObject sentMessagePrefab;
    [SerializeField] private GameObject messageSelectPrefab;
    [SerializeField] private GameObject receivedDocumentMessagePrefab;
    [SerializeField] private GameObject sentDocumentMessagePrefab;
    [SerializeField] private GameObject typingIconPrefab;
    [SerializeField] private Sprite defaultProfilePicture;

    [Header("Parameters")]
    [SerializeField] private int maxRecipientsPerPage;
    [SerializeField] private float scrollbarSizeScalerValue;
    [SerializeField] private float notificationTextDuration;
    [SerializeField] private float delayBetweenMessages;

    [Header("Available Conversation Data")]
    public List<MessagingRecipientSO> availableMessagingRecipients;
    public List<MessagingDialogueSO> availableConversations;


    //Currently used dialogue
    private MessagingDialogueSO currentDialogue;
    [HideInInspector] public AvailableConversation respectiveAvailableConversation;
    private List<string> remainingDialogueMessages;


    //Size references of overarching messaging UI
    private float defaultUISpaceSize;
    private float currentUISpaceSize;


    //Temp Variables
    private List<GameObject> tempMessageHistory;
    private GameObject option1;
    private GameObject option2;
    private bool documentAlreadyDisplayed;
    public bool inConversation;

    //Whether waiting to ammend new text or start new line
    private bool waiting;

    //Most recent value of scrollbar
    private float lastMenuScrollBarValue = 0;
    private float lastConversationScrollBarValue = 0;

    private struct dialogueCompletionEvent
    {
        public MessagingDialogueSO respectiveDialogue;
        public UnityEvent eventOnCompletion;
    }

    private List<dialogueCompletionEvent> dialogueCompletionEvents;

    private UnityEvent eventOnDialogueCompletion;

    //For the flash of the mandatory message icon
    private bool goingToGreen;
    private float currentIndex;

    private GameObject currentTypingIcon;

    public List<AvailableConversation> mostRecentlyAccessedConversations;

    private float defaultVolume;


    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        remainingDialogueMessages = new List<string>();
        dialogueCompletionEvents = new List<dialogueCompletionEvent>();
        mostRecentlyAccessedConversations = new List<AvailableConversation>();

        //Sets menu as default view
        UpdateMenuDisplay();

        foreach (Transform child in availableConversationsParent.transform)
        {
            AddToRecentlyAccessedList(child.GetComponent<AvailableConversation>());
        }

        ReturnToMenu();
        ToggleVisibility();
        defaultVolume = computerAudioSource.volume;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        computerAudioSource.volume = defaultVolume * SettingsManager.instance.MasterVolume * SettingsManager.instance.GameVolume;
        CheckNotificationIconsToDisplay();

        if (currentDialogue != null)
        {
            DisplayDialogue();
        }

        UpdateConversationScrollPosition();
        AssignOrderOfConversations();
    }


    //////////////////////////////////////////////////////////////////////////////////
    private void CheckNotificationIconsToDisplay()
    {
        if (availableConversations.Count > 0)
        {
            bool mandatoryConversationPresent = false;
            bool gameplayUpdateConversationPresent = false;

            foreach (MessagingDialogueSO conversation in availableConversations)
            {
                if (conversation.mandatory)
                {
                    mandatoryConversationPresent = true;
                }
                if (conversation.gameplayUpdate && !messagingAppUI.activeSelf)
                {
                    gameplayUpdateConversationPresent = true;
                }

            }

            gameplayUpdateMessageIcon.SetActive(gameplayUpdateConversationPresent);

            if (mandatoryConversationPresent || gameplayUpdateConversationPresent)
            {
                mandatoryIcon.SetActive(true);
                notificationIcon.SetActive(false);
            }
            else
            {
                mandatoryIcon.SetActive(false);
                notificationIcon.SetActive(true);
            }
        }
        else
        {
            mandatoryIcon.SetActive(false);
            notificationIcon.SetActive(false);
            gameplayUpdateMessageIcon.SetActive(false);
        }

        if (mandatoryIcon.activeSelf)
        {
            FlashMandatoryIcon();
        }
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
    private void DisplayDialogue()
    {
        //Starts a new line if current one complete
        if (remainingDialogueMessages.Count > 0)
        {
            if (!waiting)
            {
                if (currentTypingIcon != null)
                {
                    Destroy(currentTypingIcon);
                    currentTypingIcon = null;
                }

                GameObject message = Instantiate(receivingMessagePrefab, messagesParent.transform);
                message.GetComponent<Message>().SetText(remainingDialogueMessages[0]);
                tempMessageHistory.Add(message);
                remainingDialogueMessages.Remove(remainingDialogueMessages[0]);

                if (remainingDialogueMessages.Count > 0 || currentDialogue.documentAfterMessages)
                {
                    StartCoroutine(Wait(delayBetweenMessages));
                    currentTypingIcon = Instantiate(typingIconPrefab, messagesParent.transform);
                }
                else if (currentDialogue.bridgeAfterMessages)
                {
                    StartCoroutine(Wait(delayBetweenMessages / 5));
                }
                else
                {
                    StartCoroutine(Wait(delayBetweenMessages));
                }
            }
        }
        //Displays option select if reached end of dialogue and branching is present
        else if (!waiting && remainingDialogueMessages.Count <= 0 && currentDialogue.bridgeAfterMessages)
        {
            if (currentTypingIcon != null)
            {
                Destroy(currentTypingIcon);
                currentTypingIcon = null;
            }

            if (option1 == null && option2 == null)
            {
                inConversation = true;
                if (currentDialogue.bridgeResponse1 != "")
                {
                    option1 = Instantiate(messageSelectPrefab, messagesParent.transform);
                    option1.GetComponent<Message>().SetText(currentDialogue.bridgeResponse1);
                    option1.GetComponentInChildren<Button>().onClick.AddListener(() => SelectResponse(1));
                }

                if (currentDialogue.bridgeResponse2 != "")
                {
                    option2 = Instantiate(messageSelectPrefab, messagesParent.transform);
                    option2.GetComponent<Message>().SetText(currentDialogue.bridgeResponse2);
                    option2.GetComponentInChildren<Button>().onClick.AddListener(() => SelectResponse(2));
                }
            }

        }
        //Displays the document to be sent after completing all messages if applicable
        else if (!waiting && remainingDialogueMessages.Count <= 0 && currentDialogue.documentAfterMessages && !documentAlreadyDisplayed)
        {
            if (currentTypingIcon != null)
            {
                Destroy(currentTypingIcon);
                currentTypingIcon = null;
            }

            //Instantiate document
            if (currentDialogue.documentMessageType == MessagingDialogueSO.MessageType.Received)
            {
                GameObject message = Instantiate(receivedDocumentMessagePrefab, messagesParent.transform);
                message.GetComponentInChildren<DocumentMessage>().respectiveDocument = currentDialogue.document;
                message.GetComponent<Message>().SetText(currentDialogue.document.name);
                tempMessageHistory.Add(message);
            }
            else if (currentDialogue.documentMessageType == MessagingDialogueSO.MessageType.Sent)
            {
                GameObject message = Instantiate(sentDocumentMessagePrefab, messagesParent.transform);
                message.GetComponentInChildren<DocumentMessage>().respectiveDocument = currentDialogue.document;
                message.GetComponent<Message>().SetText(currentDialogue.document.name);
                tempMessageHistory.Add(message);

            }
            documentAlreadyDisplayed = true;
        }
        //Continues after the documents completion if required
        else if (currentDialogue.documentAfterMessages && documentAlreadyDisplayed && currentDialogue.continueAfterDocument)
        {
            if (currentTypingIcon != null)
            {
                Destroy(currentTypingIcon);
                currentTypingIcon = null;
            }

            BridgeDialogue(currentDialogue.continuedDialogue);
            StartCoroutine(Wait(delayBetweenMessages));
            currentTypingIcon = Instantiate(typingIconPrefab, messagesParent.transform);

        }
        //Ends dialogue if dialogue complete branching not present 
        else if (!currentDialogue.bridgeAfterMessages && ((currentDialogue.documentAfterMessages && documentAlreadyDisplayed) || (!currentDialogue.documentAfterMessages)))
        {
            if (currentTypingIcon != null)
            {
                Destroy(currentTypingIcon);
                currentTypingIcon = null;
            }

            EndDialogue();
        }
        AssignRangeOfScrollbarForMessagingUI();
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void AssignNewTranscriptDialogue(MessagingDialogueSO newDialogue)
    {
        //Assigns variable values for new dialogue
        if (newDialogue != currentDialogue)
        {
            currentDialogue = newDialogue;

            foreach (string line in currentDialogue.lines)
            {
                remainingDialogueMessages.Add(line);
            }
            documentAlreadyDisplayed = false;

            waiting = false;

            if (eventOnDialogueCompletion == null)
            {
                eventOnDialogueCompletion = GetRespectiveEventForDialogue();
            }
            //else
            //{
            //    eventOnDialogueCompletion = null;
            //}

            foreach (Transform child in availableConversationsParent.transform)
            {
                if (child.gameObject.GetComponent<AvailableConversation>().respectiveRecipient == currentDialogue.personSpokenTo)
                {
                    AppendAllPreviousDialogue(child.gameObject.GetComponent<AvailableConversation>().messageHistoryMessageTypes, child.gameObject.GetComponent<AvailableConversation>().messageHistoryMessageContents, child.gameObject.GetComponent<AvailableConversation>().messageDocumentsHistory);
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void EndDialogue()
    {

        //if ((tempMessageHistory.Count == currentDialogue.lines.Count) || (currentDialogue.bridgeAfterMessages && (tempMessageHistory.Count == currentDialogue.lines.Count + 1)) || (currentDialogue.documentAfterMessages && (tempMessageHistory.Count == currentDialogue.lines.Count + 1))) 

        //Removes the dialogue from the list of avaiable dialogue
        List<MessagingDialogueSO> toDelete = new List<MessagingDialogueSO>();
        foreach (MessagingDialogueSO availConvo in availableConversations)
        {
            if (availConvo.personSpokenTo == currentDialogue.personSpokenTo)
            {
                toDelete.Add(availConvo);
            }
        }
        foreach (MessagingDialogueSO deleteableThing in toDelete)
        {
            availableConversations.Remove(deleteableThing);

        }

        foreach (GameObject message in tempMessageHistory)
        {
            Debug.Log(respectiveAvailableConversation.GetComponent<AvailableConversation>().respectiveRecipient);
            respectiveAvailableConversation.messageHistoryMessageTypes.Add(message.name);
            respectiveAvailableConversation.messageHistoryMessageContents.Add(message.GetComponentInChildren<TextMeshProUGUI>().text);
            if (message.GetComponentInChildren<DocumentMessage>() != null)
            {
                respectiveAvailableConversation.messageDocumentsHistory.Add(message.GetComponentInChildren<DocumentMessage>().respectiveDocument);
            }
        }

        if (currentDialogue.gameplayUpdate)
        {
            GameManager.instance.StartSecondHalf();
        }

        currentDialogue = null;
        StopAllCoroutines();
        waiting = true;
        inConversation = false;

        if (eventOnDialogueCompletion != null)
        {
            eventOnDialogueCompletion.Invoke();
            eventOnDialogueCompletion = null;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void AppendAllPreviousDialogue(List<string> messageTypes, List<string> messageContents, List<GameObject> documents)
    {
        foreach (Transform child in messagesParent.transform)
        {
            Destroy(child.gameObject);
        }
        int index = 0;

        for (int i = 0; i < messageTypes.Count; i++)
        {
            GameObject previousMessage = null;

            if (messageTypes[i].Contains(receivingMessagePrefab.name))
            {
                previousMessage = Instantiate(receivingMessagePrefab, messagesParent.transform);
            }
            else if (messageTypes[i].Contains(sentMessagePrefab.name))
            {
                previousMessage = Instantiate(sentMessagePrefab, messagesParent.transform);
            }
            else if (messageTypes[i].Contains(receivedDocumentMessagePrefab.name))
            {
                previousMessage = Instantiate(receivedDocumentMessagePrefab, messagesParent.transform);
                previousMessage.GetComponentInChildren<DocumentMessage>().respectiveDocument = documents[index];
                index++;
            }
            else if (messageTypes[i].Contains(sentDocumentMessagePrefab.name))
            {
                previousMessage = Instantiate(sentDocumentMessagePrefab, messagesParent.transform);
                previousMessage.GetComponentInChildren<DocumentMessage>().respectiveDocument = documents[index];
                index++;
            }
            previousMessage.GetComponent<Message>().SetText(messageContents[i]);
        }

    }

    //////////////////////////////////////////////////////////////////////////////////
    private void BridgeDialogue(MessagingDialogueSO dialogueToBridgeTo)
    {
        inConversation = true;

        //Removes the previous dialogue from the list of avaiable dialogue
        List<MessagingDialogueSO> toDelete = new List<MessagingDialogueSO>();
        foreach (MessagingDialogueSO availConvo in availableConversations)
        {
            if (availConvo.personSpokenTo == currentDialogue.personSpokenTo)
            {
                toDelete.Add(availConvo);
            }
        }
        foreach (MessagingDialogueSO deleteableThing in toDelete)
        {
            availableConversations.Remove(deleteableThing);

        }


        currentDialogue = dialogueToBridgeTo;

        foreach (string line in currentDialogue.lines)
        {
            remainingDialogueMessages.Add(line);
        }
        documentAlreadyDisplayed = false;

        waiting = false;

        if (eventOnDialogueCompletion == null)
        {
            eventOnDialogueCompletion = GetRespectiveEventForDialogue();
        }
        //else
        //{
        //    eventOnDialogueCompletion = null;
        //}


    }

    //////////////////////////////////////////////////////////////////////////////////
    public void SelectRecipient(MessagingRecipientSO messagingRecipient)
    {
        foreach (Transform child in availableConversationsParent.transform)
        {
            if (child.GetComponent<AvailableConversation>().respectiveRecipient == messagingRecipient)
            {
                respectiveAvailableConversation = child.GetComponent<AvailableConversation>();
                AddToRecentlyAccessedList(respectiveAvailableConversation);
            }
        }
        menuUI.SetActive(false);
        conversationUI.SetActive(true);
        AssignNewTranscriptDialogue(GetAvailableConversationForRecipient(messagingRecipient));
        AppendAllPreviousDialogue(respectiveAvailableConversation.gameObject.GetComponent<AvailableConversation>().messageHistoryMessageTypes, respectiveAvailableConversation.gameObject.GetComponent<AvailableConversation>().messageHistoryMessageContents, respectiveAvailableConversation.gameObject.GetComponent<AvailableConversation>().messageDocumentsHistory);
        tempMessageHistory = new List<GameObject>();


        ResetConversationScrollbar();
        conversationScrollbar.value = 1;

        if (messagingRecipient.profilePictureImage != null)
        {
            contactProfilePicture.sprite = messagingRecipient.profilePictureImage;
        }
        else
        {
            contactProfilePicture.sprite = defaultProfilePicture;
        }

        contactNameText.text = messagingRecipient.recipientName;
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void SelectResponse(int branchToGoTo)
    {
        //Delete both the branch options, instantiate the correct as a response and continue 
        Destroy(option1);
        option1 = null;
        Destroy(option2);
        option2 = null;


        if (branchToGoTo == 1)
        {
            GameObject message = Instantiate(sentMessagePrefab, messagesParent.transform);
            message.GetComponent<Message>().SetText(currentDialogue.bridgeResponse1);
            tempMessageHistory.Add(message);
            BridgeDialogue(currentDialogue.bridgedDialogue1);
        }
        if (branchToGoTo == 2)
        {
            GameObject message = Instantiate(sentMessagePrefab, messagesParent.transform);
            message.GetComponent<Message>().SetText(currentDialogue.bridgeResponse2);
            tempMessageHistory.Add(message);
            BridgeDialogue(currentDialogue.bridgedDialogue2);
        }

        StartCoroutine(Wait(delayBetweenMessages));
        currentTypingIcon = Instantiate(typingIconPrefab, messagesParent.transform);
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void UpdateMenuDisplay()
    {
        //Assisng size of the menu area to fit all available conversations
        UpdateSizeReferences();
        if (currentUISpaceSize > defaultUISpaceSize)
        {
            menuUI.GetComponent<RectTransform>().sizeDelta = new Vector2(menuUI.GetComponent<RectTransform>().sizeDelta.x, currentUISpaceSize);
        }
        else
        {
            menuUI.GetComponent<RectTransform>().sizeDelta = new Vector2(menuUI.GetComponent<RectTransform>().sizeDelta.x, defaultUISpaceSize);
        }

        List<MessagingRecipientSO> recipientsToAdd = new List<MessagingRecipientSO>();
        foreach (MessagingRecipientSO recipient in availableMessagingRecipients)
        {
            recipientsToAdd.Add(recipient);
        }

        foreach (Transform child in availableConversationsParent.transform)
        {
            if (availableMessagingRecipients.Contains(child.gameObject.GetComponent<AvailableConversation>().respectiveRecipient))
            {
                recipientsToAdd.Remove(child.gameObject.GetComponent<AvailableConversation>().respectiveRecipient);
            }
        }
        foreach (MessagingRecipientSO recipientToAdd in recipientsToAdd)
        {
            GameObject availConvo = Instantiate(availableConversationPrefab, availableConversationsParent.transform);
            if (recipientToAdd.profilePictureImage != null)
            {
                availConvo.GetComponent<AvailableConversation>().SetProfilePicture(recipientToAdd.profilePictureImage);
            }
            else
            {
                availConvo.GetComponent<AvailableConversation>().SetProfilePicture(defaultProfilePicture);
            }
            availConvo.GetComponent<AvailableConversation>().AssignValues(recipientToAdd, this, GetAvailableConversationForRecipient(recipientToAdd));
            availConvo.GetComponent<Button>().onClick.AddListener(() => SelectRecipient(recipientToAdd));
        }


        //Reorders the recipients in the menu 

        AssignRangeOfScrollbarForMenu();
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void ReturnToMenu()
    {
        if (!inConversation)
        {
            currentDialogue = null;
            menuUI.SetActive(true);
            conversationUI.SetActive(false);
            remainingDialogueMessages.Clear();
            ResetMenuScrollbar();
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void ToggleVisibility()
    {
        if (!inConversation)
        {
            messagingAppUI.SetActive(!messagingAppUI.activeSelf);
            if (!messagingAppUI.activeSelf)
            {
                ReturnToMenu();
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void AssignRangeOfScrollbarForMenu()
    {
        //ResetMenuScrollbar();

        float prefabSize = availableConversationPrefab.GetComponent<RectTransform>().sizeDelta.y;
        float defaultSize = maxRecipientsPerPage * prefabSize;
        float currentSize = prefabSize * availableMessagingRecipients.Count;


        //Sets size of scrollbar according to size of page
        if (currentSize > defaultSize)
        {
            menuScrollbar.size = (defaultSize / currentSize) * scrollbarSizeScalerValue;
            menuScrollbar.enabled = true;
        }
        else
        {
            menuScrollbar.size = 1;
            menuScrollbar.enabled = false;
        }

    }

    //////////////////////////////////////////////////////////////////////////////////
    private void AssignRangeOfScrollbarForMessagingUI()
    {
        //ResetConversationScrollbar();

        if (messagesParent.transform.childCount > 0)
        {
            float defaultSize = messagesParent.GetComponent<RectTransform>().sizeDelta.y;
            float currentSize = GetHeightOfMessagesParent();

            //Sets size of scrollbar according to size of page
            if (currentSize > defaultSize)
            {
                conversationScrollbar.size = (defaultSize / currentSize) * scrollbarSizeScalerValue;
                conversationScrollbar.enabled = true;
            }
            else
            {
                conversationScrollbar.size = 1;
                conversationScrollbar.enabled = false;
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void Scroll()
    {
        if (menuUI.activeSelf)
        {
            ScrollMenu();
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void ScrollMenu()
    {
        //Determines amount scrolled
        float amountScrolled = menuScrollbar.value - lastMenuScrollBarValue;
        lastMenuScrollBarValue = menuScrollbar.value;

        float prefabSize = availableConversationPrefab.GetComponent<RectTransform>().sizeDelta.y;
        float defaultSize = maxRecipientsPerPage * prefabSize;
        float currentSize = prefabSize * availableMessagingRecipients.Count;

        //Moves website contents based on amount scrolled 
        availableConversationsParent.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, (currentSize - defaultSize) * amountScrolled);
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void UpdateConversationScrollPosition()
    {
        float defaultSize = messagesParent.GetComponent<RectTransform>().sizeDelta.y;
        float currentSize = GetHeightOfMessagesParent();

        //Moves website contents based on amount scrolled 
        messagesParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, ((currentSize - defaultSize)) * conversationScrollbar.value);

    }


    //////////////////////////////////////////////////////////////////////////////////
    private void ResetMenuScrollbar()
    {
        lastMenuScrollBarValue = 0;
        menuScrollbar.value = 0;
    }
    //////////////////////////////////////////////////////////////////////////////////
    private void ResetConversationScrollbar()
    {
        lastConversationScrollBarValue = 0;
        conversationScrollbar.value = 0;
    }

    //////////////////////////////////////////////////////////////////////////////////
    public MessagingDialogueSO GetAvailableConversationForRecipient(MessagingRecipientSO recipient)
    {
        foreach (MessagingDialogueSO conversation in availableConversations)
        {
            if (conversation.personSpokenTo == recipient)
            {
                return conversation;
            }
        }
        return null;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private float GetHeightOfMessagesParent()
    {
        if (messagesParent.transform.childCount > 3)
        {
            GameObject highestElement = null;
            GameObject lowestElement = null;

            foreach (Transform child in messagesParent.transform)
            {
                if (highestElement == null)
                {
                    highestElement = child.gameObject;
                    lowestElement = child.gameObject;
                }
                else
                {
                    if (child.GetComponent<RectTransform>().anchoredPosition.y > highestElement.GetComponent<RectTransform>().anchoredPosition.y)
                    {
                        highestElement = child.gameObject;
                    }
                    if (child.GetComponent<RectTransform>().anchoredPosition.y < lowestElement.GetComponent<RectTransform>().anchoredPosition.y)
                    {
                        lowestElement = child.gameObject;
                    }
                }
            }

            float highestPoint = highestElement.GetComponent<RectTransform>().anchoredPosition.y + (highestElement.GetComponent<RectTransform>().sizeDelta.y / 2);
            float lowestPoint = lowestElement.GetComponent<RectTransform>().anchoredPosition.y - (lowestElement.GetComponent<RectTransform>().sizeDelta.y / 2);
            return highestPoint - lowestPoint;
        }
        else
        {
            return 1;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void UpdateSizeReferences()
    {
        //Assigns values of size variables
        float prefabSize = availableConversationPrefab.GetComponent<RectTransform>().sizeDelta.y;
        defaultUISpaceSize = maxRecipientsPerPage * prefabSize;
        currentUISpaceSize = prefabSize * availableMessagingRecipients.Count;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private IEnumerator Wait(float waitingDuration)
    {
        waiting = true;
        yield return new WaitForSeconds(waitingDuration);
        waiting = false;
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void AddNewMessagingRecipient(MessagingRecipientSO newRecipient)
    {
        availableMessagingRecipients.Add(newRecipient);
        UpdateMenuDisplay();

        foreach (Transform child in availableConversationsParent.transform)
        {
            if (child.GetComponent<AvailableConversation>().respectiveRecipient == newRecipient)
            {
                AddToRecentlyAccessedList(child.GetComponent<AvailableConversation>());
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void AddNewAvailableConversation(MessagingDialogueSO newConversation)
    {
        if (!availableMessagingRecipients.Contains(newConversation.personSpokenTo))
        {
            AddNewMessagingRecipient(newConversation.personSpokenTo);
        }
        availableConversations.Add(newConversation);

        PlayNotificationSound();
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void AddEventOnDialogueCompletion(MessagingDialogueSO dialogue, UnityEvent newEvent)
    {
        dialogueCompletionEvent diaCompEvent = new dialogueCompletionEvent();
        diaCompEvent.respectiveDialogue = dialogue;
        diaCompEvent.eventOnCompletion = newEvent;

        dialogueCompletionEvents.Add(diaCompEvent);
    }

    //////////////////////////////////////////////////////////////////////////////////
    private UnityEvent GetRespectiveEventForDialogue()
    {
        foreach (dialogueCompletionEvent compEvent in dialogueCompletionEvents)
        {
            if (compEvent.respectiveDialogue == currentDialogue)
            {
                return compEvent.eventOnCompletion;
            }
        }
        return null;
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void ClearAvailableConversations()
    {
        availableConversations.Clear();
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void PlayNotificationSound()
    {
        computerAudioSource.clip = notificationSound;
        computerAudioSource.Play();
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void AssignOrderOfConversations()
    {
        List <AvailableConversation> fullList = new List<AvailableConversation>();
        List<AvailableConversation> mandatoryConvos = new List<AvailableConversation>();
        List<AvailableConversation> convos = new List<AvailableConversation>();
        List<AvailableConversation> noConvos = new List<AvailableConversation>();

        foreach (AvailableConversation convo in mostRecentlyAccessedConversations)
        {
            if (convo.mandatoryConversationPresent)
            {
                mandatoryConvos.Add(convo);
            }
            else if (convo.convoPresent)
            {
                convos.Add(convo);
            }
            else
            {
                noConvos.Add(convo);
            }
        }
        fullList.AddRange(mandatoryConvos);
        fullList.AddRange(convos);
        fullList.AddRange(noConvos);

        foreach (Transform child in availableConversationsParent.transform)
        {
            child.SetSiblingIndex(fullList.IndexOf(child.GetComponent<AvailableConversation>()));
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void AddToRecentlyAccessedList(AvailableConversation conversationToAdd)
    {
        if (mostRecentlyAccessedConversations.Contains(conversationToAdd))
        {
            mostRecentlyAccessedConversations.Remove(conversationToAdd);
        }

        mostRecentlyAccessedConversations.Insert(0,conversationToAdd);
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////


