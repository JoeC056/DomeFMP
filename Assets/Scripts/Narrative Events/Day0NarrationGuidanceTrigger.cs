using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

//////////////////////////////////////////////////////////////////////////////////
public class Day0NarrationGuidanceTrigger : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private string dialogueText;
    [SerializeField] private float lifetime;

    //////////////////////////////////////////////////////////////////////////////////
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            List<string> message = new List<string>();
            message.Add(dialogueText);
            Subtitles.instance.DisplaySubtitles(message, lifetime);
            Destroy(gameObject);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
