using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////
public class MasterVolume : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private TMP_InputField inputField;

    //////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        inputField.text = (Convert.ToInt32((SettingsManager.instance.MasterVolume * 100))).ToString();
        scrollbar.value = SettingsManager.instance.MasterVolume;
    }


    //////////////////////////////////////////////////////////////////////////////
    public void UpdateBasedOnTextInput()
    {
        SettingsManager.instance.MasterVolume = Convert.ToInt32(inputField.text);
        scrollbar.value = Convert.ToSingle(Convert.ToInt32(inputField.text)) / 100;
    }

    //////////////////////////////////////////////////////////////////////////////
    public void UpdateBasedOnScroll()
    {
        SettingsManager.instance.MasterVolume = scrollbar.value;
        inputField.text = (Convert.ToInt32((SettingsManager.instance.MasterVolume * 100))).ToString();
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
