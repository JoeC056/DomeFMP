using TMPro;
using UnityEngine;
using UnityEngine.Events;

//////////////////////////////////////////////////////////////////////////////
public class GameplayDocument : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI detailsText;

    [Header("Parameters")]
    [SerializeField] private UnityEvent documentSpecificEventOnInstantiate;

    [HideInInspector] public EncounterSO respectiveEncounter;


    //////////////////////////////////////////////////////////////////////////////
    public void UpdateTextForEncounterData(EncounterSO encounter)
    {
        respectiveEncounter = encounter;

        nameText.text = respectiveEncounter.encounterName;
        detailsText.text = respectiveEncounter.dob + " (" + respectiveEncounter.age + "yo.)\n" + respectiveEncounter.sex;

        documentSpecificEventOnInstantiate.Invoke();

    }
    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
