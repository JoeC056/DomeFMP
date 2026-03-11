using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] private Scrollbar scrollbar;

    [Header("Prefabs")]
    [SerializeField] private GameObject availableConversationPrefab;
    [SerializeField] private GameObject receivingMessagePrefab;
    [SerializeField] private GameObject sentMessagePrefab;
    [SerializeField] private GameObject messageSelectPrefab;
    [SerializeField] private GameObject receivedDocumentMessagePrefab;
    [SerializeField] private GameObject sentDocumentMessagePrefab;

    [Header("Parameters")]
    [SerializeField] private int maxRecipientsPerPage;
    [SerializeField] private float scrollbarSizeScalerValue;

    [Header("Available Conversation Data")]
    public List<MessagingRecipientSO> availableMessagingRecipients;
    public List<MessagingDialogueSO> availableConversations;

    //Most recent value of scrollbar
    private float lastScrollBarValue = 0;

    //Size references of important elements
    private float convoPrefabSize;
    private float defaultUISpaceSize;
    private float currentUISpaceSize;

    //Currently used dialogue
    private MessagingDialogueSO currentDialogue;
    public AvailableConversation respectiveAvailableConversation;

    private List<GameObject> tempMessageHistory;


    //Content of the dialogue
    private List<string> remainingDialogueMessages;

    //Whether waiting to ammend new text or start new line
    private bool waiting;

    private GameObject option1;
    private GameObject option2;

    private bool documentAlreadyDisplayed;

    private bool inConversation;

    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        remainingDialogueMessages = new List<string>();

        //Sets menu as default view
        UpdateMenuDisplay();
        ReturnToMenu();

        ToggleVisibility();

    }

    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (currentDialogue != null)
        {
            DisplayDialogue();
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void DisplayDialogue()
    {
        scrollbar.gameObject.SetActive(false);

        //Starts a new line if current one complete
        if (remainingDialogueMessages.Count > 0)
        {
            if (!waiting)
            {
                if (scrollbar.gameObject.activeSelf)
                {
                    scrollbar.value = 1;
                }

                GameObject message = Instantiate(receivingMessagePrefab, messagesParent.transform);
                message.GetComponent<Message>().SetText(remainingDialogueMessages[0]);
                tempMessageHistory.Add(message);
                StartCoroutine(Wait(currentDialogue.delayBetweenMessages));
                remainingDialogueMessages.Remove(remainingDialogueMessages[0]);
            }
        }
        //Displays option select if reached end of dialogue and branching is present
        else if (!waiting && remainingDialogueMessages.Count <= 0 && currentDialogue.bridgeAfterMessages)
        {
            if (option1 == null && option2 == null)
            {
                if (scrollbar.gameObject.activeSelf)
                {
                    scrollbar.value = 1;
                }

                inConversation = true;
                option1 = Instantiate(messageSelectPrefab, messagesParent.transform);
                option2 = Instantiate(messageSelectPrefab, messagesParent.transform);
                option1.GetComponent<Message>().SetText(currentDialogue.bridgeResponse1);
                option2.GetComponent<Message>().SetText(currentDialogue.bridgeResponse2);
                option1.GetComponent<Button>().onClick.AddListener(() => SelectResponse(1));
                option2.GetComponent<Button>().onClick.AddListener(() => SelectResponse(2));
            }
        }
        else if (!waiting && remainingDialogueMessages.Count <= 0 && currentDialogue.documentAfterMessages && !documentAlreadyDisplayed)
        {
            //Instantiate document
            if (currentDialogue.documentMessageType == MessagingDialogueSO.MessageType.Received)
            {
                if (scrollbar.gameObject.activeSelf)
                {
                    scrollbar.value = 1;
                }

                GameObject message = Instantiate(receivedDocumentMessagePrefab, messagesParent.transform);
                message.GetComponentInChildren<DocumentMessage>().respectiveDocument = currentDialogue.document;
                message.GetComponent<Message>().SetText(currentDialogue.document.name);
            }
            if (currentDialogue.documentMessageType == MessagingDialogueSO.MessageType.Sent)
            {
                if (scrollbar.gameObject.activeSelf)
                {
                    scrollbar.value = 1;
                }

                GameObject message = Instantiate(sentDocumentMessagePrefab, messagesParent.transform);
                message.GetComponentInChildren<DocumentMessage>().respectiveDocument = currentDialogue.document;
                message.GetComponent<Message>().SetText(currentDialogue.document.name);
            }
            documentAlreadyDisplayed = true;
        }
        //else if continue after document
        else if (currentDialogue.documentAfterMessages && documentAlreadyDisplayed && currentDialogue.continueAfterDocument)
        {
            AssignNewTranscriptDialogue(currentDialogue.bridgedDialogue1);
        }
        //Ends dialogue if dialogue complete branching not present 
        else if (!currentDialogue.bridgeAfterMessages && ((currentDialogue.documentAfterMessages && documentAlreadyDisplayed) || (!currentDialogue.documentAfterMessages)))
        {
            EndDialogue();
        }

        AssignRangeOfScrollbarForMessagingUI();
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void SelectRecipient(MessagingRecipientSO messagingRecipient)
    {
        menuUI.SetActive(false);
        conversationUI.SetActive(true);
        AssignNewTranscriptDialogue(GetAvailableConversationForRecipient(messagingRecipient));
        AppendAllPreviousDialogue(respectiveAvailableConversation.messageHistory);
        tempMessageHistory = new List<GameObject>();
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void AppendAllPreviousDialogue(List<GameObject> history)
    {
        //foreach (Transform child in messagesParent.transform)
        //{
        //    Destroy(child.gameObject);
        //}
        //foreach (GameObject message in history)
        //{
        //    GameObject previousMessage = Instantiate(message, messagesParent.transform);
        //    previousMessage.GetComponentInChildren<Image>().enabled = true;
        //    previousMessage.GetComponentInChildren<Message>().enabled = true;
        //}

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



        foreach (Transform child in availableConversationsParent.transform)
        {
            if (!availableMessagingRecipients.Contains(child.gameObject.GetComponent<AvailableConversation>().recipientOfThis))
            {
                Destroy(child);
            }
        }


        List<MessagingRecipientSO> recipientsToAdd;
        recipientsToAdd = availableMessagingRecipients;
        foreach (Transform child in availableConversationsParent.transform)
        {
            if (recipientsToAdd.Contains(child.gameObject.GetComponent<AvailableConversation>().recipientOfThis))
            {
                recipientsToAdd.Remove(child.gameObject.GetComponent<AvailableConversation>().recipientOfThis);
            }
        }



        foreach (MessagingRecipientSO recipientToAdd in recipientsToAdd)
        {
            GameObject availConvo = Instantiate(availableConversationPrefab, availableConversationsParent.transform);
            availConvo.GetComponent<AvailableConversation>().AssignValues(recipientToAdd, this, GetAvailableConversationForRecipient(recipientToAdd));
            availConvo.GetComponent<Button>().onClick.AddListener(() => SelectRecipient(recipientToAdd));
        }

        AssignRangeOfScrollbarForMenu();
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void ReturnToMenu()
    {
        if (!inConversation)
        {
            menuUI.SetActive(true);
            conversationUI.SetActive(false);
            remainingDialogueMessages.Clear();
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
    private void UpdateSizeReferences()
    {
        //Assigns values of size variables
        float prefabSize = availableConversationPrefab.GetComponent<RectTransform>().sizeDelta.y;
        float defaultSize = maxRecipientsPerPage * prefabSize;
        float currentSize = prefabSize * availableMessagingRecipients.Count;
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
    private void AssignRangeOfScrollbarForMenu()
    {
        float prefabSize = availableConversationPrefab.GetComponent<RectTransform>().sizeDelta.y;

        ResetScrollbar();
        float defaultSize = maxRecipientsPerPage * prefabSize;
        float currentSize = prefabSize * availableMessagingRecipients.Count;

        //Displays scrollbar only if page large enough to warrant scrolling being required
        if (currentSize > defaultSize)
        {
            scrollbar.gameObject.SetActive(true);
        }
        else
        {
            scrollbar.gameObject.SetActive(false);
        }

        //Scales size of scrollbar to size of page 
        scrollbar.size = (defaultSize / currentSize) * scrollbarSizeScalerValue;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void AssignRangeOfScrollbarForMessagingUI()
    {
        if (messagesParent.transform.childCount > 0)
        {
            float defaultSize = messagesParent.GetComponent<RectTransform>().sizeDelta.y;
            float currentSize = GetHeightOfMessagesParent();

            //Displays scrollbar only if page large enough to warrant scrolling being required
            if (currentSize > defaultSize)
            {
                scrollbar.gameObject.SetActive(true);
            }
            else
            {
                scrollbar.gameObject.SetActive(false);
            }

            //Scales size of scrollbar to size of page 
            scrollbar.size = (defaultSize / currentSize) * scrollbarSizeScalerValue;
        }
    }


    //////////////////////////////////////////////////////////////////////////////////
    public void Scroll()
    {
        if (menuUI.activeSelf)
        {
            ScrollMenu();
        }
        else
        {
            ScrollConversation();
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void ScrollMenu()
    {
        //Determines amount scrolled
        float amountScrolled = scrollbar.value - lastScrollBarValue;
        lastScrollBarValue = scrollbar.value;

        float prefabSize = availableConversationPrefab.GetComponent<RectTransform>().sizeDelta.y;
        float defaultSize = maxRecipientsPerPage * prefabSize;
        float currentSize = prefabSize * availableMessagingRecipients.Count;

        //Moves website contents based on amount scrolled 
        availableConversationsParent.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, (currentSize - defaultSize) * amountScrolled);
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void ScrollConversation()
    {
        GetHeightOfMessagesParent();
        //Determines amount scrolled
        float amountScrolled = scrollbar.value - lastScrollBarValue;
        lastScrollBarValue = scrollbar.value;

        float defaultSize = messagesParent.GetComponent<RectTransform>().sizeDelta.y;
        float currentSize = GetHeightOfMessagesParent();

        //Moves website contents based on amount scrolled 
        messagesParent.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, ((currentSize - defaultSize)) * amountScrolled);
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void ResetScrollbar()
    {
        lastScrollBarValue = 0;
        scrollbar.value = 0;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private IEnumerator Wait(float waitingDuration)
    {
        waiting = true;
        yield return new WaitForSeconds(waitingDuration);
        waiting = false;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void EndDialogue()
    {
        if ((tempMessageHistory.Count == currentDialogue.lines.Count) || (currentDialogue.bridgeAfterMessages && (tempMessageHistory.Count == currentDialogue.lines.Count + 1)) || (currentDialogue.documentAfterMessages && (tempMessageHistory.Count == currentDialogue.lines.Count + 1)));
        {
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
                respectiveAvailableConversation.messageHistory.Add(message);
            }


        }
        currentDialogue = null;
        StopAllCoroutines();
        waiting = true;
        inConversation = false;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void AssignNewTranscriptDialogue(MessagingDialogueSO newDialogue)
    {
        //Assigns variable values for new dialogue
        if (newDialogue != null)
        {
            currentDialogue = newDialogue;
            foreach (string line in currentDialogue.lines)
            {
                remainingDialogueMessages.Add(line);
            }
            documentAlreadyDisplayed = false;

            waiting = false;

            foreach (Transform child in availableConversationsParent.transform)
            {
                if (child.gameObject.GetComponent<AvailableConversation>().recipientOfThis == currentDialogue.personSpokenTo)
                {
                    AppendAllPreviousDialogue(child.gameObject.GetComponent<AvailableConversation>().messageHistory);
                }
            }
        }
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
            AssignNewTranscriptDialogue(currentDialogue.bridgedDialogue1);
        }
        if (branchToGoTo == 2)
        {
            GameObject message = Instantiate(sentMessagePrefab, messagesParent.transform);
            message.GetComponent<Message>().SetText(currentDialogue.bridgeResponse2);
            tempMessageHistory.Add(message);
            AssignNewTranscriptDialogue(currentDialogue.bridgedDialogue2);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private float GetHeightOfMessagesParent()
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
                if (child.transform.position.y > highestElement.transform.position.y)
                {
                    highestElement = child.gameObject;
                }
                if (child.transform.position.y < lowestElement.transform.position.y)
                {
                    lowestElement = child.gameObject;
                }
            }
        }

        float highestPoint = highestElement.transform.position.y + (highestElement.GetComponent<RectTransform>().sizeDelta.y / 2);
        float lowestPoint = lowestElement.transform.position.y - (lowestElement.GetComponent<RectTransform>().sizeDelta.y / 2);
        return highestPoint - lowestPoint;

    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////


