using TMPro;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
public class VirusInfoSymptoms : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private string defaultText;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI symptomsText;

    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (GameManager.instance.dayNo == 1)
        {
            symptomsText.text = defaultText + "\nArm Rash";
        }
        if (GameManager.instance.dayNo == 2)
        {
            if (GameManager.instance.secondHalfStarted)
            {
                symptomsText.text = defaultText + "\nArm Rash OR Back Acne";
            }
            else
            {
                symptomsText.text = defaultText + "\nArm Rash AND Back Acne";
            }
        }
        if (GameManager.instance.dayNo == 3)
        {
            symptomsText.text = defaultText + "\nArm Rash OR Back Acne OR Chest Discolouration \nAcne and Chest Discolouration NOT considered if both apparent at the same time \n Rapid Weight Loss";
        }
        if (GameManager.instance.dayNo == 4)
        {
            symptomsText.text = defaultText + "\nArm Rash OR Back Acne OR Chest Discolouration \nAcne and Chest Discolouration NOT considered if both apparent at the same time \n Rapid Weight Loss\n Irregular Speech";
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
