using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////
public class FieldOfView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private TMP_InputField inputField;

    //////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        inputField.text = SettingsManager.instance.FieldOfView.ToString();
        if (SettingsManager.instance.FieldOfView == 60)
        {
            scrollbar.value = 0;
        }
        else
        {
            scrollbar.value = (Convert.ToSingle(SettingsManager.instance.FieldOfView - 60)) / 30;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void UpdateBasedOnTextInput()
    {
        SettingsManager.instance.FieldOfView = Convert.ToInt32(inputField.text);
        if (SettingsManager.instance.FieldOfView == 60)
        {
            scrollbar.value = 0;
        }
        else
        {
            scrollbar.value = (Convert.ToSingle(SettingsManager.instance.FieldOfView - 60)) / 30;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void UpdateBasedOnScroll()
    {
        SettingsManager.instance.FieldOfView = Convert.ToInt32(60 + (30 * scrollbar.value));
        inputField.text = SettingsManager.instance.FieldOfView.ToString();
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
