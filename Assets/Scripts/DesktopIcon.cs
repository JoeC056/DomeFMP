using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class DesktopIcon : MonoBehaviour
{
    [Header("Respective Application")]
    [SerializeField] private ApplicationSO application;

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
