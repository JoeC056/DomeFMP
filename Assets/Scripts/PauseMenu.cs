using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseUI;

    private bool paused;

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
        if (Input.GetKeyDown(InputManager.instance.pauseKey))
        {
            TogglePause();
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    private void TogglePause()
    {
        paused = !paused;
        pauseUI.SetActive(paused);

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
}

//////////////////////////////////////////////////////////////////////////////
