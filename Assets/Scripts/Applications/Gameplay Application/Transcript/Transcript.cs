using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class Transcript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI textSpace;
    [SerializeField] private GameObject optionSelectButtonParent;
    [SerializeField] private GameObject submitButton;

    //Currently used dialogue
    private Dialogue dialogue;

    //Content of the dialogue
    private List<string> remainingTranscriptDialogue;
    private string remainingLineDialogue = "";

    //Whether waiting to ammend new text or start new line
    private bool waiting;

    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        //Assigns default values
        optionSelectButtonParent.SetActive(false);
        waiting = true;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (dialogue != null)
        {
            DisplayDialogue();
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void DisplayDialogue()
    {
        //Appends dialogue to text box if remaining text in line and not waiting 
        if (remainingLineDialogue != "")
        {
            if (!waiting)
            {
                textSpace.text += remainingLineDialogue[0];
                remainingLineDialogue = remainingLineDialogue.Substring(1);
                StartCoroutine(Wait(dialogue.delayBetweenCharacters));
            }

        }
        //Starts a new line if current one complete
        else if (remainingTranscriptDialogue.Count > 0)
        {
            if (!waiting)
            {
                textSpace.text += "\n";
                StartCoroutine(Wait(dialogue.delayBetweenMessages));
                remainingTranscriptDialogue.Remove(remainingTranscriptDialogue[0]);
                if (remainingTranscriptDialogue.Count > 0)
                {
                    remainingLineDialogue = remainingTranscriptDialogue[0];
                }
            }
        }
        //Displays option select if reached end of dialogue and branching is present
        else if (!waiting && remainingTranscriptDialogue.Count <= 0 && dialogue.bridgeAfterDialogue)
        {
            optionSelectButtonParent.SetActive(true);
            optionSelectButtonParent.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = dialogue.bridgeResponse1;
            optionSelectButtonParent.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = dialogue.bridgeResponse2;
        }
        //Ends dialogue if dialogue complete branching not present 
        else if (!dialogue.bridgeAfterDialogue)
        {
            EndDialogue();
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void AssignNewTranscriptDialogue(Dialogue newDialogue)
    {
        remainingTranscriptDialogue = new List<string>();

        //Assigns variable values for new dialogue
        dialogue = newDialogue;
        foreach (string line in dialogue.lines)
        {
            remainingTranscriptDialogue.Add(line);
        }
        remainingLineDialogue = dialogue.lines[0];

        //Hides dialogue button if dialogue requires it 
        if (dialogue.dialogueHidesSubmit)
        {
            submitButton.SetActive(false);
        }

        waiting = false;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private IEnumerator Wait(float waitingDuration)
    {
        waiting = true;
        yield return new WaitForSeconds(waitingDuration);
        waiting = false;
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void BranchDialogue(int branchToGoTo)
    {
        //Branches dialogue based on pressed button 
        optionSelectButtonParent.SetActive(false);

        if (branchToGoTo == 1)
        {
            AssignNewTranscriptDialogue(dialogue.bridgedDialogue1);
        }
        if (branchToGoTo == 2)
        {
            AssignNewTranscriptDialogue(dialogue.bridgedDialogue2);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void EndDialogue()
    {
        waiting = true;
        optionSelectButtonParent.SetActive(false);
        if (!submitButton.activeSelf)
        { 
            submitButton.SetActive(true);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////
    public void ClearDialogue()
    {
        EndDialogue();
        dialogue = null;
        textSpace.text = "";
    }

    /////////////////////////////////////////////////////////////////////////////////
}

/////////////////////////////////////////////////////////////////////////////////
