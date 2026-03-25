using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class GameplayApplicationMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject minimizeButton;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject taskbar;


    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        gameplayUI.SetActive(false);
        CheckToDisplayStartButton();
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void StartGameplay()
    {
        gameplayUI.SetActive(true);
        minimizeButton.SetActive(false); 
        closeButton.SetActive(false);
        GameManager.instance.StartDaysGameplay(); 
        CheckToDisplayStartButton();
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
        if (GameManager.instance.gameplayInProgress || GameManager.instance.daysGameplayCompleted || !GameManager.instance.daysGameplayAvailable)
        {
            startButton.SetActive(false);
        }
        else
        {
            startButton.SetActive(true);
        }    
    }
}

//////////////////////////////////////////////////////////////////////////////////
