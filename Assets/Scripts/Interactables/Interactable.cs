using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject m_interactionCanvas;
    [SerializeField] private UIManager m_uiManager;
    private bool m_playerInside = false;

    private void Start()
    {
        m_interactionCanvas.SetActive(false);
    }

    private void Update()
    {
        if (m_playerInside && Input.GetKeyDown(KeyCode.F))
        {
            m_uiManager.openInteractionCanvas(true, m_interactionCanvas);
            m_uiManager.openIntreractionPrompt(false);
        }

        if (m_playerInside && !m_uiManager.isPaused() && !m_interactionCanvas.activeInHierarchy )
        {
            m_uiManager.openIntreractionPrompt(true);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_playerInside = true;
            m_uiManager.openIntreractionPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_playerInside = false;
            m_uiManager.openIntreractionPrompt(false);
        }
    }

    public void closeInteractionCanvas()
    {
        m_uiManager.openInteractionCanvas(false, m_interactionCanvas);
    }

}
