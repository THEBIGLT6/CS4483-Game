using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MeleeWeapon", menuName = "ScriptableObjects/MeleeWeapon")] 
public class MeleeWeaponSO : ScriptableObject
{
    public int damage;
    public float range;
    public GameObject weaponPrefab;
    public float swingSpeedFactor;     // 0-1, where 1 is slower 
    public float swingLength;
    public float halfwayPoint;         // the point where windup -> Slash
    public Vector3 windupPos;          // new Vector3(1f, 1.0f, 0f);  // up + right + back
    public Vector3 windupRot;          // new Vector3(-10f, 30f, 10f);
    public Vector3 slashPos;           // new Vector3(-0.6f, 0f, 0f);    // LEFT + down + forward
    public Vector3 slashRot;           // new Vector3(-30f, -80f, -10f);
    public float hitWindowStart;
    public float hitWindowEnd;

}
