using System.Collections;
using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class EndingSequence : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI textSpace;
    [SerializeField] private GameObject mainMenuButton;

    [Header("Parameters")]
     private string contents;
    [SerializeField] private float delayBetweenCharacters;

    private bool waiting;

    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        contents = textSpace.text;
        mainMenuButton.SetActive(false);
        textSpace.text = "";
    }

    //////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        Time.timeScale = 1;

        if (contents == "")
        {
            mainMenuButton.SetActive(true);
        }
        else if (!waiting)
        {
            textSpace.text += contents[0];
            contents = contents.Substring(1);
            StartCoroutine(Wait());
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    private IEnumerator Wait()
    {
        waiting = true;
        Debug.Log(delayBetweenCharacters);
        yield return new WaitForSeconds(delayBetweenCharacters);
        waiting = false;
    }

    //////////////////////////////////////////////////////////////////////////////
}
