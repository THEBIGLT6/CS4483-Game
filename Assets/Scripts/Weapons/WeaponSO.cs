using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)] 
public class WeaponSO : ScriptableObject
{
    public float coneAngle = 30f;
    public float maxDistance = 10f;
    public float knockbackForce = 10f;
    public int damage;
    public float reloadTime;
    public int magazineSize = 5;
    public int ammo = 5;
}
