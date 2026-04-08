using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class GameplayApplicationMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject postGameplayUI;
    [SerializeField] private GameObject minimizeButton;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject tutorialButton;
    [SerializeField] private GameObject menuText;
    [SerializeField] private GameObject taskbar;
    [SerializeField] private GameObject tutorialUI;

    private bool tutorialComplete;
    private bool viewingTutorial;

    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        gameplayUI.SetActive(false);
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        CheckUIToDisplay();
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void StartGameplay()
    {
        gameplayUI.SetActive(true);
        minimizeButton.SetActive(false); 
        closeButton.SetActive(false);
        GameManager.instance.StartDaysGameplay(); 
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void ConcludeGameplay()
    {
        gameplayUI.SetActive(false);
        minimizeButton.SetActive(true);
        closeButton.SetActive(true);
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void CheckUIToDisplay()
    {
        if (!tutorialComplete)
        {
            tutorialButton.SetActive(true);
            tutorialUI.SetActive(viewingTutorial);
            menuText.SetActive(!viewingTutorial);
            startButton.SetActive(false);
        }
        else if (GameManager.instance.gameplayInProgress || GameManager.instance.daysGameplayCompleted || !GameManager.instance.daysGameplayAvailable)
        {
            startButton.SetActive(false);
            menuText.SetActive(false);
            tutorialButton.SetActive(false);
            tutorialUI.SetActive(false);
        }
        else
        {
            startButton.SetActive(true);
            menuText.SetActive(true);
            tutorialButton.SetActive(false);
            tutorialUI.SetActive(false);
        }    

        postGameplayUI.SetActive(GameManager.instance.daysGameplayCompleted);
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void TutorialButton()
    {
        if (!viewingTutorial)
        {
            viewingTutorial = true;
            tutorialButton.GetComponentInChildren<TextMeshProUGUI>().text = "Finish";
        }
        else
        {
            tutorialComplete = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
