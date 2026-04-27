using TMPro;
using UnityEngine;

public class IrregularSpeechTestDocument : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI thingsCheckedForTitles;
    [SerializeField] private TextMeshProUGUI testResults;
    [SerializeField] private GameObject childText;

    //////////////////////////////////////////////////////////////////////////////
    public void UpdateDocumentContents()
    {
        EncounterSO encounter = GetComponent<GameplayDocument>().respectiveEncounter;
        if (encounter.age < 13)
        {
            childText.SetActive(true);
            thingsCheckedForTitles.text = "Tourette syndrome\n \nHereditary Stutter";
            testResults.text = "Not Found\n\nNot Found";
        }
        else
        {
            childText.SetActive(false);

            thingsCheckedForTitles.text = "Alcohol Intoxication\n \nTourette syndrome\n \nHereditary Stutter";

            if (encounter.alcoholIntoxication)
            {
                testResults.text = "Present\n\nNot Found\n\nNot Found";
            }
            else
            {
                testResults.text = "Not Found\n\nNot Found\n\nNot Found";
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}
