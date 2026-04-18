using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////
public class ComputerUIVolume : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private TMP_InputField inputField;

    //////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        inputField.text = (Convert.ToInt32((SettingsManager.instance.ComputerUIVolume * 100))).ToString();
        scrollbar.value = SettingsManager.instance.ComputerUIVolume;
    }

    //////////////////////////////////////////////////////////////////////////////
    public void UpdateBasedOnTextInput()
    {
        SettingsManager.instance.ComputerUIVolume = Convert.ToInt32(inputField.text);
        scrollbar.value = Convert.ToSingle(Convert.ToInt32(inputField.text)) / 100;
    }

    //////////////////////////////////////////////////////////////////////////////
    public void UpdateBasedOnScroll()
    {
        SettingsManager.instance.ComputerUIVolume = scrollbar.value;
        inputField.text = (Convert.ToInt32((SettingsManager.instance.ComputerUIVolume * 100))).ToString();
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
