using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class WindowMode : MonoBehaviour
{
    //References
    private TextMeshProUGUI text;

    //////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        if (SettingsManager.instance.windowMode == SettingsManager.WindowModes.WindowedFullscreen)
        {
            text.text = "Windowed\nFullscreen";
        }
        else
        {
            text.text = SettingsManager.instance.windowMode.ToString();
        }

        SettingsManager.instance.UpdateWindowMode();
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ToggleSetting()
    {
        if (SettingsManager.instance.windowMode == SettingsManager.WindowModes.Windowed)
        {
            SettingsManager.instance.windowMode = SettingsManager.WindowModes.WindowedFullscreen;
        }
        else if (SettingsManager.instance.windowMode == SettingsManager.WindowModes.WindowedFullscreen)
        {
            SettingsManager.instance.windowMode = SettingsManager.WindowModes.Fullscreen;
        }
        else if (SettingsManager.instance.windowMode == SettingsManager.WindowModes.Fullscreen)
        {
            SettingsManager.instance.windowMode = SettingsManager.WindowModes.Windowed;
        }


        if (SettingsManager.instance.windowMode == SettingsManager.WindowModes.WindowedFullscreen)
        {
            text.text = "Windowed\nFullscreen";
        }
        else
        {
            text.text = SettingsManager.instance.windowMode.ToString();
        }

        SettingsManager.instance.UpdateWindowMode();
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
