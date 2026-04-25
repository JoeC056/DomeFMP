using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class EndingChoiceSelectUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject ending1UI;
    [SerializeField] private GameObject ending2UI;


    //////////////////////////////////////////////////////////////////////////////
    public void ChooseEnding(int endingIndex)
    {
        if (endingIndex == 1)
        {
            ending1UI.SetActive(true);
            //GameManager.instance.stateOfGame = GameManager.States.WatchingEndingSequence; 
            this.gameObject.SetActive(false);
        }
        if (endingIndex == 2)
        {
            ending2UI.SetActive(true);
            //GameManager.instance.stateOfGame = GameManager.States.WatchingEndingSequence;
            this.gameObject.SetActive(false);
        }

    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
