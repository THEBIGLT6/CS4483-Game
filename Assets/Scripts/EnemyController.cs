using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyController : MonoBehaviour
{
    [Header("Zombie Variables")]
    private int hp;
    private int moneyReward; 
    private int maxHp;
    private Renderer[] allRenderers;
    private Color damageColor = new Color( 183f/255f, 34f/255f, 34f / 255f, 0);

    [Header("Notifiers")]
    public Action onDeath;
    private Transform target;

    [Header("Push Back Settings")]
    private float pushRadius = 2.5f;
    private float pushForce = 20f;

    [Header("Audio")]
    [SerializeField] private AudioClip[] m_idleSounds;
    [SerializeField] private AudioClip[] m_damageSounds;
    [SerializeField] private AudioClip[] m_attackSounds;
    private AudioSource m_audioSource;
    private float m_minDelay = 4f;
    private float m_maxDelay = 20f;

    [Header("Attacks")]
    private bool canAttack = true;
    private float attackCooldown = 3f;
    private float lastAttackTime = -Mathf.Infinity;
    [SerializeField] private float attackRange = 1.75f;
    [SerializeField] private float attackRadius = 0.75f;

    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public Material mat;
    [HideInInspector]
    public Color m_Color;
    [HideInInspector]
    public Animator animator;

    private void Start()
    {
        hp = 100;
        maxHp = hp;
        moneyReward = 2;

        target = FindObjectByTag("Player").transform;
        
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        m_audioSource = GetComponent<AudioSource>();
        allRenderers = GetComponentsInChildren<Renderer>();
        animator = GetComponent<Animator>();
        m_Color = allRenderers[0].material.color;

        applyVolume();
        StartCoroutine( PlayRandomSounds() );
    }

    private void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.SetDestination(target.position);
            ApplyPlayerPush();
        }

        if (hp <= 0)
        {
            die();
        }

        if ( canAttack )
        {
            TryAttack();
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
        UpdateColor();
        //Debug.Log(gameObject.name + " HP: " + hp);

        StartCoroutine(DamageAgent());
    }

    IEnumerator DamageAgent()
    {
        //enemy.agent.isStopped = true;
        mat.DOColor(damageColor, 0.1f);

        yield return new WaitForSeconds(0.1f);

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }


        //enemy.agent.ResetPath();
        //enemy.agent.isStopped = false;
        //enemy.mat.DOColor(enemy.m_Color, 0.1f);
        //enemy.hp -= weapon.damage;
    }

    public int getHP()
    {
        return hp;
    }

    private void die()
    {
        agent.enabled = false;
        canAttack = false;
        animator.SetTrigger("dead");
    }

    private void destroyZombie()
    {
        GameManager.Instance.addMoney( moneyReward );
        onDeath?.Invoke();
        Destroy( gameObject );
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

    void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        if (Physics.SphereCast(transform.position, attackRadius, transform.forward, out RaycastHit hit, attackRange))
        {
            var player = hit.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                triggerAttack(hit.collider);
                lastAttackTime = Time.time;
            }
        }
    }

    private void triggerAttack(Collider other)
    {
        animator.SetTrigger("attack");
        AudioClip clip = m_attackSounds[UnityEngine.Random.Range(0, m_attackSounds.Length)];
        m_audioSource.PlayOneShot(clip);
    }

    public void performAttack()
    {
        if (Physics.SphereCast(transform.position, attackRadius, transform.forward, out RaycastHit hit, attackRange))
        {
            var player = hit.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(25);
            }
        }
    }

    private void applyVolume()
    {
        m_audioSource.volume = MusicManager.Instance.soundFXVolume();   
    }

   void UpdateColor()
    {
        float healthPercent = (float)hp / maxHp;
        foreach (Renderer r in allRenderers)
        {
            r.material.color = Color.Lerp(Color.red, m_Color, healthPercent);
        }
    }

    private void ApplyPlayerPush()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance < pushRadius)
        {
            Vector3 dir = (transform.position - target.position).normalized;
            float strength = (pushRadius - distance) / pushRadius * pushForce; // stronger push when closer

            agent.Move(dir * strength * Time.deltaTime);
        }
    }

    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 origin = transform.position + Vector3.up * 1.2f;
        Vector3 end = origin + transform.forward * attackRange;

        // Draw start + end spheres
        Gizmos.DrawWireSphere(origin, attackRadius);
        Gizmos.DrawWireSphere(end, attackRadius);

        // Draw lines connecting them (like a capsule)
        Gizmos.DrawLine(origin + transform.right * attackRadius, end + transform.right * attackRadius);
        Gizmos.DrawLine(origin - transform.right * attackRadius, end - transform.right * attackRadius);
        Gizmos.DrawLine(origin + transform.up * attackRadius, end + transform.up * attackRadius);
        Gizmos.DrawLine(origin - transform.up * attackRadius, end - transform.up * attackRadius);
    }
    */
}
