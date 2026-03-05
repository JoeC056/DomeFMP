using UnityEngine;

//////////////////////////////////////////////////////////////////////////////
public class CharacterRender : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject characterRender;

    [Header("Parameters")]
    [SerializeField] private GameObject characterToRender;

    //////////////////////////////////////////////////////////////////////////////
    public void UpdateCharacterRender(GameObject newCharacterRender)
    {
        characterToRender = newCharacterRender;
        characterRender = Instantiate(characterRender, transform.position, Quaternion.Euler(Vector3.zero), transform);
    }

    //////////////////////////////////////////////////////////////////////////////
    public void RotateRender(bool vertical, float amountToRotateBy)
    {

    }
}

//////////////////////////////////////////////////////////////////////////////
