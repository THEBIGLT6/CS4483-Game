using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZombieSpawner : MonoBehaviour
{
    enum RoundState
    {
        WaitingToStart,
        InProgress,
        RoundComplete
    }

    // Singleton pattern implementation
    private static ZombieSpawner _instance;
    public static ZombieSpawner Instance => _instance;

    [Header("Zombies")]
    [SerializeField] private GameObject m_basicZombie;
    private int m_zombieSpawnCount;
    private int m_zombiesToSpawn;
    private int m_zombiesAlive;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] m_stage1Points;
    [SerializeField] private Transform[] m_stage2Points;
    [SerializeField] private Transform[] m_stage3Points;
    [SerializeField] private Transform[] m_stage4Points;

    [Header("Round Settings")]
    private int m_startingZombies = 15;
    private float m_spawnInterval = 1.0f;
    private int m_currentRound;
    private RoundState m_currentState;

    [Header("UI")]
    [SerializeField] private TMP_Text m_roundText;
    [SerializeField] private SkillTreeUI m_skillTreeUI;
    [SerializeField] private TMP_Text m_startRound;

    [Header("Door Controllers")]
    [SerializeField] private DoorController m_door1To2;
    [SerializeField] private DoorController m_door2To3;
    [SerializeField] private DoorController m_door3To1;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);


        m_currentRound = 1;
        m_roundText.text = $"Round {m_currentRound}";
        m_startRound.gameObject.SetActive(false);

        if ( GameManager.Instance.getCurrentStage() == 0 )
        {
            GameManager.Instance.setStage(1);
        }

        prepareRound();
        m_skillTreeUI.toggleUpgradeScreen( false );
    }

    private void Update()
    {
        if (m_currentState == RoundState.WaitingToStart && m_startRound.isActiveAndEnabled)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                StartRound();
            }
        }
    }

    public void prepareRound()
    {
        m_zombiesToSpawn = m_startingZombies + (m_currentRound * 5) ;
        m_zombieSpawnCount = 0;
        m_skillTreeUI.toggleUpgradeScreen( true );
    }

    private void StartRound()
    {
        m_startRound.gameObject.SetActive( false );
        m_currentState = RoundState.InProgress;
        StartCoroutine( SpawnZombies() );
    }

    private IEnumerator SpawnZombies()
    {
        while (m_zombieSpawnCount < m_zombiesToSpawn)
        {
            SpawnZombie();
            m_zombieSpawnCount++;
            yield return new WaitForSeconds( m_spawnInterval );
        }
    }

    private void SpawnZombie()
    {
        Transform spawnPoint = getRandomSpawnPoint();
        GameObject zombie = Instantiate(m_basicZombie, spawnPoint.position, spawnPoint.rotation);

        m_zombiesAlive++;

        zombie.GetComponent<EnemyController>().onDeath += OnZombieDeath;
    }

    void OnZombieDeath()
    {
        m_zombiesAlive--;
        if (m_zombiesAlive <= 0 && m_currentState == RoundState.InProgress)
        {
            roundComplete();
        }
    }

    private void roundComplete()
    {  
        int currentStage = GameManager.Instance.getCurrentStage();
        if ( currentStage == 1 )      m_door1To2.triggerDoorOpen();
        else if ( currentStage == 2 ) m_door2To3.triggerDoorOpen();
        else if ( currentStage == 3 ) m_door3To1.triggerDoorOpen();
        
        m_currentRound++;
        m_currentState = RoundState.RoundComplete;
        m_roundText.text = $"Round {m_currentRound}";
    }

    private Transform getRandomSpawnPoint()
    {
        Transform[] activePoints = getStagePoints();
        return activePoints[Random.Range(0, activePoints.Length)];
    }

    private Transform[] getStagePoints()
    {
        int currentStage = GameManager.Instance.getCurrentStage(); 
        if      ( currentStage == 1 ) return m_stage1Points;
        else if ( currentStage == 2 ) return m_stage2Points;
        else if ( currentStage == 3 ) return m_stage3Points;
        else                          return m_stage4Points;
    }

    public void openStartRound()
    {
        m_currentState = RoundState.WaitingToStart;
        m_startRound.gameObject.SetActive(true);
    }

}
