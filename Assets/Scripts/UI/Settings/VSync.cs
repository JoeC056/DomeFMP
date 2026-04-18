using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class VSync : MonoBehaviour
{
    //References
    private TextMeshProUGUI text;

    //////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        if (SettingsManager.instance.vSync)
        {
            text.text = "Enabled";
        }
        else
        {
            text.text = "Disabled";
        }

        SettingsManager.instance.UpdateVync();  
    }

    //////////////////////////////////////////////////////////////////////////////
    public void ToggleSetting()
    {
        if (SettingsManager.instance.vSync)
        {
            SettingsManager.instance.vSync = false;
            text.text = "Disabled";
        }
        else
        {
            SettingsManager.instance.vSync = true;
            text.text = "Enabled";
        }

        SettingsManager.instance.UpdateVync();
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
