using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/DialogueNode")]
////////////////////////////////////////////////////////////////////
public class DialogueNode: ScriptableObject
{
    [Header("Data")]
    public string speakerName;
    public string dialogueText;

    [Header("Parameters")]
    public bool shouldChangeConfirmAvailability;
    public bool stateToChangeItTo;

    public DialogueNode subsequentNodes;

    ////////////////////////////////////////////////////////////////////
    public bool IsLastNode()
    {
        return subsequentNodes == null;
    }

    ////////////////////////////////////////////////////////////////////
}

////////////////////////////////////////////////////////////////////