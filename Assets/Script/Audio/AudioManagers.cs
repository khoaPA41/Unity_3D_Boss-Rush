using System.Collections;
using Script.Design_Pattern.Object_Pooling;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManagers : MonoBehaviour
{
    public static AudioManagers Instance;

    [Header("Object Pooling")]
    [SerializeField] private ObjectPooling _objectPooling;
    
    [Header("Background Music")]
    [SerializeField] private AudioSource bossFight_I;
    [SerializeField] private AudioSource bossFight_II;

    [Header("SFX For Boss Skill")]
    [field:SerializeField] public AudioResource throwSwordResource;
    [field:SerializeField] public AudioResource firstAOEResource;
    [field:SerializeField] public AudioResource transformToTwoSwordResource;
    [field:SerializeField] public AudioResource transformToBladeResource;
    [field:SerializeField] public AudioResource fireBulletResource;
    [field:SerializeField] public AudioResource fireExplosionResource;
    [field:SerializeField] public AudioResource waveResource;
    [field:SerializeField] public AudioResource attractiveResource;
    [field:SerializeField] public AudioResource slowMotionAttackResource;

    [Header("SFX For Player Skill")]
    [field:SerializeField] public AudioResource inescapableResource;
    [field:SerializeField] public AudioResource indestructibleResource;
    [field:SerializeField] public AudioResource invisibleResource;
    [field:SerializeField] public AudioResource worldBreakerResource;
    [field:SerializeField] public AudioResource phantomRetreatResource;
    [field:SerializeField] public AudioResource phantomMirageResource;
    
    [Header("UI Sound")]
    [SerializeField] private AudioSource uiAudioSource;
    [field:SerializeField] public AudioResource buttonSoundClick_1;
    [field:SerializeField] public AudioResource buttonSoundClick_2;
    [field:SerializeField] public AudioResource buttonSoundClick_3;
    [field:SerializeField] public AudioResource buttonSoundClick_4;
    [field:SerializeField] public AudioResource buttonSoundHold_1;
    [field:SerializeField] public AudioResource buttonSoundHold_2;
    [field:SerializeField] public AudioResource buttonSoundHold_3;
    [field:SerializeField] public AudioResource buttonSoundHold_4;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    public void PlayerBackgroundMusic(bool isPhaseI)
    {
        if (isPhaseI)
        {
            bossFight_I.Play();
            return;
        }
        bossFight_I.Stop();
        bossFight_II.Play();
    }
    
    public void PlaySound(Transform pos, AudioResource resource)
    {
        var audioPool = _objectPooling.GetPooledObject("Audio", pos.position);
        var audioSource = audioPool.GetComponent<AudioSource>();
        audioSource.resource = resource;
        audioSource.Play();
        StartCoroutine(WaitToReturnSound(audioPool, audioSource));
    }

    private IEnumerator WaitToReturnSound(PooledObject pooledObject, AudioSource audioSource)
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        _objectPooling.ReturnToPool("Audio", pooledObject);
    }

    public void PlayUISound(AudioResource resource)
    {
        uiAudioSource.Stop();
        uiAudioSource.resource = resource;
        uiAudioSource.Play();
    }
    
    public void PlayUIHoldSound(AudioResource resource)
    {
        uiAudioSource.Stop();
        uiAudioSource.resource = resource;
        uiAudioSource.Play();
    }

}
