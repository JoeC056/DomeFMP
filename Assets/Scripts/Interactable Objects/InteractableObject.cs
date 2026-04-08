using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////////
public class InteractableObject : MonoBehaviour
{
    [Header("Parameters")]
    public UnityEvent EventOnInteract;
    public string textOnInteractionHover;
    [HideInInspector] public bool interactable = true;


    //////////////////////////////////////////////////////////////////////////////////
    public string GetTextOnInteractionHover()
    {
        if (interactable)
        {
            return textOnInteractionHover;  
        }
        else
        {
            return "";
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void OnInteract()
    {
        EventOnInteract.Invoke();
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
