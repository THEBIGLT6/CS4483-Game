using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapType
{
    Fire,
    Roller,
    Spike,
    BearTrap
}


public class TrapController : MonoBehaviour
{
    public TrapType type;
    public float cooldownTime; 
    public int damage;
    protected float nextFireTime = 0f;

    public Animator animator;

    protected virtual void OnTriggerEnter(Collider other){
        if (Time.time > nextFireTime)
        {
            if (other.CompareTag("Zombie")){
                other.GetComponent<EnemyController>().TakeDamage(damage);
                if (animator != null)
                {
                    animator.SetTrigger("triggerTrap");
                }
            }

            nextFireTime = Time.time + cooldownTime;
        }
    }
}
