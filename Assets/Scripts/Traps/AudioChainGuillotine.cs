using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrapsAudio
{
    public class AudioChainGuillotine : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip audioClipChainGuillotineAttack;
        public AudioClip audioClipChainGuillotineLoopingAttack;
        public AudioClip audioClipChainGuillotineRise;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public void PlayChainGuillotineAttack()
        {
            audioSource.PlayOneShot(audioClipChainGuillotineAttack);
        }
        public void PlayChainGuillotineLoopingAttack()
        {
            audioSource.PlayOneShot(audioClipChainGuillotineLoopingAttack);
        }
        public void PlayAudioChainGuillotineRise()
        {
            audioSource.PlayOneShot(audioClipChainGuillotineRise);
        }
    }
}

