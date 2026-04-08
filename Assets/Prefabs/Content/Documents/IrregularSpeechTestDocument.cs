using TMPro;
using UnityEngine;

public class IrregularSpeechTestDocument : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI thingsCheckedForTitles;
    [SerializeField] private TextMeshProUGUI testPerformedDetails;
    [SerializeField] private GameObject childText;

    //////////////////////////////////////////////////////////////////////////////
    public void UpdateDocumentContents()
    {
        EncounterSO encounter = GetComponent<GameplayDocument>().respectiveEncounter;
        if (encounter.age < 13)
        {
            childText.SetActive(true);
            thingsCheckedForTitles.text = "Tourette syndrome\n \nHereditary Stutter";
            testPerformedDetails.text = "Not Found\n\nNotFound";
        }
        else
        {
            childText.SetActive(false);

            if (encounter.alcoholIntoxication)
            {
                testPerformedDetails.text = "Present\n\nNotFound\n\nNotFound";
            }
            else
            {
                testPerformedDetails.text = "Not Found\n\nNotFound\n\nNotFound";
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////
}
