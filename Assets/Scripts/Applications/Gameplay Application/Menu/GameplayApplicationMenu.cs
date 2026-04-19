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
    [SerializeField] private GameObject startAndSymptomsButtons;
    [SerializeField] private GameObject tutorialButton;
    [SerializeField] private GameObject menuText;
    [SerializeField] private GameObject taskbar;
    [SerializeField] private GameObject tutorialUI;
    [SerializeField] private GameObject whatToCheckInfoParent;
    [SerializeField] private GameObject day1WhatToCheckInfo;
    [SerializeField] private GameObject day2WhatToCheckInfo;
    [SerializeField] private GameObject day3WhatToCheckInfo;
    [SerializeField] private GameObject day4WhatToCheckInfo;

    private bool tutorialComplete;
    private bool viewingTutorial;
    private bool viewingWhatToCheck;

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
            startAndSymptomsButtons.SetActive(false);
            whatToCheckInfoParent.SetActive(false);
        }
        else if (GameManager.instance.gameplayInProgress || GameManager.instance.daysGameplayCompleted || !GameManager.instance.daysGameplayAvailable)
        {
            startAndSymptomsButtons.SetActive(false);
            menuText.SetActive(false);
            tutorialButton.SetActive(false);
            tutorialUI.SetActive(false);
            whatToCheckInfoParent.SetActive(false);
        }
        else if (viewingWhatToCheck)
        {
            startAndSymptomsButtons.SetActive(false);
            menuText.SetActive(false);
            tutorialButton.SetActive(false);
            tutorialUI.SetActive(false);
            whatToCheckInfoParent.SetActive(true);
        }
        else
        {
            startAndSymptomsButtons.SetActive(true);
            menuText.SetActive(true);
            tutorialButton.SetActive(false);
            tutorialUI.SetActive(false);
            whatToCheckInfoParent.SetActive(false);
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
    public void WhatToCheck()
    {
        viewingWhatToCheck = true;
        day1WhatToCheckInfo.SetActive(false);
        day2WhatToCheckInfo.SetActive(false);
        day3WhatToCheckInfo.SetActive(false);
        day4WhatToCheckInfo.SetActive(false);

        switch (GameManager.instance.dayNo)
        {
            case 1:
                day1WhatToCheckInfo.SetActive(true);
                break;
            case 2:
                day2WhatToCheckInfo.SetActive(true);
                break;
            case 3:
                day3WhatToCheckInfo.SetActive(true);
                break;
            case 4:
                day4WhatToCheckInfo.SetActive(true);
                break;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void WhatToCheckBackButton()
    {
        viewingWhatToCheck = false;
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////