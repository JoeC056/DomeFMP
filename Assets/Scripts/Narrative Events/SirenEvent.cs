using System.Collections;
using UnityEngine;
using UnityEngine.Events;

//////////////////////////////////////////////////////////////////////////////////
public class SirenEvent : MonoBehaviour
{
    [Header("Parameters")]
    public UnityEvent EventOnCollision;
    [SerializeField] private float soundDuration;
    [SerializeField] private float soundStartTime;
    [SerializeField] private float fadeOutSpeed;

    [Header("References")]
    [SerializeField] private AudioClip sirenSound;

    private bool interactedWith;

    private AudioSource audioSource;

    private bool fadingOut;
    private float defaultVolume;

    //////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
        defaultVolume = audioSource.volume;
    }

    //////////////////////////////////////////////////////////////////////////////////
    private void Update()
    {
        if (!fadingOut)
        {
            audioSource.volume = defaultVolume * SettingsManager.instance.MasterVolume * SettingsManager.instance.GameVolume;
        }
    }
    //////////////////////////////////////////////////////////////////////////////////
    private void FixedUpdate()
    {
        if (fadingOut)
        {
            audioSource.volume -= fadeOutSpeed;
            if (audioSource.volume <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

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
        audioSource.clip = sirenSound;
        audioSource.time = soundStartTime;
        audioSource.Play();
        yield return new WaitForSeconds(soundDuration);
        fadingOut = true;
    }

    //////////////////////////////////////////////////////////////////////////////////
}

//////////////////////////////////////////////////////////////////////////////////
