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
    public void startGame( int selectedStage )
    {
        SceneManager.LoadScene(1);
        GameManager.Instance.setStage( selectedStage );
        GameManager.Instance.resetMultiplier();
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
        m_stage1Button.interactable = false;
        m_stage2Button.interactable = false;
        m_stage3Button.interactable = false;
        m_stage4Button.interactable = false;

        for ( int i = 1; i <= GameManager.Instance.getMaxStageUnlocked(); i++ )
        {
            switch (i)
            {
                case 1:
                    m_stage1Button.interactable = true;
                    break;
                case 2:
                    m_stage2Button.interactable = true;
                    break;
                case 3:
                    m_stage3Button.interactable = true;
                    break;
                case 4:
                    m_stage4Button.interactable = true;
                    break;
            }
        }
        
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
        GameManager.Instance.Save();

        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }
}
