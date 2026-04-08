using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class ComputerInteractable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MessagingApplication messagingApplication;
    [SerializeField] private WebBrowser webBrowser;

    private bool playerUsingComputer;

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        GetInput();
    }

    //////////////////////////////////////////////////////////////////////////////
    private void GetInput()
    {
        if (playerUsingComputer && Input.GetKeyDown(InputManager.instance.stopInteractingKey) && !GameManager.instance.gameplayInProgress && !messagingApplication.inConversation)
        {
            StopUsingComputer();
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void UseComputer()
    {
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
}

//////////////////////////////////////////////////////////////////////////////
