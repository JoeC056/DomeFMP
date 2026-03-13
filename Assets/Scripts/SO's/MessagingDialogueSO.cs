using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Messaging Dialogue", menuName = "Messaging Dialogue/New Messaging Dialogue")]
////////////////////////////////////////////////////////////////////
public class MessagingDialogueSO : ScriptableObject
{
    [Header("Parameters")]
    public MessagingRecipientSO personSpokenTo;
    public float delayBetweenMessages;

    [Header("Content")]
    public List<string> lines;

    [Header("For bridging dialogue")]
    public bool bridgeAfterMessages;
    public string bridgeResponse1;
    public string bridgeResponse2;
    public MessagingDialogueSO bridgedDialogue1;
    public MessagingDialogueSO bridgedDialogue2;

    [Header("For Dialogue that contains a Document")]
    public bool documentAfterMessages;
    public MessageType documentMessageType;
    public GameObject document;
    public bool continueAfterDocument;
    public MessagingDialogueSO continuedDialogue;

    public enum MessageType
    {
        Received,
        Sent
    }
}

////////////////////////////////////////////////////////////////////
    