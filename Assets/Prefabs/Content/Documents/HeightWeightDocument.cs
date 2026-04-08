using TMPro;
using UnityEngine;

public class HeightWeightDocument : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI testTimesDetails;
    [SerializeField] private TextMeshProUGUI testResultsDetails;

    //////////////////////////////////////////////////////////////////////////////
    public void UpdateDocumentContents()
    {
        EncounterSO encounter = GetComponent<GameplayDocument>().respectiveEncounter;

        testTimesDetails.text = encounter.timeHeightChecked + "\n\n" + encounter.timeWeightChecked;
        testResultsDetails.text = encounter.height + "\n\n" + encounter.weight;
    }

    //////////////////////////////////////////////////////////////////////////////
}
