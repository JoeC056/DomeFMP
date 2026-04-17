using UnityEngine;
using System;

//////////////////////////////////////////////////////////////////////////////
public class SettingsManager : MonoBehaviour
{
    [Header("Game Settings")]
    public bool crosshairEnabled;
    public bool stylizedCursorEnabled;

    [Header("Audio Settings")]
    private float masterVolume;
    public float MasterVolume
    {
        get { return masterVolume; }
        set { masterVolume = Mathf.Clamp(value, 0, 1); }
    }

    private float gameVolume;
    public float GameVolume
    {
        get { return gameVolume; }
        set { gameVolume = Mathf.Clamp(value, 0, 1); }
    }

    private float computerUIVolume;
    public float ComputerUIVolume
    {
        get { return computerUIVolume; }
        set { computerUIVolume = Mathf.Clamp(value, 0, 1); }
    }

    private float menuUIVolume;
    public float MenuUIVolume
    {
        get { return menuUIVolume; }
        set { menuUIVolume = Mathf.Clamp(value, 0, 1); }
    }

    private float sensitivity;
    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = Convert.ToSingle(System.Math.Round(Mathf.Clamp(value, 0, 5),2)); }
    }


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
    private void Update()
    {
        Debug.Log("Master: " + MasterVolume);
        Debug.Log("Game: " + GameVolume);
        Debug.Log("Comp: " + ComputerUIVolume);
        Debug.Log("Menu: " + MenuUIVolume);
    }
}

//////////////////////////////////////////////////////////////////////////////
