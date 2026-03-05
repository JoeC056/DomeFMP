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

    public Dialogue dialogue;

    private List<string> dialogueLines;
    private List<string> remainingTranscriptDialogue;
    private string remainingLineDialogue = "";

    private bool waiting;


    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        remainingTranscriptDialogue = new List<string>();
        optionSelectButtonParent.SetActive(false);
        waiting = true;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AssignNewTranscriptDialogue();
        }

        if (remainingLineDialogue != "")
        {
            if (!waiting)
            {
                textSpace.text += remainingLineDialogue[0];
                remainingLineDialogue = remainingLineDialogue.Substring(1);
                StartCoroutine(Wait(dialogue.delayBetweenCharacters));
            }

        }
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
        else if (!waiting && remainingTranscriptDialogue.Count <= 0 && dialogue.bridgeAfterDialogue)
        {
            optionSelectButtonParent.SetActive(true);
            optionSelectButtonParent.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = dialogue.bridgeResponse1;
            optionSelectButtonParent.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = dialogue.bridgeResponse2;
        }
        else if (!dialogue.bridgeAfterDialogue)
        {
            EndDialogue();
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void AssignNewTranscriptDialogue()
    {
        foreach (string line in dialogue.lines)
        {
            remainingTranscriptDialogue.Add(line);
        }
        remainingLineDialogue = dialogue.lines[0];
        waiting = false;
        if (dialogue.dialogueHidesSubmit)
        {
            submitButton.SetActive(false);
        }
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
        optionSelectButtonParent.SetActive(false);

        if (branchToGoTo == 1)
        {
            dialogue = dialogue.bridgedDialogue1;
        }
        if (branchToGoTo == 2)
        {
            dialogue = dialogue.bridgedDialogue2;
        }
        AssignNewTranscriptDialogue();
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void EndDialogue()
    {
        waiting = true;
        if (!submitButton.activeSelf)
        { 
            submitButton.SetActive(true);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////
}
