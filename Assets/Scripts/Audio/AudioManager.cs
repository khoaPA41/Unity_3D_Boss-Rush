using System.Collections;
using DesignPattern.ObjectPooling;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [Header("Object Pooling")]
        [SerializeField] private ObjectPoolManager _objectPooling;

        [Header("Background Music")]
        [SerializeField] private AudioSource _bossFightI;
        [SerializeField] private AudioSource _bossFightII;

        [Header("SFX For Boss Skill")]
        [field: SerializeField] public AudioResource throwSwordResource;
        [field: SerializeField] public AudioResource firstAOEResource;
        [field: SerializeField] public AudioResource transformToTwoSwordResource;
        [field: SerializeField] public AudioResource transformToBladeResource;
        [field: SerializeField] public AudioResource fireBulletResource;
        [field: SerializeField] public AudioResource fireExplosionResource;
        [field: SerializeField] public AudioResource waveResource;
        [field: SerializeField] public AudioResource attractiveResource;
        [field: SerializeField] public AudioResource slowMotionAttackResource;

        [Header("SFX For Player Skill")]
        [field: SerializeField] public AudioResource inescapableResource;
        [field: SerializeField] public AudioResource indestructibleResource;
        [field: SerializeField] public AudioResource invisibleResource;
        [field: SerializeField] public AudioResource worldBreakerResource;
        [field: SerializeField] public AudioResource phantomRetreatResource;
        [field: SerializeField] public AudioResource phantomMirageResource;

        [Header("UI Sound")]
        [SerializeField] private AudioSource _uiAudioSource;
        [field: SerializeField] public AudioResource buttonSoundClick_1;
        [field: SerializeField] public AudioResource buttonSoundClick_2;
        [field: SerializeField] public AudioResource buttonSoundClick_3;
        [field: SerializeField] public AudioResource buttonSoundClick_4;
        [field: SerializeField] public AudioResource buttonSoundHold_1;
        [field: SerializeField] public AudioResource buttonSoundHold_2;
        [field: SerializeField] public AudioResource buttonSoundHold_3;
        [field: SerializeField] public AudioResource buttonSoundHold_4;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void PlayBackgroundMusic(bool isPhaseI)
        {
            if (isPhaseI)
            {
                _bossFightI.Play();
                return;
            }
            _bossFightI.Stop();
            _bossFightII.Play();
        }

        public void StopBackgroundMusic()
        {
            _bossFightI.Stop();
            _bossFightII.Stop();
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
            _uiAudioSource.Stop();
            _uiAudioSource.resource = resource;
            _uiAudioSource.Play();
        }
    }
}
