using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class GameManager : MonoBehaviour
{
    //Instance of GameManager
    public static GameManager instance { get; private set; }

    //Current state of game
    public enum States
    {
        InGame,
        UsingComputer,
        Paused
    }
    public States stateOfGame;

    public int score;

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
        //Mouse is hidden when exploring level 
        if (stateOfGame == States.InGame) 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}
