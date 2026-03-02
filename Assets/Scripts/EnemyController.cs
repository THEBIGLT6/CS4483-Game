using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    public int hp = 3;

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
        target = FindObjectByTag("Target").transform;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        mat = GetComponent<Renderer>().material;
        m_Color = mat.color;
    }

    private void Update()
    {
        agent.SetDestination(target.position);

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
}
