using UnityEngine;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////
public class SettingsMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject homeMenu;
    [SerializeField] private GameObject gameSettingsSubmenu;
    [SerializeField] private GameObject audioSettingsSubmenu;
    [SerializeField] private GameObject videoSettingsSubmenu;
    [SerializeField] private GameObject buttonsParent;
    [SerializeField] private GameObject buttonsBlur;

    [Header("Parameters")]
    [SerializeField] private float buttonsScaleInSubMenu;

    [HideInInspector] public bool inSubMenu;


    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        ExitSubmenu();
    }

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && inSubMenu)
        {
            ExitSubmenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void Game()
    {
        OpenSubmenu(gameSettingsSubmenu);

    }

    //////////////////////////////////////////////////////////////////////////////
    public void Audio()
    {
        OpenSubmenu(audioSettingsSubmenu);
    }


    //////////////////////////////////////////////////////////////////////////////
    public void Video()
    {
        OpenSubmenu(videoSettingsSubmenu);

    }

    //////////////////////////////////////////////////////////////////////////////
    public void OpenSubmenu(GameObject submenuToOpen)
    {
        submenuToOpen.SetActive(true);
        buttonsBlur.SetActive(true);
        ToggleButtons(false);
        inSubMenu = true;
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ExitSubmenu()
    {
        gameSettingsSubmenu.SetActive(false);
        audioSettingsSubmenu.SetActive(false);
        videoSettingsSubmenu.SetActive(false);
        buttonsBlur.SetActive(false);
        ToggleButtons(true);
        inSubMenu = false;
    }

    //////////////////////////////////////////////////////////////////////////////
    public void Back()
    {
        homeMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //////////////////////////////////////////////////////////////////////////////
    private void ToggleButtons(bool stateToSetTo)
    {
        foreach (Transform child in buttonsParent.transform)
        {
            if (child.GetComponent<Button>() != null)
            {
                child.GetComponent<Button>().enabled = stateToSetTo;
            }
        }

        if (stateToSetTo)
        {
            buttonsParent.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            buttonsParent.transform.localScale = new Vector3(buttonsScaleInSubMenu, buttonsScaleInSubMenu, buttonsScaleInSubMenu);
        }

        buttonsBlur.SetActive(!stateToSetTo);
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
