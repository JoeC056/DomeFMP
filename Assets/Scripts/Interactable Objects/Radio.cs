using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class Radio : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject radioUI;
    [SerializeField] private TextMeshProUGUI textSpace;

    [Header("Currently available transmission")]
    public RadioTransmissionSO currentRadioTransmission;

    //Content to display
    private List<string> remainingTransmissionDialogue;
    private string remainingLineDialogue = "";

    //Whether waiting to ammend text again
    private bool waiting;
    private bool waitedForThisLineAlready;

    private bool usingRadio;
    private bool radioAlreadyUsed;


    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        radioUI.SetActive(false);
        remainingTransmissionDialogue = new List<string>();
    }

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        GetInput();
        if (usingRadio && currentRadioTransmission != null)
        {
            DisplayTransmission();
        }
        if (!radioAlreadyUsed && currentRadioTransmission != null)
        {
            PlayStaticSound();
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    private void GetInput()
    {
        //Puts down radio if currently held 
        if (usingRadio && Input.GetKeyDown(InputManager.instance.stopInteractingKey))
        {
            PutDownRadio();
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void PickupRadio()
    {
        if (currentRadioTransmission != null)
        {
            //Assigns values of variables
            usingRadio = true;
            radioUI.SetActive(true);
            radioAlreadyUsed = true;
            GameManager.instance.stateOfGame = GameManager.States.InteractingWithObject;

            AssignTextToDisplay();
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    private void PutDownRadio()
    {
        //Assigns values of variables
        usingRadio = false;
        radioUI.SetActive(false);
        GameManager.instance.stateOfGame = GameManager.States.InGame;

        //Disables any coroutines & waiting to prevent bugs upon repicking up radio
        StopAllCoroutines();
        waiting = false;
        waitedForThisLineAlready = false;
    }

    //////////////////////////////////////////////////////////////////////////////
    private void AssignTextToDisplay()
    {
        //First empties the values
        remainingTransmissionDialogue.Clear();
        remainingLineDialogue = "";
        textSpace.text = "";

        //Assigns values based on content of current transmission
        foreach (string line in currentRadioTransmission.lines)
        {
            remainingTransmissionDialogue.Add(line);
        }
        remainingLineDialogue = currentRadioTransmission.lines[0];
    }

    //////////////////////////////////////////////////////////////////////////////
    private void DisplayTransmission()
    {
        //Appends dialogue to text box if remaining text in line and not waiting 
        if (remainingLineDialogue != "")
        {
            if (!waiting)
            {
                textSpace.text += remainingLineDialogue[0];
                remainingLineDialogue = remainingLineDialogue.Substring(1);
                if (remainingLineDialogue != "")
                {
                    StartCoroutine(Wait(currentRadioTransmission.delayBetweenCharacters));
                }
            }

        }
        //Waits between lines if line already complete 
        else if (!waitedForThisLineAlready && !(remainingTransmissionDialogue.Count == 0))
        {
            StartCoroutine(Wait(currentRadioTransmission.delayBetweenLines));
            waitedForThisLineAlready = true;
        }

        //Starts a new line if finished waiting after previous line 
        else if (remainingTransmissionDialogue.Count > 0)
        {
            if (!waiting)
            {
                textSpace.text = "";
                remainingTransmissionDialogue.Remove(remainingTransmissionDialogue[0]);
                if (remainingTransmissionDialogue.Count > 0)
                {
                    remainingLineDialogue = remainingTransmissionDialogue[0];
                }
                waitedForThisLineAlready = false;
            }
        }

        //Puts down radio upon completing the transmission
        else if (!waiting)
        {
            currentRadioTransmission = null;
            PutDownRadio();
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    private IEnumerator Wait(float waitingDuration)
    {
        waiting = true;
        yield return new WaitForSeconds(waitingDuration);
        waiting = false;
    }

    //////////////////////////////////////////////////////////////////////////////
    private void PlayStaticSound()
    {
        Debug.Log("Bzzzzz");
    }


    //////////////////////////////////////////////////////////////////////////////
    public void AddNewTransmission(RadioTransmissionSO newTransmission)
    {
        currentRadioTransmission = newTransmission;
        radioAlreadyUsed = false;
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
