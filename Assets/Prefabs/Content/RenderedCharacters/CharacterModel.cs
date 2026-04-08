using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class CharacterModel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject leftArmRash;
    [SerializeField] private GameObject rightArmRash;
    [SerializeField] private GameObject backAcne;
    [SerializeField] private GameObject chestDiscolouration;
    [SerializeField] private GameObject patterenedChestDiscolouration;


    //////////////////////////////////////////////////////////////////////////////
    public void AssignVisibleSymptoms(EncounterSO encounterToGatherDataFrom)
    {
        if (encounterToGatherDataFrom.rash)
        {
            int no = Random.Range(0, 3);
            if (no == 0)
            {
                leftArmRash.SetActive(true); 
                rightArmRash.SetActive(false);
            }
            if (no == 1)
            {
                leftArmRash.SetActive(false);
                rightArmRash.SetActive(true);
            }
            if (no == 2)
            {
                leftArmRash.SetActive(true);
                rightArmRash.SetActive(true);
            }
        }
        else
        {
            leftArmRash.SetActive(false);
            rightArmRash.SetActive(false);
        }

        backAcne.SetActive(encounterToGatherDataFrom.backAcne);
        chestDiscolouration.SetActive(encounterToGatherDataFrom.chestDiscolouration);
        patterenedChestDiscolouration.SetActive(encounterToGatherDataFrom.patterenedChestDiscolouration);
    }
}

//////////////////////////////////////////////////////////////////////////////

