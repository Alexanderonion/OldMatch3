using UnityEngine;
using UnityEngine.XR;

public class SoundManager : MonoBehaviour
{
    public static AudioSource[] audioSources;
    private AudioSource backAudioSource;
    private AudioSource destroyAudioSource;
    public static AudioClip[] deleteObjectSound;
    [SerializeField] private AudioClip deleteTokenSound;
    [SerializeField] private AudioClip deleteBonusRocketSound;
    [SerializeField] private AudioClip deleteBonusBombSound;
    
    private void Start()
    {
        backAudioSource = gameObject.GetComponent<AudioSource>();
        destroyAudioSource = gameObject.AddComponent<AudioSource>();
        deleteObjectSound = new AudioClip[3] {deleteTokenSound, deleteBonusRocketSound, deleteBonusBombSound };
        audioSources = new AudioSource[2] { backAudioSource, destroyAudioSource };
    }
}