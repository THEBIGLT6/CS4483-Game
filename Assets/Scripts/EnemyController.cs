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

    public GameObject FindObjectByTag(string tag)
    {
        GameObject obj = GameObject.FindWithTag(tag);
        return obj;
    }

    public void TakeDamage(int damage)
    {
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

}
