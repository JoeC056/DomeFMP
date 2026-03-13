using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class InputManager : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode interactKey;
    public KeyCode pauseKey;
    public KeyCode stopInteractingKey;

    //Instance of InputManager
    public static InputManager instance { get; private set; }

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
}

//////////////////////////////////////////////////////////////////////////////
