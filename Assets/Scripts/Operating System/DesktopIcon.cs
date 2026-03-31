using TMPro;
using UnityEngine;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////////
public class DesktopIcon : MonoBehaviour
{
    [Header("Respective Application")]
    [SerializeField] private ApplicationSO application;

    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        GetComponent<Image>().sprite = application.desktopIconImage;
        GetComponentInChildren<TextMeshProUGUI>().text = application.applicationName;
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void OnClick()
    {
        //Focuses app if already open, otherwise opens it
        if (!ComputerManager.instance.openApplications.Contains(application))
        {
            ComputerManager.instance.OpenApplication(application);
        }
        else
        {
            ComputerManager.instance.FocusApplication(application);
        }
    }
    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
