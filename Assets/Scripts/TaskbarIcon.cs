using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class TaskbarIcon : MonoBehaviour
{
    [Header("Respective Application")]
    public ApplicationSO application;

    //////////////////////////////////////////////////////////////////////////////////
    public void OnClick()
    { 
        //If currently focused window == this app, unfocus. Otherwise, focus app
        if (ComputerManager.instance.openWindowsStack.Count > 0)
        {
            if (ComputerManager.instance.openWindowsStack.First.Value == application)
            {
                ComputerManager.instance.UnfocusApplication(application);
            }
            else
            {
                ComputerManager.instance.FocusApplication(application);
            }
        }
        else
        {
            ComputerManager.instance.FocusApplication(application);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
}
