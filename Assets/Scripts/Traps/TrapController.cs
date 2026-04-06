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
    private float nextFireTime = 0f;

    public Animator animator;

    void OnTriggerEnter(Collider other){
        if (Time.time > nextFireTime)
        {
            if (other.CompareTag("Zombie")){
                other.GetComponent<EnemyController>().TakeDamage(damage);
                if (animator != null)
                {
                    animator.SetTrigger("triggerTrap");
                }
            }
            Debug.Log("Trigger Activated!");

            nextFireTime = Time.time + cooldownTime;
        }
    }
}
