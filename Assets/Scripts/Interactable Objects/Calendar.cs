using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class Calendar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshPro dateText;

    private bool playerInteractingWithCalendar;

    //////////////////////////////////////////////////////////////////////////////
    public void OnClick()
    {
        playerInteractingWithCalendar = true;
        GameManager.instance.stateOfGame = GameManager.States.InteractingWithCalendar;
    }

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        dateText.text = GameManager.instance.dayNo.ToString();
        GetInput();
    }

    //////////////////////////////////////////////////////////////////////////////
    private void GetInput()
    {
        if (playerInteractingWithCalendar && Input.GetKeyDown(InputManager.instance.stopInteractingKey))
        {
            playerInteractingWithCalendar = false;
            GameManager.instance.stateOfGame = GameManager.States.InGame;
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
