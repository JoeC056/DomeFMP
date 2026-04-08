using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class VirusTestResultsDocument : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI testResultsDetails;

    //////////////////////////////////////////////////////////////////////////////
    public void UpdateDocumentContents()
    {
        EncounterSO encounter = GetComponent<GameplayDocument>().respectiveEncounter;

        testResultsDetails.text = encounter.virusTestResultsShowVirus + "\n\n" + encounter.bloodTestResultsShowVirus + "\nType: " + encounter.bloodType;
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
