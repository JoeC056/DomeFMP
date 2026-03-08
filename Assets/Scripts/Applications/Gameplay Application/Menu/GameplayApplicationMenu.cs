using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class GameplayApplicationMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject minimizeButton;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private GameObject startButton;

    public bool gameplayAvailable;

    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        gameplayUI.SetActive(false);
        CheckToDisplayStartButton();
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void StartGameplay()
    {
        gameplayAvailable = false;

        gameplayUI.SetActive(true);
        CheckToDisplayStartButton();
        minimizeButton.SetActive(false); 
        closeButton.SetActive(false);

    }

    //////////////////////////////////////////////////////////////////////////////////
    public void ConcludeGameplay()
    {
        gameplayUI.SetActive(false);
        CheckToDisplayStartButton();
        minimizeButton.SetActive(true);
        closeButton.SetActive(true);
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void CheckToDisplayStartButton()
    {
        startButton.SetActive(gameplayAvailable);
    }
}

//////////////////////////////////////////////////////////////////////////////////
