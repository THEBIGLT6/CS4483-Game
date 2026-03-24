using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private GameObject m_upgradeScreen;

    [Header("Shooting")]
    [SerializeField] private RaycastShooting m_raycastShooting;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void toggleUpgradeScreen( bool setActive )
    {
        m_upgradeScreen.SetActive( setActive );
        m_raycastShooting.enabled = !setActive;
        Cursor.lockState = setActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = setActive;
        if( !setActive ) ZombieSpawner.Instance.openStartRound();
    }
}
