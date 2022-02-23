using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Services
{
    public class SoundEffectService : SerializedMonoBehaviour
    {
        [Serializable]
        public class SoundFxInfo
        {
            [Range(0, 1)] public float volume = 1;
            public List<List<AudioClip>> audioClipsLayers;
        }
        
        [SerializeField] private PoolService _poolService;

        [Header("Audio Clips")] 
        [SerializeField] private Dictionary<AudioClipEnum, SoundFxInfo> _soundFxInfos = default;
        
        [Header("Audio source")]
        [SerializeField] private AudioSource audioSource = default;
        
        private const string AudioSourcePrefab = "SoundEffectPrefab";
        
        
        private void PlayClipAt(AudioClipEnum clipEnum, float3 pos)
        {
            if (clipEnum == AudioClipEnum.None) return;
            
            var clips = GetRandomClips(clipEnum);
            if (clips.Count == 0) return;
            
            foreach (var audioClip in clips)
            {
                var playAudioOnSpawn = _poolService.Spawn<PlayAudioOnSpawn>(AudioSourcePrefab, pos, Quaternion.identity);
                playAudioOnSpawn.Play(audioClip, _soundFxInfos[clipEnum].volume);
            }
        }


        public void PlayClipOneShot(AudioClipEnum clipEnum)
        {
            if (clipEnum == AudioClipEnum.None) return;
            var clips = GetRandomClips(clipEnum);

            if (clips.Count == 0) return;
            
            if (clips.Count > 25)
            {
                Debug.LogWarning(clips.Count + " too many audio clips");
            }
            
            audioSource.volume = _soundFxInfos[clipEnum].volume;
            foreach (var audioClip in clips)
            {
                audioSource.PlayOneShot(audioClip);   
            }
        }

        // public void PlayClipInLoop(AudioClipEnum clipEnum)
        // {
        //     var clips = GetRandomClips(clipEnum);
        //     if (clips.Count == 0) return;
        //     
        //     if (continuousAudioSource != null && continuousAudioSource.clip != clips)
        //     {
        //         continuousAudioSource.clip = clips;
        //         continuousAudioSource.Play();
        //     }
        // }
        //
        // public void StopClipInLoop(AudioClipEnum clipEnum)
        // {
        //     var clips = GetRandomClips(clipEnum);
        //
        //     if (clips.Count == 0) return;
        //
        //     if (continuousAudioSource != null && continuousAudioSource.clip == clips)
        //     {
        //         continuousAudioSource.Stop();
        //         continuousAudioSource.clip = null;
        //     }
        // }

        private List<AudioClip> GetRandomClips(AudioClipEnum clipEnum)
        {
            if (_soundFxInfos.ContainsKey(clipEnum) == false)
            {
                Debug.LogError($"AudioService: The Generic Clip for {clipEnum.ToString()} cannot be found in the clip map");
                return null;
            }

            var randomClips = new List<AudioClip>();
            
            var soundFxInfo = _soundFxInfos[clipEnum];

            foreach (var audioClipsLayer in soundFxInfo.audioClipsLayers)
            {
                randomClips.Add(audioClipsLayer[Random.Range(0, audioClipsLayer.Count)]);
            }
            
            return randomClips;
        }
    }
}



