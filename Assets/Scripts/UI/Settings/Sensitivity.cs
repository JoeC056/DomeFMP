using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////
public class Sensitivity : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private TMP_InputField inputField;

    //////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        inputField.text = SettingsManager.instance.Sensitivity.ToString();
        scrollbar.value = SettingsManager.instance.Sensitivity;
    }


    //////////////////////////////////////////////////////////////////////////////
    public void UpdateBasedOnTextInput()
    {
        SettingsManager.instance.Sensitivity = Convert.ToSingle(inputField.text);
        scrollbar.value = SettingsManager.instance.Sensitivity / 5;
    }

    //////////////////////////////////////////////////////////////////////////////
    public void UpdateBasedOnScroll()
    {
        SettingsManager.instance.Sensitivity = scrollbar.value * 5;
        inputField.text = SettingsManager.instance.Sensitivity.ToString();
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
