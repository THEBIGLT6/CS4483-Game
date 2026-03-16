using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)] 
public class WeaponSO : ScriptableObject
{
    public float coneAngle = 30f;
    public float maxDistance = 10f;
    public float knockbackForce = 10f;
    public int damage = 25;
    public float reloadTime = 1.8f;
    public int magazineSize = 30;
    public int ammo = 90;
    public float headShotMultiplier = 2.5f; // Damage multiplier for headshots 
    public float fireRate = 0.15f; //seconds between each shot

}
