using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    private int hp;

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
        target = FindObjectByTag("Target").transform;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //agent.SetDestination(target.position);

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public GameObject FindObjectByTag(string tag)
    {
        GameObject obj = GameObject.FindWithTag(tag);
        return obj;
    }

    public void TakeDamage(int damage)
    {
    hp -= damage;
    Debug.Log($"Enemy {name} took {damage}. HP now: {hp}");
    }

    public int getHP(){
        return hp;
    }

}
