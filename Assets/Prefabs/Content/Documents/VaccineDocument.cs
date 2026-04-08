using TMPro;
using UnityEngine;

public class VaccineDocument : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI vaccinesAdministeredDetails;
    [SerializeField] private TextMeshProUGUI dosageDetails;

    //////////////////////////////////////////////////////////////////////////////
    public void UpdateDocumentContents()
    {
        EncounterSO encounter = GetComponent<GameplayDocument>().respectiveEncounter;
        int vaccinesReceived = 0;
        vaccinesAdministeredDetails.text = "";
        dosageDetails.text = "";

        if (encounter.receivedVaccine1)
        {
            vaccinesAdministeredDetails.text += "Vaccine_001\n\n";
            vaccinesReceived++;
        }
        if (encounter.receivedVaccine2)
        {
            vaccinesAdministeredDetails.text += "Vaccine_002\n\n";
            vaccinesReceived++;
        }
        if (encounter.receivedVaccine3)
        {
            vaccinesAdministeredDetails.text += "Vaccine_003\n\n";
            vaccinesReceived++;
        }
        for (int i = 0; i < vaccinesReceived; i++) 
        {
            dosageDetails.text += "10mg\n\n";
        }

    }

    //////////////////////////////////////////////////////////////////////////////
}
