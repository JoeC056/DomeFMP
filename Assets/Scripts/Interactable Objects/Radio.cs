using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

//////////////////////////////////////////////////////////////////////////////
public class Radio : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject radioUI;
    [SerializeField] private GameObject uiRadio;
    [SerializeField] private TextMeshProUGUI textSpace;

    [Header("Parameters")]
    [SerializeField] private float delayBetweenCharacters;
    [SerializeField] private float delayBetweenLines;

    [Header("Sound Data")]
    [SerializeField] private AudioClip staticSound;

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

    private struct transmissionCompletionEvent
    {
        public RadioTransmissionSO respectiveTransmission;
        public UnityEvent eventOnCompletion;
    }

    private List<transmissionCompletionEvent> transmissionCompletionEvents;

    private InteractableObject interactableObjectScript;

    private AudioSource audioSource;
    private float defaultVolume;


    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        interactableObjectScript = GetComponent<InteractableObject>();
        audioSource = GetComponent<AudioSource>();

        radioUI.SetActive(false);
        remainingTransmissionDialogue = new List<string>();
        transmissionCompletionEvents = new List<transmissionCompletionEvent>();
        defaultVolume = audioSource.volume;
    }

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        audioSource.volume = defaultVolume * SettingsManager.instance.MasterVolume * SettingsManager.instance.GameVolume;
        GetInput();
        interactableObjectScript.interactable = (currentRadioTransmission != null);

        bool shouldPlayStatic;

        if (usingRadio && currentRadioTransmission != null)
        {
            DisplayTransmission();
        }
        if (currentRadioTransmission != null)
        {
            if (currentRadioTransmission.mandatory || !radioAlreadyUsed && GameManager.instance.stateOfGame == GameManager.States.InGame)
            {
                shouldPlayStatic = true;
            }
            else
            {
                shouldPlayStatic = false;
            }
        }
        else
        {
            shouldPlayStatic = false;
        }

        CheckSoundToPlay(shouldPlayStatic);
        CheckToDisplay();
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

        //Assigns values of variables
        usingRadio = true;
        radioUI.SetActive(true);
        radioAlreadyUsed = true;
        GameManager.instance.stateOfGame = GameManager.States.InteractingWithObject;

        AssignTextToDisplay();

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
                    StartCoroutine(Wait(delayBetweenCharacters));
                }
            }

        }
        //Waits between lines if line already complete 
        else if (!waitedForThisLineAlready && !(remainingTransmissionDialogue.Count == 0))
        {
            StartCoroutine(Wait(delayBetweenLines));
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

            if (GetRespectiveEventForTransmission() != null)
            {
                GetRespectiveEventForTransmission().Invoke();
            }
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
    private void CheckSoundToPlay(bool playStatic)
    {
        if (playStatic && !usingRadio)
        {
            if (audioSource.clip != staticSound)
            {
                audioSource.clip = staticSound;
                audioSource.Play();
                audioSource.loop = true;
            }
        }
        else
        {
            audioSource.clip = null;
            audioSource.Stop();
            audioSource.loop = false;
        }
    }


    //////////////////////////////////////////////////////////////////////////////
    public void AddNewTransmission(RadioTransmissionSO newTransmission)
    {
        currentRadioTransmission = newTransmission;
        radioAlreadyUsed = false;
    }

    //////////////////////////////////////////////////////////////////////////////
    public void AddEventOnTransmissionCompletion(RadioTransmissionSO transmission, UnityEvent newEvent)
    {
        transmissionCompletionEvent diaCompEvent = new transmissionCompletionEvent();
        diaCompEvent.respectiveTransmission = transmission;
        diaCompEvent.eventOnCompletion = newEvent;

        transmissionCompletionEvents.Add(diaCompEvent);
    }

    //////////////////////////////////////////////////////////////////////////////////
    private UnityEvent GetRespectiveEventForTransmission()
    {
        foreach (transmissionCompletionEvent compEvent in transmissionCompletionEvents)
        {
            if (compEvent.respectiveTransmission == currentRadioTransmission)
            {
                return compEvent.eventOnCompletion;
            }
        }
        return null;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void CheckToDisplay()
    {
        GetComponent<MeshRenderer>().enabled = !usingRadio;
        uiRadio.SetActive(usingRadio);
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
