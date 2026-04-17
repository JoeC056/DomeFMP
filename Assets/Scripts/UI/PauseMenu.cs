using UnityEngine;
using UnityEngine.SceneManagement;

//////////////////////////////////////////////////////////////////////////////
public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SettingsMenu settingsMenu;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject playerHUD;

    public bool paused;

    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        //Disables pause menu at game start
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
        if (Input.GetKeyDown(InputManager.instance.pauseKey) && !(GameManager.instance.stateOfGame == GameManager.States.WatchingEndingSequence) && !settingsMenu.inSubMenu)
        {
            TogglePause();
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void TogglePause()
    {
        paused = !paused;
        pauseUI.SetActive(paused);
        playerHUD.SetActive(!paused);

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
}

//////////////////////////////////////////////////////////////////////////////
