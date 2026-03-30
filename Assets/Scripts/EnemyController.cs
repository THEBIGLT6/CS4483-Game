using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Zombie Variables")]
    private int hp;
    private int moneyReward; 

    [Header("Notifiers")]
    public Action onDeath;
    private Transform target;

    [Header("Audio")]
    [SerializeField] private AudioClip[] m_idleSounds;
    [SerializeField] private AudioClip[] m_damageSounds;
    [SerializeField] private AudioClip[] m_attackSounds;
    private AudioSource m_audioSource;
    private float m_minDelay = 4f;
    private float m_maxDelay = 20f;

    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Material mat;
    [HideInInspector]
    public Color m_Color;

    private void Start()
    {
        hp = 3;
        moneyReward = 2;

        target = FindObjectByTag("Player").transform;
        
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        m_audioSource = GetComponent<AudioSource>();

        applyVolume();
        StartCoroutine( PlayRandomSounds() );
    }

    private void Update()
    {
        if( agent.isActiveAndEnabled ) agent.SetDestination(target.position);

        if (hp <= 0)
        {
            die();
        }

        // for debugging
        //if( Input.GetKeyDown( KeyCode.X ) )
        //{
        //    die();
        //}
    }

    void OnEnable()
    {
        SettingsManager.Instance.m_OnSettingsChanged += applyVolume;
    }

    void OnDisable()
    {
        SettingsManager.Instance.m_OnSettingsChanged -= applyVolume;
        StopAllCoroutines();
    }

    public GameObject FindObjectByTag(string tag)
    {
        GameObject obj = GameObject.FindWithTag(tag);
        return obj;
    }

    public void TakeDamage(int damage)
    {
        AudioClip clip = m_damageSounds[UnityEngine.Random.Range(0, m_damageSounds.Length)];
        m_audioSource.PlayOneShot(clip);

        hp -= damage;
        //Debug.Log($"Enemy {name} took {damage}. HP now: {hp}");
    }

    public int getHP()
    {
        return hp;
    }

    private void die()
    {
        GameManager.Instance.addMoney( moneyReward );
        onDeath?.Invoke();
        Destroy(gameObject);
    }

    private System.Collections.IEnumerator PlayRandomSounds()
    {
        while (true)
        {
            float wait = UnityEngine.Random.Range(m_minDelay, m_maxDelay);
            yield return new WaitForSeconds(wait);

            PlayRandomSound();
        }
    }

    private void PlayRandomSound()
    {
        if (m_idleSounds.Length == 0) return;

        AudioClip clip = m_idleSounds[UnityEngine.Random.Range(0, m_idleSounds.Length)];

        m_audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f); // slight pitch change
        m_audioSource.PlayOneShot(clip);
    }

    private void attack()
    {
        AudioClip clip = m_attackSounds[UnityEngine.Random.Range(0, m_attackSounds.Length)];
        m_audioSource.PlayOneShot(clip);
    }

    private void applyVolume()
    {
        m_audioSource.volume = MusicManager.Instance.soundFXVolume();   
    }

}
