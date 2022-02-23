using System;
using Tools.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Services
{
    public class PlayAudioOnSpawn : MonoBehaviour, IPoolInit, IPoolOnSpawn, IPoolOnDespawn
    {
        [SerializeField] [HideInInspector]private AudioSource audioSource;
        [SerializeField] [Range(0.01f, 10f)]private float pitchRandomMultiplier = 1f;
    
        private float _originalPitch;
        private PoolService _poolService;

        public void Init()
        {
            audioSource.Stop();
        }

        public void OnSpawn(PoolService poolService)
        {
            _poolService = poolService;
            _originalPitch = audioSource.pitch;
        
            //Multiply pitch
            if (Math.Abs(pitchRandomMultiplier - 1) > 0.001f)
            {
                if (Random.value < .5)
                    audioSource.pitch *= Random.Range(1 / pitchRandomMultiplier, 1);
                else
                    audioSource.pitch *= Random.Range(1, pitchRandomMultiplier);
            }
        }
    
        public void Play(AudioClip clip, float volume = 1)
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
        
            float audioLife = audioSource.clip.length / audioSource.pitch;
            _poolService.Despawn(gameObject, audioLife);
        }
    
        public void OnDespawn()
        {
            audioSource.Stop();
            audioSource.pitch = _originalPitch;
        }
    
#if UNITY_EDITOR
        public void Reset()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.playOnAwake = false;
        }
#endif
    }
}