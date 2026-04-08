using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue")]
////////////////////////////////////////////////////////////////////
public class Dialogue : ScriptableObject
{
    [Header("Parameters")]
    public bool dialogueHidesSubmit;

    [Header("Content")]
    public List<string> lines;

    [Header("For bridging dialogue")]
    public bool bridgeAfterDialogue;
    public bool clearTranscriptUponBridging;
    public string bridgeResponse1;
    public string bridgeResponse2;
    public Dialogue bridgedDialogue1;
    public Dialogue bridgedDialogue2;

    [Header("For Dialogue with Documents")]
    public GameObject documentToGive;
    public int noOfMessagesBeforeGivingDocument;
}

////////////////////////////////////////////////////////////////////
    