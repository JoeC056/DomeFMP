using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//////////////////////////////////////////////////////////////////////////////
public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SettingsMenu settingsMenu;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject homeUI;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private Subtitles subtitles;
    [SerializeField] private GameObject endingChoiceSelectUI;

    public bool paused;

    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        //Disables pause menu at game start
        settingsUI.SetActive(false);
        pauseUI.SetActive(false);
        paused = false;
    }

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        GetInput();
    }

    //////////////////////////////////////////////////////////////////////////////
    private void GetInput()
    {
        if (Input.GetKeyDown(InputManager.instance.pauseKey) && !(GameManager.instance.stateOfGame == GameManager.States.WatchingEndingSequence && !endingChoiceSelectUI.activeSelf) && !settingsMenu.inSubMenu && !settingsUI.activeSelf)
        {
            TogglePause();
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void TogglePause()
    {
        if (GameManager.instance.stateOfGame == GameManager.States.WatchingEndingSequence)
        {
            GameManager.instance.stateOfGame = GameManager.States.InGame;
        }
        paused = !paused;
        pauseUI.SetActive(paused);
        playerHUD.SetActive(!paused);
        StartCoroutine(UpdateSubtitles());

        //Checks whether to freeze time based on paused or not
        if (paused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    ///////////////////////////////////////////////////////////////////////////////
    public void Settings()
    {
        homeUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    ///////////////////////////////////////////////////////////////////////////////
    private IEnumerator UpdateSubtitles()
    {
        yield return new WaitForSeconds(0.05f);
        subtitles.CheckHowLongToWait();
    }

    ///////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
