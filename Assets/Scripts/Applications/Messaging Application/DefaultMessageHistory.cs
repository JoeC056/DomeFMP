using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class DefaultMessageHistory : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject conversationsParent;
    [SerializeField] private MessagingRecipientSO daughter;
    [SerializeField] private MessagingRecipientSO government;

    [Header("Parameters")]
    [SerializeField] private List<GameObject> daughterMessageTypes;
    [SerializeField] private List<string> daughterMessagesContents;
    [SerializeField] private List<GameObject> daughterMessagesDocuments;
    [SerializeField] private List<GameObject> governmentMessageTypes;
    [SerializeField] private List<string> governmentMessagesContents;
    [SerializeField] private List<GameObject> governmentMessagesDocuments;

    private bool messagesAssigned;

    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (!messagesAssigned && conversationsParent.transform.childCount > 0)
        {
            AssignMessages();
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void AssignMessages()
    {
        foreach (GameObject message in daughterMessageTypes)
        {
            GetAvailableConversationFromRecipient(daughter).messageHistoryMessageTypes.Add(message.name);
        }
        foreach (GameObject message in governmentMessageTypes)
        {
            GetAvailableConversationFromRecipient(government).messageHistoryMessageTypes.Add(message.name);
        }
        GetAvailableConversationFromRecipient(daughter).messageHistoryMessageContents.AddRange(daughterMessagesContents);
        GetAvailableConversationFromRecipient(daughter).messageDocumentsHistory.AddRange(daughterMessagesDocuments);
        GetAvailableConversationFromRecipient(government).messageHistoryMessageContents.AddRange(governmentMessagesContents);
        GetAvailableConversationFromRecipient(government).messageDocumentsHistory.AddRange(governmentMessagesDocuments);

        messagesAssigned = true;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private AvailableConversation GetAvailableConversationFromRecipient(MessagingRecipientSO recipient)
    {
        foreach (Transform child in conversationsParent.transform)
        {
            if (child.GetComponent<AvailableConversation>().respectiveRecipient == recipient)
            {
                return child.GetComponent<AvailableConversation>();
            }
        }
        return null;
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
