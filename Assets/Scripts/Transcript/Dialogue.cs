using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/Dialogue")]
////////////////////////////////////////////////////////////////////
public class Dialogue : ScriptableObject
{
    [Header("Parameters")]
    public float delayBetweenCharacters;
    public float delayBetweenMessages;
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
}

////////////////////////////////////////////////////////////////////
    