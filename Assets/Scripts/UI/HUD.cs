using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class HUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject ending1UI;
    [SerializeField] private GameObject ending2UI;

    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        ending1UI.SetActive(false);
        ending2UI.SetActive(false);
    }

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        //HUD only enable when exploring the level or in ending sequence
        hud.SetActive(GameManager.instance.stateOfGame == GameManager.States.InGame);

        if (hud.activeSelf)
        {
            crosshair.SetActive(SettingsManager.instance.crosshairEnabled);
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////

