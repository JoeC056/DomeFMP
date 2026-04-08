using System.Collections;
using UnityEngine;
using UnityEngine.Events;

//////////////////////////////////////////////////////////////////////////////////
public class SirenEvent : MonoBehaviour
{
    [Header("Parameters")]
    public UnityEvent EventOnCollision;
    [SerializeField] private float textDuration;

    [Header("References")]
    public GameObject sirenSoundText;

    private bool interactedWith;

    //////////////////////////////////////////////////////////////////////////////////
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !interactedWith)
        {
            interactedWith = true;
            StartCoroutine(PlaySirenSound());
            EventOnCollision.Invoke();
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    private IEnumerator PlaySirenSound()
    {
        sirenSoundText.SetActive(true);
        yield return new WaitForSeconds(textDuration);
        sirenSoundText.SetActive(false);
        Destroy(gameObject);
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
