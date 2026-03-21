using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class DoorController : MonoBehaviour
{
    [SerializeField] private AudioClip m_doorOpen;
    [SerializeField] private AudioClip m_doorClose;
    private Door[] m_doors;
    private Light m_light;

    void Start()
    {
        m_light = GetComponentInChildren<Light>();
        m_light.enabled = false;

        m_doors = GetComponentsInChildren<Door>();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    triggerDoorOpen();
        //}
    }

    public void triggerDoorOpen()
    {
        for( int i = 0; i < m_doors.Length; i++ )
        {
            m_doors[i].OpenDoor();
        }

        MusicManager.Instance.playOneShot( m_doorOpen );
        m_light.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < m_doors.Length; i++)
            {
                m_doors[i].CloseDoor();
            }
        }
        MusicManager.Instance.playOneShot(m_doorClose);
        m_light.enabled = false;
    }
}
