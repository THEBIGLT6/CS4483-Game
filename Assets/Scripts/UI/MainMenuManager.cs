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

    [Header("Leaderboard")]
    [SerializeField] private TextMeshProUGUI[] m_leaderboardNames;
    [SerializeField] private TextMeshProUGUI[] m_leaderboardScores;
    private List<int> m_scores;
    private List<string> m_players;

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
        loadScores();
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

    public void resetScores()
    {
        PlayerPrefs.SetFloat("Score1", 0);
        PlayerPrefs.SetFloat("Score2", 0);
        PlayerPrefs.SetFloat("Score3", 0);
        PlayerPrefs.SetFloat("Score4", 0);
        PlayerPrefs.SetFloat("Score5", 0);
        PlayerPrefs.SetString("Player1", "");
        PlayerPrefs.SetString("Player2", "");
        PlayerPrefs.SetString("Player3", "");
        PlayerPrefs.SetString("Player4", "");
        PlayerPrefs.SetString("Player5", "");
        loadScores();
    }

    private void loadScores()
    {
        m_scores = new List<int>();
        m_scores.Add(PlayerPrefs.GetInt("Score1"));
        m_scores.Add(PlayerPrefs.GetInt("Score2"));
        m_scores.Add(PlayerPrefs.GetInt("Score3"));
        m_scores.Add(PlayerPrefs.GetInt("Score4"));
        m_scores.Add(PlayerPrefs.GetInt("Score5"));

        m_players = new List<string>();
        m_players.Add(PlayerPrefs.GetString("Player1"));
        m_players.Add(PlayerPrefs.GetString("Player2"));
        m_players.Add(PlayerPrefs.GetString("Player3"));
        m_players.Add(PlayerPrefs.GetString("Player4"));
        m_players.Add(PlayerPrefs.GetString("Player5"));

        for (int i = 0; i < 5; i++)
        {
            if (m_players[i] != "") m_leaderboardNames[i].text = (i + 1) + ".   " + m_players[i];
            else m_leaderboardNames[i].text = (i + 1) + ".   " + "---";

            if (m_scores[i] != 0) m_leaderboardScores[i].text = m_scores[i].ToString();
            else m_leaderboardScores[i].text = "--";
        }

    }
}
