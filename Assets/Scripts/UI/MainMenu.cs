using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////
public class MainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject creditsUI;
    [SerializeField] private GameObject renderTextureImage;
    [SerializeField] private GameObject titleText;
    [SerializeField] private GameObject buttonsParent;

    [Header("Parameters")]
    [SerializeField] private float textStartSize;
    [SerializeField] private float textEndSize;
    [SerializeField] private float fadeInSpeed;

    private bool playingOpeningAnimation;
    private bool backgroundHalfRendered;
    private float amountToDecrementTextSizeBy;

    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        creditsUI.SetActive(false);
        settingsUI.SetActive(false);    

        PlayOpeningAnimation();
    }

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && creditsUI.activeSelf)
        {
            CreditsBackButton();
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //////////////////////////////////////////////////////////////////////////////
    private void FixedUpdate()
    {
        if (playingOpeningAnimation)
        {
            renderTextureImage.GetComponent<CanvasGroup>().alpha += fadeInSpeed;

            if (renderTextureImage.GetComponent<CanvasGroup>().alpha >= 0.5 && !backgroundHalfRendered)
            {
                backgroundHalfRendered = true;
                amountToDecrementTextSizeBy = (textStartSize - textEndSize) / (1 / (fadeInSpeed * 2));
            }

            if (backgroundHalfRendered)
            {
                titleText.GetComponent<CanvasGroup>().alpha += fadeInSpeed * 2;
                buttonsParent.GetComponent<CanvasGroup>().alpha += fadeInSpeed * 2;
                titleText.GetComponent<TextMeshProUGUI>().fontSize -= amountToDecrementTextSizeBy;

            }
            if (renderTextureImage.GetComponent<CanvasGroup>().alpha == 1)
            {
                titleText.GetComponent<TextMeshProUGUI>().fontSize = textEndSize;
                ToggleButtonFunctionality(true);
                playingOpeningAnimation = false;
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    //////////////////////////////////////////////////////////////////////////////
    public void Exit()
    {
        UnityEngine.Application.Quit();
    }

    //////////////////////////////////////////////////////////////////////////////
    public void Settings()
    {
        settingsUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    //////////////////////////////////////////////////////////////////////////////
    public void SettingsBackButton()
    {
        settingsUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    //////////////////////////////////////////////////////////////////////////////
    public void Credits()
    {
        creditsUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    //////////////////////////////////////////////////////////////////////////////
    public void CreditsBackButton()
    {
        creditsUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    //////////////////////////////////////////////////////////////////////////////
    public void PlayOpeningAnimation()
    {
        ToggleButtonFunctionality(false);
        renderTextureImage.GetComponent<CanvasGroup>().alpha = 0;
        buttonsParent.GetComponent<CanvasGroup>().alpha = 0;
        titleText.GetComponent<CanvasGroup>().alpha = 0;
        titleText.GetComponent<TextMeshProUGUI>().fontSize = textStartSize;

        backgroundHalfRendered = false;
        playingOpeningAnimation = true;
    }

    //////////////////////////////////////////////////////////////////////////////
    private void ToggleButtonFunctionality(bool stateToSetTo)
    {
        foreach (Transform child in buttonsParent.transform)
        {
            child.GetComponent<Button>().enabled = stateToSetTo;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////

