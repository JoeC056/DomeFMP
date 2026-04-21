using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

//////////////////////////////////////////////////////////////////////////////
public class CursorManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ComputerInteractable computerInteractable;
    [SerializeField] private Canvas computerCanvas;

    [Header("Prefabs")]
    [SerializeField] private Texture2D stylizedCursor;
    [SerializeField] private Texture2D stylizedHandCursor;

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (computerInteractable.playerUsingComputer && SettingsManager.instance.stylizedCursorEnabled)
        {
            if (GetMouseHoveringOverButton())
            {
                Cursor.SetCursor(stylizedHandCursor, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(stylizedCursor, Vector2.zero, CursorMode.Auto);
            }
        
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    private bool GetMouseHoveringOverButton()
    {
        PointerEventData currentMousePosition = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> listOfCurrentHover = new List<RaycastResult>();


        computerCanvas.GetComponent<GraphicRaycaster>().Raycast(currentMousePosition, listOfCurrentHover);


        foreach (var thingHoveredOver in listOfCurrentHover)
        {
            if (thingHoveredOver.gameObject.GetComponent<Button>() != null)
            {
                if (thingHoveredOver.gameObject.GetComponent<Button>().enabled)
                {
                    return true;
                }
            }
        }

        return false;
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
