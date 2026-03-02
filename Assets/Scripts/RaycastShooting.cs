using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class RaycastShooting : MonoBehaviour
{
    public TMP_Text ammoText;
    public Slider reloadSlider;
    public WeaponSO weapon;
    public LayerMask enemyLayer;
    public Color damageColor;

    private bool reloading = false;

    private void Start()
    {
        weapon.ammo = weapon.magazineSize;
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * weapon.maxDistance, Color.white);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, weapon.coneAngle, 0) * transform.forward * weapon.maxDistance, Color.magenta);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, -weapon.coneAngle, 0) * transform.forward * weapon.maxDistance, Color.magenta);
        
        if (Input.GetButtonDown("Fire1") && weapon.ammo >= 1)
        {
            weapon.ammo--;
            ShootRaycast();
        }
        else if (weapon.ammo <= 0 && !reloading)
        {
            StartCoroutine(ReloadGun(weapon));
        }
        
        ammoText.text = weapon.ammo + "/" + weapon.magazineSize;
    }

    void ShootRaycast()
    {
        Vector3 direction = transform.forward;
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, weapon.maxDistance, enemyLayer);
        
        foreach (var collider in hitColliders)
        {
            //Debug.Log("Object in overlapSphere: " + collider.name);
            
            Vector3 toTarget = collider.transform.position - transform.position;
            float angle = Vector3.Angle(direction, toTarget);
            if (angle < weapon.coneAngle+5) //+5 degrees of tolerance because we are only counting from the center of the object
            {
                //Debug.Log("Object in cone: " + collider.name);
                EnemyController enemy = collider.GetComponent<EnemyController>();
                
                Vector3 knockbackDirection = toTarget.normalized;
                
                enemy.rb.AddForce(knockbackDirection * weapon.knockbackForce, ForceMode.Impulse);
                
                StartCoroutine(DamageAgent(enemy));
            }
        }
    }
    
    IEnumerator DamageAgent(EnemyController enemy)
    {
        enemy.agent.isStopped = true;
        enemy.mat.DOColor(damageColor, 0.1f);
        
        yield return new WaitForSeconds(0.1f);
        
        enemy.rb.velocity = Vector3.zero;
        enemy.rb.angularVelocity = Vector3.zero;
        
        enemy.agent.ResetPath();
        enemy.agent.isStopped = false;
        enemy.mat.DOColor(enemy.m_Color, 0.1f);
        enemy.hp -= weapon.damage;
    }
    
    private IEnumerator ReloadGun(WeaponSO weapon)
    {
        reloading = true;
        reloadSlider.gameObject.SetActive(true);
        reloadSlider.value = 0;
        reloadSlider.DOValue(1, weapon.reloadTime).SetEase(Ease.Linear);;
        
        yield return new WaitForSeconds(weapon.reloadTime);
        
        reloadSlider.gameObject.SetActive(false);
        weapon.ammo = weapon.magazineSize;
        reloading = false;
    }
}