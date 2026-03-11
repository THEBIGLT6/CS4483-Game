using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Transform m_hingePoint;
    private const float OPEN_ANGLE = 90;
    private const float OPEN_SPEED = 120;

    public int m_doorDirection = 1;          // determines which direction the doors open 

    private float m_currentAngle = 0f;
    private int m_direction = 0;        // 1 = opening, -1 = closing, 0 = idle


    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("HingePoint"))
            {
                m_hingePoint = child;
                break;
            }
        }
    }

    void Update()
    {
        handleDoor();
    }

    private void handleDoor()
    {
        if (m_direction == 0) return;

        float step = OPEN_SPEED * Time.deltaTime * m_direction;

        transform.RotateAround(m_hingePoint.position, Vector3.up * m_doorDirection, step );
        m_currentAngle += step;

        // Stop at limits
        if (m_currentAngle >= OPEN_ANGLE)
        {
            m_direction = 0;
            m_currentAngle = OPEN_ANGLE;
        }
        else if (m_currentAngle <= 0)
        {
            m_direction = 0;
            m_currentAngle = 0;
        }
    }

    public void OpenDoor()
    {
        m_direction = 1;
    }

    public void CloseDoor()
    {
        m_direction = -1;
    }
}
