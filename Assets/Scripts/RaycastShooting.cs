using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class RaycastShooting : MonoBehaviour
{
    public TMP_Text ammoText;
    public Slider reloadSlider;
    private WeaponSO weapon;
    public LayerMask enemyLayer;
    public Color damageColor;
    public TrailRenderer bulletTracer;
    public ParticleSystem muzzleFlash;
    public Transform muzzlePoint; 

    private bool reloading = false;
    [SerializeField] private Transform gunAttatchPoint;
    private GameObject weaponModel;

    private void Start()
    {
        if( weapon != null )
        {
            weapon.ammo = weapon.magazineSize;
        }

        reloadSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        //Debug.DrawRay(transform.position, transform.forward * weapon.maxDistance, Color.white);
        //Debug.DrawRay(transform.position, Quaternion.Euler(0, weapon.coneAngle, 0) * transform.forward * weapon.maxDistance, Color.magenta);
        //Debug.DrawRay(transform.position, Quaternion.Euler(0, -weapon.coneAngle, 0) * transform.forward * weapon.maxDistance, Color.magenta);
        
        if( weapon != null )
        {
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
        else
        {
            ammoText.text = "";
        }


        if (Input.GetKeyDown(KeyCode.V))
        {
            EquipWeapon(weapon);
        }
    }

    void ShootRaycast()
    {
        Vector3 direction = Camera.main.transform.forward;

        MusicManager.Instance.playOneShot( weapon.shootSound );

        // Muzzle flash
        if (muzzleFlash != null)
            muzzleFlash.Play();

        // Bullet tracer
        if (bulletTracer != null && muzzlePoint != null)
            StartCoroutine(SpawnTracer(muzzlePoint.position, direction));
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, weapon.maxDistance, enemyLayer);
        //Debug.Log("Colliders found: " + hitColliders.Length);

        foreach (var collider in hitColliders)
        {
            //Debug.Log("Object in overlapSphere: " + collider.name);
            
            Vector3 toTarget = collider.transform.position - transform.position;
            float angle = Vector3.Angle(direction, toTarget);
            //Debug.Log("Angle to " + collider.name + ": " + angle);
            if (angle < weapon.coneAngle+15) //+5 degrees of tolerance because we are only counting from the center of the object
            {
                //Debug.Log("Object in cone: " + collider.name);
                EnemyController enemy = collider.GetComponentInParent<EnemyController>();
                //Debug.Log("Enemy component found: " + (enemy != null) + " on: " + collider.name);

                if (enemy == null) continue; //ignoring the "agent in enemycontroller is null for now

                //Debug.Log("Hit: " + collider.name);
                enemy.TakeDamage(weapon.damage);
                //Debug.Log("Zombie HP: " + enemy.getHP());
                //Debug.Log(weapon.damage);
                if (enemy !=null){
                    Vector3 knockbackDirection = toTarget.normalized;
                    
                    enemy.rb.AddForce(knockbackDirection * weapon.knockbackForce, ForceMode.Impulse);
                }
                StartCoroutine(DamageAgent(enemy));
            }
        }
    }
    
    IEnumerator DamageAgent(EnemyController enemy)
    {
        //enemy.agent.isStopped = true;
        enemy.mat.DOColor(damageColor, 0.1f);
        
        yield return new WaitForSeconds(0.1f);
        
        if( enemy.rb != null )
        {
            enemy.rb.velocity = Vector3.zero;
            enemy.rb.angularVelocity = Vector3.zero;
        }

        
        //enemy.agent.ResetPath();
        //enemy.agent.isStopped = false;
        //enemy.mat.DOColor(enemy.m_Color, 0.1f);
        //enemy.hp -= weapon.damage;
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

    IEnumerator SpawnTracer(Vector3 origin, Vector3 direction)
    {
        TrailRenderer tracer = Instantiate(bulletTracer, origin, Quaternion.identity);
        tracer.transform.position = origin;
        
        Vector3 endPoint = origin + direction * weapon.maxDistance;
        float distance = Vector3.Distance(origin, endPoint);
        float remainingDistance = distance;
        
        while (remainingDistance > 0)
        {
            tracer.transform.position = Vector3.Lerp(origin, endPoint, 1 - (remainingDistance / distance));
            remainingDistance -= Time.deltaTime * 100f;
            yield return null;
        }
        
        tracer.transform.position = endPoint;
        Destroy(tracer.gameObject, tracer.time);
    }

    public void EquipWeapon(WeaponSO newWeapon)
    {
        // Destroy old model
        if (weaponModel != null)
        {
            Destroy(weaponModel);
        }

        // Set new data
        weapon = newWeapon;
        weapon.ammo = weapon.magazineSize;
        weaponModel = Instantiate( newWeapon.weaponPrefab, gunAttatchPoint );
        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;

        muzzleFlash = weaponModel.GetComponentInChildren<ParticleSystem>();
        foreach (Transform child in weaponModel.transform)
        {
            if (child.CompareTag("MuzzlePoint")) muzzlePoint = child;
        }

    }
}