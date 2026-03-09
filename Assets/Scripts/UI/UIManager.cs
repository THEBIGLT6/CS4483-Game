using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Screens")]
    [SerializeField] private GameObject m_pauseUI;
    [SerializeField] private GameObject m_hud;
    [SerializeField] private GameObject m_interactionPrompt;

    [Header("State")]
    private bool m_pausable;
    private bool m_paused;

    void Start()
    {
        m_paused = false;
        m_pausable = true;

        m_hud.SetActive(true);
        m_pauseUI.SetActive(false);
        m_interactionPrompt.SetActive(false);
    }

    void Update()
    {
        if (m_pausable && Input.GetKeyDown(KeyCode.Escape))
        {
            pauseGame(!m_pauseUI.activeInHierarchy);
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && m_pauseUI.activeInHierarchy)
        {
            pauseGame(!m_pauseUI.activeInHierarchy);
        }
    }

    public bool isPaused()
    {
        return m_paused;
    }

    public void pauseGame(bool status)
    {
        m_paused = status;

        MusicManager.Instance.TogglePause(status);
        m_hud.SetActive(!status);
        m_pauseUI.SetActive(status);
        m_interactionPrompt.SetActive(false);
        m_pausable = !status;
        Time.timeScale = status ? 0f : 1f;

        Cursor.lockState = status ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = status ? true : false;
    }

    public void quitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void mainMenu()
    {
        MusicManager.Instance.stopMusic();

        Time.timeScale = 1f;
        m_pausable = true;
        m_hud.SetActive(true);
        m_pauseUI.SetActive(false);

        SceneManager.LoadScene(0);
    }

    public void openInteractionCanvas( bool open, GameObject interactionCanvas )
    {
        m_pausable = !open;
        interactionCanvas.SetActive( open );
        m_hud.SetActive(!open);

        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = open ? true : false;
        Time.timeScale = open ? 0f : 1f;
    }

    public void openIntreractionPrompt( bool open )
    {
        m_interactionPrompt.SetActive( open );
    }

}
