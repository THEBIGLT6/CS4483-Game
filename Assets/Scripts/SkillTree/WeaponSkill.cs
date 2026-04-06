using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Skills/Weapon")]
public class WeaponSkill : Skill
{
    public WeaponSO newWeapon;

    public float gunDamageIncrement;
    public float meleeDamageIncrement;

    public override void apply()
    {
        if( newWeapon != null )
        {
            RaycastShooting shooting = null;
            GameObject obj = FindRootWithTag("PlayerContainer");
            if (obj != null)
            {
                shooting = obj.GetComponentInChildren<RaycastShooting>();
            }
            
            if( shooting !=null ) shooting.EquipWeapon( newWeapon );
        }
        else if( gunDamageIncrement != 0 )
        {
            RaycastShooting shooting = null;
            GameObject obj = FindRootWithTag("PlayerContainer");
            if (obj != null)
            {
                shooting = obj.GetComponentInChildren<RaycastShooting>();
            }

            if (shooting != null)  shooting.setDamageMultiplier( gunDamageIncrement );
        }
        else if( meleeDamageIncrement != 0 ) 
        {
            MeleeCombat melee = null;
            GameObject obj = FindRootWithTag("PlayerContainer");
            if (obj != null)
            {
                melee = obj.GetComponentInChildren <MeleeCombat>();
            }

            melee.setDamageMultiplier( meleeDamageIncrement );
        }
    }

    private GameObject FindRootWithTag(string tag)
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] roots = scene.GetRootGameObjects();

        foreach (GameObject obj in roots)
        {
            if (obj.CompareTag(tag))
            {
                return obj;
            }
        }

        return null;
    }
}
