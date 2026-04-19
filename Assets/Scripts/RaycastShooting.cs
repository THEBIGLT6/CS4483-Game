using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;

public class RaycastShooting : MonoBehaviour
{
    public TMP_Text ammoText;
    public Slider reloadSlider;
    private WeaponSO weapon;
    public LayerMask enemyLayer;
    public TrailRenderer bulletTracer;
    public ParticleSystem muzzleFlash;
    public Transform muzzlePoint; 

    private bool reloading = false;
    private float reloadSpeedIncrease;
    private float m_nextFireTime = 0f;
    [SerializeField] private Transform gunAttatchPoint;
    private GameObject weaponModel;
    private bool hideWeapon;

    [SerializeField] WeaponSO startingWeapon;

    private float damageMultiplier;

    private int totalAmmo;
    [SerializeField] private AudioClip reloadSound;


    private void Start()
    {
        reloadSpeedIncrease = 1.0f;
        damageMultiplier = 1.0f;

        if( weapon != null )
        {
            weapon.ammo = weapon.magazineSize;
        }

        reloadSlider.gameObject.SetActive(false);
        hideWeapon = false;

        totalAmmo = 20;

        if( startingWeapon != null )
        {
            EquipWeapon( startingWeapon );
        }
    }

    void Update()
    {
        //Debug.DrawRay(transform.position, transform.forward * weapon.maxDistance, Color.white);
        //Debug.DrawRay(transform.position, Quaternion.Euler(0, weapon.coneAngle, 0) * transform.forward * weapon.maxDistance, Color.magenta);
        //Debug.DrawRay(transform.position, Quaternion.Euler(0, -weapon.coneAngle, 0) * transform.forward * weapon.maxDistance, Color.magenta);
        
        if( weapon != null )
        {
            bool canShoot = Time.time >= m_nextFireTime;
            if (weapon.isAutomatic)
            {
                if (Input.GetButton("Fire1") && canShoot && weapon.ammo > 0 && !reloading && Time.timeScale > 0f) fire();
                
            }
            else
            {
                if (Input.GetButtonDown("Fire1") && canShoot && weapon.ammo > 0 && !reloading && Time.timeScale > 0f) fire();
                
            }

            if (weapon.ammo <= 0 && !reloading && totalAmmo > 0)
            {
                StartCoroutine(ReloadGun(weapon));
            }

            ammoText.text = weapon.ammo + "/" + totalAmmo;
        }
        else
        {
            ammoText.text = "";
        }
    
        if( Input.GetKeyDown( KeyCode.R ) && !reloading && totalAmmo > 0)
        {
            StartCoroutine( ReloadGun(weapon) );
        }
    }

    private void fire()
    {
        weapon.ammo--;
        ShootRaycast();

        m_nextFireTime = Time.time + weapon.fireRate;
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
            if( !collider.isTrigger ) continue;  // account for 2nd capsule collider on zombie

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
                int damage = Mathf.CeilToInt(weapon.damage * damageMultiplier);
                enemy.TakeDamage(damage);
                //Debug.Log("Zombie HP: " + enemy.getHP());
                //Debug.Log(weapon.damage);
                if (enemy !=null){
                    Vector3 knockbackDirection = toTarget.normalized;
                    
                    enemy.rb.AddForce(knockbackDirection * weapon.knockbackForce, ForceMode.Impulse);
                }
                
            }
        }
    }
    
    private IEnumerator ReloadGun(WeaponSO weapon)
    {
        if( weapon != null )
        {
            reloading = true;
            reloadSlider.gameObject.SetActive(true);
            reloadSlider.value = 0;

            float adjustedReloadTime = weapon.reloadTime / reloadSpeedIncrease;

            reloadSlider.DOValue(1, adjustedReloadTime).SetEase(Ease.Linear);;
            MusicManager.Instance.playOneShot( reloadSound );

            yield return new WaitForSeconds(adjustedReloadTime);
        
            reloadSlider.gameObject.SetActive(false);

            int ammoNeeded = weapon.magazineSize - weapon.ammo;
            if ( totalAmmo >= ammoNeeded )
            {
                totalAmmo -= ammoNeeded;
                weapon.ammo = weapon.ammo + ammoNeeded;
            }
            else
            {
                weapon.ammo = weapon.ammo + totalAmmo;
                totalAmmo = 0;
            }

            reloading = false;
        }
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
        damageMultiplier = 1f;

        muzzleFlash = weaponModel.GetComponentInChildren<ParticleSystem>();
        foreach (Transform child in weaponModel.transform)
        {
            if (child.CompareTag("MuzzlePoint")) muzzlePoint = child;
        }

        if( hideWeapon ) weaponModel.SetActive(false);

    }

    public void addAmmo( int ammo )
    {
        totalAmmo += ammo;
    }

    public void setAmmo( int ammo )
    {
        totalAmmo = ammo;
    }

    public void setDamageMultiplier( float multiple )
    {
        damageMultiplier = multiple;
    }

    public void setReloadSpeedIncrease( float increase )
    {
        reloadSpeedIncrease = increase;
    }

    void OnDisable()
    {
        hideWeapon = true;
        if (weaponModel != null)
        {
            weaponModel.SetActive(false);
        }
    }

    void OnEnable()
    {
        hideWeapon = false;
        if (weaponModel != null)
        {
            weaponModel.SetActive(true);
        }
    }
}