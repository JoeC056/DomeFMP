using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class Transcript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI textSpace;

    public Dialogue dialogue;

    private List<string> remainingTranscriptDialogue;

    private bool waiting;


    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        remainingTranscriptDialogue = new List<string>();
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AssignNewTranscriptDialogue();
        }
        if (remainingTranscriptDialogue.Count > 0)
        {
            if (!waiting)
            {
                textSpace.text += "\n" + remainingTranscriptDialogue[0];
                StartCoroutine(Wait(dialogue.timeBetweenMessages));
                remainingTranscriptDialogue.Remove(remainingTranscriptDialogue[0]);
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void AssignNewTranscriptDialogue()
    {
        remainingTranscriptDialogue = dialogue.lines;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private IEnumerator Wait(float waitingDuration)
    {
        waiting = true;
        yield return new WaitForSeconds(waitingDuration);
        waiting = false;
    }

    //////////////////////////////////////////////////////////////////////////////////
}
