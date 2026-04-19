using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : TrapController 
{
    private List<EnemyController> zombiesInRange = new List<EnemyController>();

    void Update()
    {
        if (Time.time > nextFireTime)
        {
            FireTrap();
            nextFireTime = Time.time + cooldownTime;
        }
    }

    void FireTrap()
    {
        foreach (EnemyController enemy in zombiesInRange)
        {
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        if (animator != null)
        {
            animator.SetTrigger("triggerTrap");
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null && !zombiesInRange.Contains(enemy))
            {
                zombiesInRange.Add(enemy);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                zombiesInRange.Remove(enemy);
            }
        }
    }
}
