using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Header("Weapon / Ammo Data")]
    [SerializeField] private WeaponSO weaponToEquip;
    [SerializeField] private int ammoAmount;
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private bool useWeapon;           // uses weapon if true, ammo if false

    [Header("Visual Effects")]
    private Vector3 rotationSpeed = new Vector3(0, 100f, 0);
    private float floatAmplitude = 0.25f;
    private float floatSpeed = 2f;

    private GameObject modelInstance;
    private Vector3 startLocalPos;

    private void Start()
    {

        if (useWeapon && weaponToEquip != null && weaponToEquip.weaponPrefab != null)
        {
            modelInstance = Instantiate(weaponToEquip.weaponPrefab, transform);
            modelInstance.transform.localPosition = Vector3.zero;
            modelInstance.transform.localRotation = Quaternion.identity;
            startLocalPos = modelInstance.transform.localPosition;
        }
        else if (!useWeapon && ammoPrefab != null)
        {
            modelInstance = Instantiate(ammoPrefab, transform);
            modelInstance.transform.localPosition = Vector3.zero;
            modelInstance.transform.localRotation = Quaternion.identity;
            startLocalPos = modelInstance.transform.localPosition;
        }
    }

    private void Update()
    {
        if (modelInstance == null) return;

        // Spin
        modelInstance.transform.Rotate(rotationSpeed * Time.deltaTime);

        // Float
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        modelInstance.transform.localPosition = startLocalPos + new Vector3(0, yOffset, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            GameObject parentObj = other.transform.parent?.gameObject;
            RaycastShooting shooting = parentObj.GetComponentInChildren<RaycastShooting>();
            if (shooting != null)
            {
                if( useWeapon ) shooting.EquipWeapon(weaponToEquip);
                else            shooting.addAmmo(ammoAmount);
                Destroy(gameObject);
            }
        }
    }
}
