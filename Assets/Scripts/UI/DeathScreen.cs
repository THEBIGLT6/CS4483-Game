using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DeathScreen : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text m_moneyText;
    [SerializeField] private TMP_Text m_roundText;
    [SerializeField] private GameObject m_newHighScoreUI;
    [SerializeField] private TMP_InputField m_nameInput;
    [SerializeField] private GameObject m_hud;

    [Header("Scoring")]
    private List<int> m_scores;
    private List<string> m_players;

    private static DeathScreen _instance;
    public static DeathScreen Instance => _instance;

    private void Awake()
    {
        _instance = this;
        gameObject.SetActive(false);
        m_newHighScoreUI.SetActive(false);
        m_nameInput.characterLimit = 3;
    }

    public void Show()
    {
        m_hud.SetActive(false);

        loadScores();
        gameObject.SetActive(true);
        GameManager.Instance.resetMultiplier();

        // Display stats
        int money = GameManager.Instance.getMoney();
        int round = ZombieSpawner.Instance.getCurrentRound();
        m_newHighScoreUI.SetActive( newHighScore(round) );

        m_moneyText.text = "Money Collected: $" + money.ToString("D4");
        m_roundText.text = "Round Reached: " + round;

        // Freeze game
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
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
    }

    public void saveScores()
    {
        string playerName = m_nameInput.text.ToUpper();
        int maxRound = ZombieSpawner.Instance.getCurrentRound();

        for (int i = 0; i < m_scores.Count; i++)
        {
            if (maxRound > m_scores[i])
            {
                m_scores.Insert(i, maxRound);
                m_players.Insert(i, playerName);
                break;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt("Score" + (i + 1), m_scores[i]);
            PlayerPrefs.SetString("Player" + (i + 1), m_players[i]);
        }

        m_newHighScoreUI.SetActive(false);
    }

    private bool newHighScore(int score)
    {
        foreach (float scoreStored in m_scores)
        {
            if (score > scoreStored) return true;
        }

        return false;
    }
}