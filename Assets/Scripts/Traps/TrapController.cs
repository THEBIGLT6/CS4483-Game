using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{

    public float cooldownTime; 
    private float nextFireTime = 0f;

    public Animator animator;

    void OnTriggerEnter(Collider other){
        if (Time.time > nextFireTime)
        {
            if (other.CompareTag("Zombie")){
                other.GetComponent<EnemyController>().TakeDamage(3);
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
