using Unity.VisualScripting;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class CharacterRender : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject rotationButtons;
    [SerializeField] private GameObject characterRenderCamera;

    [Header("Parameters")]
    [SerializeField] private Vector3 renderedCharacterDisplacementFromCamera;
    [SerializeField] private Vector3 renderedCharacterDefaultRot;

    //Stores currently rendered character
    private GameObject renderedCharacter;

    //Constraints for rotation of rendered character
    private float rotationClampUpperBound;
    private float rotationClampLowerBound;


    //////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        CheckToDisplayRotationButtons();
    }

    //////////////////////////////////////////////////////////////////////////////
    public void AddNewCharacterRender(GameObject newCharacterRender, float clampLowerBound, float clampUpperBound, EncounterSO respectiveEncounter)
    {
        //Instantiates new character render 
        renderedCharacter = Instantiate(newCharacterRender, characterRenderCamera.transform.position + renderedCharacterDisplacementFromCamera, Quaternion.Euler(renderedCharacterDefaultRot), characterRenderCamera.transform);
        renderedCharacter.GetComponent<CharacterModel>().AssignVisibleSymptoms(respectiveEncounter);
        rotationClampUpperBound = clampUpperBound;
        rotationClampLowerBound = clampLowerBound;
        CheckToDisplayRotationButtons();
    }

    //////////////////////////////////////////////////////////////////////////////
    public void RemoveCurrentlyRenderedCharacter()
    {
        //Removes current character render and updates variable accordingly 
        Destroy(renderedCharacter);
        renderedCharacter = null;
        CheckToDisplayRotationButtons();
    }

    //////////////////////////////////////////////////////////////////////////////
    public void RotateRender(float amountToRotateBy)
    {
        renderedCharacter.transform.Rotate(0, amountToRotateBy, 0);
        ////Updates rotation 
    }

    //////////////////////////////////////////////////////////////////////////////
    private void CheckToDisplayRotationButtons()
    {
        rotationButtons.SetActive(renderedCharacter != null);
    }

    //////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////
