using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class Subtitles : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI subtitles;

    [Header("Parameters")]
    [SerializeField] private float delayBetweenCharacters;

    //Instance of subtitles
    public static Subtitles instance;

    private List<string> subtitlesToDisplay;
    private float delay;
    public bool waiting;

    private string remainingSubtitleContents;

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
        if (GameManager.instance.stateOfGame != GameManager.States.InDaysStartAnimation)
        {
            if (remainingSubtitleContents != "")
            {
                if (!waiting)
                {
                    subtitles.text += remainingSubtitleContents[0];
                    remainingSubtitleContents = remainingSubtitleContents.Substring(1);

                    CheckHowLongToWait();
                }
            }
            else if (subtitlesToDisplay.Count > 0)
            {
                if (!waiting)
                {
                    subtitlesToDisplay.RemoveAt(0);

                    subtitles.text = "";

                    if (subtitlesToDisplay.Count > 0)
                    {
                        remainingSubtitleContents = subtitlesToDisplay[0];
                    }
                }
            }
            else if (!waiting)
            {
                subtitles.text = "";
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void DisplaySubtitles(List<string> contentToDisplay, float delayBetweenText)
    {
        subtitlesToDisplay.AddRange(contentToDisplay);
        delay = delayBetweenText;
        remainingSubtitleContents = subtitlesToDisplay[0];
        waiting = false;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private IEnumerator Wait(float duration)
    {
        waiting = true;
        yield return new WaitForSeconds(duration);
        waiting = false;
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void ClearSubtitles()
    {
        StopAllCoroutines();
        subtitles.text = "";
        subtitlesToDisplay = new List<string>();
        remainingSubtitleContents = "";
    }

    //////////////////////////////////////////////////////////////////////////////////
    public void CheckHowLongToWait()
    {
        if (remainingSubtitleContents == "")
        {
            StartCoroutine(Wait(delay));
        }
        else
        {
            StartCoroutine(Wait(delayBetweenCharacters));
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
