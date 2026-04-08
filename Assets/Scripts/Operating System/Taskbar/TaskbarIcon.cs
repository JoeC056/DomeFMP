using UnityEngine;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////////
public class TaskbarIcon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image iconImage;

    [Header("Respective Application")]

    private ApplicationSO application;

    //////////////////////////////////////////////////////////////////////////////////
    public void OnClick()
    { 
        //If currently focused window == this app, unfocus. Otherwise, focus app
        if (application.applicationName == "Gameplay Application" && GameManager.instance.gameplayInProgress)
        {
            //Doesnt work
        }
        else if (ComputerManager.instance.openWindowsStack.Count > 0)
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
    public void AssignApplicationRef(ApplicationSO appToAssign)
    {
        application = appToAssign;
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void AssignImage(Sprite imageToAssign)
    {
        iconImage.sprite = imageToAssign;
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
