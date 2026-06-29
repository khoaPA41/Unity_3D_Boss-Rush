using System.Collections;
using Script.Design_Pattern.Object_Pooling;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManagers : MonoBehaviour
{
    public static AudioManagers Instance;

    [SerializeField] private ObjectPooling _objectPooling;
    
    [Header("Background Music")]
    [SerializeField] private AudioSource bossFight_I;
    [SerializeField] private AudioSource bossFight_II;

    private void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
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
}
