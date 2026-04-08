using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class Subtitles : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI subtitles;

    //Instance of subtitles
    public static Subtitles instance;

    private List<string> subtitlesToDisplay;
    private float delay;
    private bool waiting;

    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        subtitles.text = "";
        waiting = false;

        subtitlesToDisplay = new List<string>();

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

    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (subtitlesToDisplay.Count > 0)
        {
            if (!waiting)
            {
                StartCoroutine(UpdateSubtitles(subtitlesToDisplay[0]));
                subtitlesToDisplay.RemoveAt(0);
            }
        }
        else if (!waiting)
        {
            subtitles.text = "";
        }
    }
    //////////////////////////////////////////////////////////////////////////////////
    public void DisplaySubtitles(List<string> contentToDisplay, float delayBetweenText)
    {
        subtitlesToDisplay = contentToDisplay;
        delay = delayBetweenText;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private IEnumerator UpdateSubtitles(string contents)
    {
        waiting = true;
        subtitles.text = contents;
        yield return new WaitForSeconds(delay);
        waiting = false;
    }

}

//////////////////////////////////////////////////////////////////////////////////
