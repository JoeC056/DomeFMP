using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class ComputerInteractable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MessagingApplication messagingApplication;
    [SerializeField] private WebBrowser webBrowser;
    [SerializeField] private GameObject openingAnimation;
    [SerializeField] private GameObject cantLeaveComputerNowMessage;

    [Header("Parameters")]
    [SerializeField] private float delayBetweenPartsOfOpeningAnimation;
    [SerializeField] private float computerCantBeLeftMessageDuration;

    private bool playerUsingComputer;
    private bool firstUseOfComputer;
    [HideInInspector] public bool firstOpenOfDay;

    private bool inOpeningAnimation;

    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        firstUseOfComputer = true;
        cantLeaveComputerNowMessage.SetActive(false);
        TurnOffComputer();
    }

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        GetInput();
    }

    //////////////////////////////////////////////////////////////////////////////
    private void GetInput()
    {
        if (playerUsingComputer && Input.GetKeyDown(InputManager.instance.stopInteractingKey) && !GameManager.instance.gameplayInProgress && !messagingApplication.inConversation && !inOpeningAnimation)
        {
            StopUsingComputer();
        }
        else if (playerUsingComputer && Input.GetKeyDown(InputManager.instance.stopInteractingKey) && !cantLeaveComputerNowMessage.activeSelf)
        {
            StartCoroutine(DisplayCantLeaveComputerMessage());  
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void UseComputer()
    {
        if (firstOpenOfDay)
        {
            if (firstUseOfComputer)
            {
                StartCoroutine(DisplayFirstBootOpeningAnimation());
            }
            else
            {
                StartCoroutine(DisplayOpeningAnimation());
            }
        }
        GameManager.instance.stateOfGame = GameManager.States.UsingComputer;
        playerUsingComputer = true;
    }

    //////////////////////////////////////////////////////////////////////////////
    private void StopUsingComputer()
    {
        GameManager.instance.stateOfGame = GameManager.States.InGame;
        messagingApplication.ReturnToMenu();
        webBrowser.HomeButton();

        playerUsingComputer = false;

    }

    //////////////////////////////////////////////////////////////////////////////
    private IEnumerator DisplayFirstBootOpeningAnimation()
    {
        int index = 1;
        inOpeningAnimation = true;
        firstOpenOfDay = false;
        firstUseOfComputer = false;

        yield return new WaitForSeconds(delayBetweenPartsOfOpeningAnimation);
        UpdateOpeningAnimationDisplay(1);
        yield return new WaitForSeconds(delayBetweenPartsOfOpeningAnimation);
        UpdateOpeningAnimationDisplay(2);
        yield return new WaitForSeconds(delayBetweenPartsOfOpeningAnimation);
        UpdateOpeningAnimationDisplay(3);
        yield return new WaitForSeconds(delayBetweenPartsOfOpeningAnimation);
        UpdateOpeningAnimationDisplay(4);
        yield return new WaitForSeconds(delayBetweenPartsOfOpeningAnimation);
        UpdateOpeningAnimationDisplay(5);
        yield return new WaitForSeconds(delayBetweenPartsOfOpeningAnimation);


        openingAnimation.SetActive(false);
        inOpeningAnimation = false;
    }

    //////////////////////////////////////////////////////////////////////////////
    private IEnumerator DisplayOpeningAnimation()
    {
        int index = 1;
        inOpeningAnimation = true;
        firstOpenOfDay = false;

        yield return new WaitForSeconds(delayBetweenPartsOfOpeningAnimation);
        UpdateOpeningAnimationDisplay(5);
        yield return new WaitForSeconds(delayBetweenPartsOfOpeningAnimation);


        openingAnimation.SetActive(false);
        inOpeningAnimation = false;
    }

    //////////////////////////////////////////////////////////////////////////////
    private void UpdateOpeningAnimationDisplay(int index)
    {
        for (int i = 0; i < openingAnimation.transform.childCount; i++)
        {
            if (i == 0 || i == index)
            {
                openingAnimation.transform.GetChild(i).gameObject.SetActive(true);  
            }
            else
            {
                openingAnimation.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

    }

    //////////////////////////////////////////////////////////////////////////////
    public void TurnOffComputer()
    {
        openingAnimation.SetActive(true);
        firstOpenOfDay = true;
        UpdateOpeningAnimationDisplay(0);
    }

    //////////////////////////////////////////////////////////////////////////////
    private IEnumerator DisplayCantLeaveComputerMessage()
    {
        cantLeaveComputerNowMessage.SetActive(true);
        yield return new WaitForSeconds(computerCantBeLeftMessageDuration);
        cantLeaveComputerNowMessage.SetActive(false);
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
