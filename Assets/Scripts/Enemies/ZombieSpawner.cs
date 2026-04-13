using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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

    [Header("Ammo Refills")]
    private int m_startingRefillAmount;
    private float m_refillMultipler;

    [Header("Zombies")]
    [SerializeField] private GameObject m_basicZombie;
    private int m_zombieSpawnCount;
    private int m_zombiesToSpawn;
    private int m_zombiesAlive;
    [SerializeField] private Transform m_zombieContainer;

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
        // dont keep the spawner persistent
        _instance = this;

        m_startingRefillAmount = 20;
        m_refillMultipler = 1.0f;

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
        if (m_currentState == RoundState.WaitingToStart && m_roundText.isActiveAndEnabled )
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
        GameObject zombie = Instantiate(m_basicZombie, spawnPoint.position, spawnPoint.rotation, m_zombieContainer);

        m_zombiesAlive++;

        zombie.GetComponent<EnemyController>().onDeath += OnZombieDeath;
    }

    void OnZombieDeath()
    {
        m_zombiesAlive--;
        if (m_zombiesAlive <= 0 && m_zombieSpawnCount == m_zombiesToSpawn && m_currentState == RoundState.InProgress)
        {
            roundComplete();
        }
    }

    private void roundComplete()
    {
        m_currentRound++;

        bool triggeredDoor = false;

        if (m_currentRound % 3 == 0)
        {
            int doorIndex = (m_currentRound / 3 - 1) % 3;

            switch (doorIndex)
            {
                case 0:
                    m_door1To2.triggerDoorOpen();
                    break;
                case 1:
                    m_door2To3.triggerDoorOpen();
                    break;
                case 2:
                    m_door3To1.triggerDoorOpen();
                    break;
            }

            triggeredDoor = true;
        }

        giveAmmo();
        resetHealth();
        m_currentState = RoundState.RoundComplete;
        m_roundText.text = $"Round {m_currentRound}";

        if ( !triggeredDoor )
        {
            prepareRound();
        }

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

    private void giveAmmo()
    {
        RaycastShooting shooting = null;
        GameObject obj = FindRootWithTag("PlayerContainer");
        if (obj != null)
        {
            shooting = obj.GetComponentInChildren<RaycastShooting>();
        }

        int ammo = Mathf.RoundToInt( ( m_startingRefillAmount + ( m_currentRound * 10 ) ) * m_refillMultipler );
        if (shooting != null) shooting.addAmmo(ammo);
    }

    private void resetHealth()
    {
        PlayerController controller = null;
        GameObject obj = FindRootWithTag("PlayerContainer");
        if (obj != null)
        {
            controller = obj.GetComponentInChildren<PlayerController>();
        }

        if (controller != null) controller.healToMax();
    }

    private GameObject FindRootWithTag(string tag)
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] roots = scene.GetRootGameObjects();

        foreach (GameObject obj in roots)
        {
            if (obj.CompareTag(tag))
            {
                return obj;
            }
        }

        return null;
    }

    public void openStartRound()
    {
        m_currentState = RoundState.WaitingToStart;
        if( TrapsUI.Instance != null ) TrapsUI.Instance.giveAllotedTraps();
        m_startRound.gameObject.SetActive(true);
    }

    public void clearAllZombies()
    {
        foreach (Transform child in m_zombieContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void setRefillMultiplier(float multiplier)
    {
        m_refillMultipler = multiplier;
    }

    public void KnockbackZombies(Vector3 origin, float force, float radius)
    {
        foreach (Transform zombie in m_zombieContainer)
        {
            NavMeshAgent agent = zombie.GetComponent<NavMeshAgent>();
            Vector3 direction = (zombie.position - origin).normalized;
            float distance = Vector3.Distance(origin, zombie.position);

            if (distance <= radius)
            {
                float falloff = 1f - (distance / radius);

                if (agent != null)
                {
                    agent.enabled = false;
                    zombie.position += direction * force * falloff;   // apply force
                    StartCoroutine(ReenableAgent(agent, 0.3f));       // re-enable agent after force applied
                }
            }
        }
    }

    private IEnumerator ReenableAgent(NavMeshAgent agent, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (agent != null) agent.enabled = true;
    }

    public int getCurrentRound()
    {
        return m_currentRound;
    }

}
