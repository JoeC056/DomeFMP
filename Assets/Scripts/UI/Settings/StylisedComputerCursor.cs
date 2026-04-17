using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class StylisedComputerCursor : MonoBehaviour
{
    //References
    private TextMeshProUGUI text;

    //////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        if (SettingsManager.instance.stylizedCursorEnabled)
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
        if (SettingsManager.instance.stylizedCursorEnabled)
        {
            SettingsManager.instance.stylizedCursorEnabled = false;
            text.text = "Disabled";
        }
        else
        {
            SettingsManager.instance.stylizedCursorEnabled = true;
            text.text = "Enabled";
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
