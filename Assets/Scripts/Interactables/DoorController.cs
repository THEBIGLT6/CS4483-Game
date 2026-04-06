using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class DoorController : MonoBehaviour
{
    [SerializeField] private AudioClip m_doorOpen;
    [SerializeField] private AudioClip m_doorClose;
    [SerializeField] private int m_stageNum;         // The stage the player is entering
    private Door[] m_doors;
    private Light m_light;

    private bool m_isOpen;

    void Start()
    {
        m_isOpen = false;

        m_light = GetComponentInChildren<Light>();
        m_light.enabled = false;

        m_doors = GetComponentsInChildren<Door>();
    }

    public void triggerDoorOpen()
    {
        for( int i = 0; i < m_doors.Length; i++ )
        {
            m_doors[i].OpenDoor();
        }

        m_isOpen = true;
        MusicManager.Instance.playOneShot( m_doorOpen );
        m_light.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && m_isOpen)
        {
            for (int i = 0; i < m_doors.Length; i++)
            {
                m_doors[i].CloseDoor();
            }

            GameManager.Instance.setStage( m_stageNum );
            MusicManager.Instance.playOneShot(m_doorClose);
            TrapsUI.Instance.clearTraps();
            ZombieSpawner.Instance.prepareRound();

            m_light.enabled = false;
            m_isOpen = false;
        }
    }
}
