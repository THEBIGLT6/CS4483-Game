using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Singleton pattern implementation
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    [Header("Stages")]
    private int m_maxStageUnlocked;
    private int m_currentStage;        // the current stage the player is on when loaded in

    [Header("Money System")]
    private int m_money;

    [Header("Game progress / Saving")]
    public GameProgress m_gameProgress = new GameProgress();
    string m_savePath;

    [Header("HUD")]
    [SerializeField] private TMP_Text m_moneyText;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);


        // should be - "C:/Users/*User*/AppData/LocalLow/LiamMayaMatt/CS4483/zombieGameSave.json"
        m_savePath = Application.persistentDataPath + "/zombieGameSave.json";
        Load();
    }

    // Saving and loading data
    public void Save()
    {
        m_gameProgress.money = m_money;
        m_gameProgress.maxStage = m_maxStageUnlocked;

        string json = JsonUtility.ToJson(m_gameProgress, true);
        File.WriteAllText(m_savePath, json);
    }

    public void Load()
    {
        if (File.Exists(m_savePath))
        {
            string json = File.ReadAllText(m_savePath);
            m_gameProgress = JsonUtility.FromJson<GameProgress>(json);
        }

        m_maxStageUnlocked = m_gameProgress.maxStage;
        m_money = m_gameProgress.money;
    }

    // PUBLIC FUNCTIONS
    public void addMoney( int amount )
    {
        m_money += amount;
        m_moneyText.text = $"$ {m_money.ToString("D4")}";
    }

    public void subtractMoney( int amount )
    {
        m_money -= amount;
        m_moneyText.text = $"$ {m_money.ToString("D4")}";
    }

    public void setMoney( int amount )
    {
        m_money = amount;
        m_moneyText.text = $"$ {m_money.ToString("D4")}";
    }

    public int getMoney()
    {
        return m_money;
    }

    public void setStage( int stage )
    {
        m_currentStage = stage;
        if( stage >= m_maxStageUnlocked )
        {
            m_maxStageUnlocked = stage;
        }
    }

    public int getCurrentStage()
    {
        return m_currentStage;
    }

    public int getMaxStageUnlocked()
    {
        return m_maxStageUnlocked;
    }
}
