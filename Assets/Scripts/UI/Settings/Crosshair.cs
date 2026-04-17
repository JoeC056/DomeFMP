using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class Crosshair : MonoBehaviour
{
    //References
    private TextMeshProUGUI text;

    //////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        if (SettingsManager.instance.crosshairEnabled)
        {
            text.text = "Enabled";
        }
        else
        {
            text.text = "Disabled";
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ToggleSetting()
    {
        if (SettingsManager.instance.crosshairEnabled)
        {
            SettingsManager.instance.crosshairEnabled = false;
            text.text = "Disabled";
        }
        else
        {
            SettingsManager.instance.crosshairEnabled = true;
            text.text = "Enabled";
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
