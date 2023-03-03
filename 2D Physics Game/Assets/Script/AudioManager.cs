using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        //test
        public static AudioManager instance;

        [SerializeField] private AudioSource _bgAudioSource;
        [SerializeField] private AudioSource _effectAudioSource;

        [SerializeField] private AudioClip _bgClip;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            PlayBG();
        }

        public void PlayBG()
        {
            _bgAudioSource.clip = _bgClip;
            _bgAudioSource.Play();
            
        }

        public void PlayEffect(AudioClip clip,float volume)
        {
            _effectAudioSource.clip =  clip;
            _effectAudioSource.volume = volume;
            _effectAudioSource.Play();
        }
        public void PlayEffect(AudioClip clip,float pitch,float volume)
        {
            _effectAudioSource.clip = clip;
            _effectAudioSource.pitch = pitch;
            _effectAudioSource.volume = volume;
            _effectAudioSource.Play();
        }
    }
}

