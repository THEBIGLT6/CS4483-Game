using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrapsAudio
{
    public class AudioSpikeTrap : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip audioClipEmerge1;
        public AudioClip audioClipEmerge2;
        public AudioClip audioClipLoopingAttack1;
        public AudioClip audioClipLoopingAttack2;
        public AudioClip audioClipRetract;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayAudioEmerge1()
        {
            audioSource.PlayOneShot(audioClipEmerge1);
        }
        public void PlayAudioEmerge2()
        {
            audioSource.PlayOneShot(audioClipEmerge2);
        }
        public void PlayAudioLoopingAttack1()
        {
            audioSource.PlayOneShot(audioClipLoopingAttack1);
        }
        public void PlayAudioLoopingAttack2()
        {
            audioSource.PlayOneShot(audioClipLoopingAttack2);
        }
        public void PlayAudioRetract()
        {
            audioSource.PlayOneShot(audioClipRetract);
        }
    }
}
