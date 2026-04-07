using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Skills/Player")]
public class PlayerSkill : Skill
{
    public float healthIncrement;
    public float movmentSpeedIncrement;
    public float reloadSpeedIncrement;
    public float ammoIncrement;
    public float cashIncrease;         // cash amount increase per kill
    public bool secondShot;

    public override void apply()
    {
        if( healthIncrement != 0f )
        {
            PlayerController controller = null;
            GameObject obj = FindRootWithTag("PlayerContainer");
            if (obj != null)
            {
                controller = obj.GetComponentInChildren<PlayerController>();
            }

            if (controller != null) controller.setMaxHealth( Mathf.RoundToInt( controller.maxHp * healthIncrement ) );
        }
        else if( movmentSpeedIncrement != 0f )
        {
            PlayerController controller = null;
            GameObject obj = FindRootWithTag("PlayerContainer");
            if (obj != null)
            {
                controller = obj.GetComponentInChildren<PlayerController>();
            }

            if (controller != null) controller.setSpeedMultiplier( movmentSpeedIncrement );
        }
        else if( reloadSpeedIncrement != 0f )
        {
            RaycastShooting shooting = null;
            GameObject obj = FindRootWithTag("PlayerContainer");
            if (obj != null)
            {
                shooting = obj.GetComponentInChildren<RaycastShooting>();
            }

            if (shooting != null) shooting.setReloadSpeedIncrease( reloadSpeedIncrement );
        }
        else if( ammoIncrement != 0f )
        {
            ZombieSpawner.Instance.setRefillMultiplier( ammoIncrement );
        }
        else if( cashIncrease != 0f )
        {
            GameManager.Instance.setMoneyMultiplier( cashIncrease );
        }
        else if( secondShot )
        {

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
