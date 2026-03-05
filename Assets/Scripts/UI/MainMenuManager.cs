using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    [Header("UI References")]
    [SerializeField] private GameObject m_mainMenu;
    [SerializeField] private GameObject m_leaderboard;
    [SerializeField] private GameObject m_settings;
    [SerializeField] private GameObject m_levelSelect;

    [Header("Stage Buttons")]
    [SerializeField] private Button m_stage1Button;
    [SerializeField] private Button m_stage2Button;
    [SerializeField] private Button m_stage3Button;
    [SerializeField] private Button m_stage4Button;

    [Header("Menu Music")]
    [SerializeField] private AudioClip m_menuMusic;

    private void Start()
    {
        backToMainMenu();

        MusicManager.Instance.playSong( m_menuMusic );
    }

    // Button functions
    public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void openLeaderboard()
    {
        m_mainMenu.SetActive( false );
        m_leaderboard.SetActive( true );
    }

    public void openSettings()
    {
        m_mainMenu.SetActive( false );
        m_settings.SetActive( true );
    }

    public void openLevelSelect()
    {
        m_mainMenu.SetActive( false );
        m_levelSelect.SetActive( true );

        // Set Button status based on progress
        m_stage1Button.interactable = true;
        m_stage2Button.interactable = false;
        m_stage3Button.interactable = false;
        m_stage4Button.interactable = false;
    }

    public void backToMainMenu()
    {
        m_mainMenu.SetActive( true );
        m_leaderboard.SetActive( false );
        m_settings.SetActive( false );
        m_levelSelect.SetActive( false );
    }

    public void quitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
