using System;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class SettingsManager : MonoBehaviour
{
    [Header("Game Settings")]
    public bool crosshairEnabled;
    public bool stylizedCursorEnabled;
    [SerializeField] private float sensitivity;
    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = Convert.ToSingle(System.Math.Round(Mathf.Clamp(value, 0, 5), 2)); }
    }


    [Header("Audio Settings")]
    [SerializeField] private float masterVolume;
    public float MasterVolume
    {
        get { return masterVolume; }
        set { masterVolume = Mathf.Clamp(value, 0, 1); }
    }

    [SerializeField] private float gameVolume;
    public float GameVolume
    {
        get { return gameVolume; }
        set { gameVolume = Mathf.Clamp(value, 0, 1); }
    }

    [SerializeField] private float computerUIVolume;
    public float ComputerUIVolume
    {
        get { return computerUIVolume; }
        set { computerUIVolume = Mathf.Clamp(value, 0, 1); }
    }

    [SerializeField] private float menuUIVolume;
    public float MenuUIVolume
    {
        get { return menuUIVolume; }
        set { menuUIVolume = Mathf.Clamp(value, 0, 1); }
    }


    [Header("Video Settings")]
    [SerializeField] private int fieldOfView;
    public int FieldOfView
    {
        get { return fieldOfView; }
        set { fieldOfView = Mathf.Clamp(value, 60, 90); }
    }

    public bool vSync;

    public enum WindowModes
    {
        Windowed,
        WindowedFullscreen,
        Fullscreen
    }
    public WindowModes windowMode;


    //Instance of SettingsManager
    public static SettingsManager instance { get; private set; }

    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        //Ensures singleton nature of instance variable
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void UpdateWindowMode()
    {
        switch (windowMode)
        {
            case WindowModes.Windowed:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case WindowModes.WindowedFullscreen:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case WindowModes.Fullscreen:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    public void UpdateVync()
    {
        if (vSync)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
