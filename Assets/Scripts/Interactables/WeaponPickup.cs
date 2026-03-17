using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Header("Weapon Data")]
    [SerializeField] private WeaponSO weaponToEquip;

    [Header("Visual Effects")]
    private Vector3 rotationSpeed = new Vector3(0, 100f, 0);
    private float floatAmplitude = 0.25f;
    private float floatSpeed = 2f;

    private GameObject modelInstance;
    private Vector3 startLocalPos;

    private void Start()
    {
        if (weaponToEquip != null && weaponToEquip.weaponPrefab != null)
        {
            modelInstance = Instantiate(weaponToEquip.weaponPrefab, transform);
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
                shooting.EquipWeapon(weaponToEquip);
                Destroy(gameObject);
            }
        }

        
    }
}
