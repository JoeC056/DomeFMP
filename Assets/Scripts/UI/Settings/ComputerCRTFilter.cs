using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class ComputerCRTFilter : MonoBehaviour
{
    //References
    private TextMeshProUGUI text;

    //////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        if (SettingsManager.instance.computerCRTFilter)
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
        if (SettingsManager.instance.computerCRTFilter)
        {
            SettingsManager.instance.computerCRTFilter = false;
            text.text = "Disabled";
        }
        else
        {
            SettingsManager.instance.computerCRTFilter = true;
            text.text = "Enabled";
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
