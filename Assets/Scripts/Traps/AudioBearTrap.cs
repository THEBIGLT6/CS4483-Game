using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TrapsAudio
{
    public class AudioBearTrap : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip audioClipArming;
        public AudioClip audioClipTrigger;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public void PlayAudioArming()
        {
            audioSource.PlayOneShot(audioClipArming);
        }
        public void PlayAudioTrigger()
        {
            audioSource.PlayOneShot(audioClipTrigger);
        }
    }
}
