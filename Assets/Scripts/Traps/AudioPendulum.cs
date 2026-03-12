using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrapsAudio
{
    public class AudioPendulum : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip[] audioClipHinges;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public void PlayAudioHinges()
        {
            audioSource.PlayOneShot(audioClipHinges[Random.Range(0, audioClipHinges.Length)]);
        }
    }
}
