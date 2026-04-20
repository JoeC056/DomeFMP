using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class ComputerCRTLines : MonoBehaviour
{
    //References
    private TextMeshProUGUI text;

    //////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        if (SettingsManager.instance.computerCRTLines)
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
        if (SettingsManager.instance.computerCRTLines)
        {
            SettingsManager.instance.computerCRTLines = false;
            text.text = "Disabled";
        }
        else
        {
            SettingsManager.instance.computerCRTLines = true;
            text.text = "Enabled";
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
