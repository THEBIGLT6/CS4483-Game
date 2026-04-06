using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class MeleeCombat : MonoBehaviour
{
    [Header("Positioning")]
    [SerializeField] private Transform m_attachPoint;

    [Header("Weapons")]
    [SerializeField] private RaycastShooting m_shooting;
    [SerializeField] private MeleeWeaponSO m_currentWeapon;
    private GameObject m_weaponModel;
    private float m_damageMultiplier;

    [Header("Swinging")]
    private Vector3 m_startPos;
    private Quaternion m_startRot;
    private bool m_isSwinging;
    private float m_t;

    [Header("Hitting Enemy")]
    private bool m_hasHit;

    [Header("Audio")]
    [SerializeField] private AudioClip m_grunt;

    void Start()
    {
        m_damageMultiplier = 1f;
        m_t = 0f;
        m_isSwinging = false;
        m_hasHit = false;
    }

    void Update()
    {
        if( Input.GetKeyDown(KeyCode.V) && !m_isSwinging)
        {
            m_isSwinging = true;
            m_t = 0f;
            m_hasHit = false;

            m_shooting.enabled = false;
            m_weaponModel = Instantiate( m_currentWeapon.weaponPrefab, m_attachPoint);
            m_weaponModel.transform.localPosition = Vector3.zero;
            m_weaponModel.transform.localRotation = Quaternion.identity;

            m_startRot = m_weaponModel.transform.localRotation;
            m_startPos = m_weaponModel.transform.localPosition;

            MusicManager.Instance.playOneShot( m_grunt );
        }

        if( m_isSwinging)
        {
            swingObject();
        }
    }

    private void swingObject()
    {
        m_t += Time.deltaTime / m_currentWeapon.swingSpeedFactor;

        // Check for hit
        if (!m_hasHit && m_t >= m_currentWeapon.hitWindowStart && m_t <= m_currentWeapon.hitWindowEnd )
        {
            performHit();
            m_hasHit = true;
        }

        // Windup
        if ( m_t < m_currentWeapon.halfwayPoint)
        {
            float curve = m_t;
            m_weaponModel.transform.localPosition = m_startPos + m_currentWeapon.windupPos * curve;
            m_weaponModel.transform.localRotation = m_startRot * Quaternion.Euler(m_currentWeapon.windupRot * curve);
        }
        // Slash
        else
        {
            float curve = m_t;
            m_weaponModel.transform.localPosition = m_startPos + m_currentWeapon.slashPos * curve;
            m_weaponModel.transform.localRotation = m_startRot * Quaternion.Euler(m_currentWeapon.slashRot * curve);
        }

        // RESET
        if ( m_t >= m_currentWeapon.swingLength )
        {
            m_isSwinging = false;
            m_weaponModel.transform.localPosition = m_startPos;
            m_weaponModel.transform.localRotation = m_startRot;

            m_shooting.enabled = true;
            if (m_weaponModel != null)
            {
                Destroy(m_weaponModel);
            }
        }

    }

    private void performHit()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, m_currentWeapon.range))
        {

            // Try to damage enemy
            EnemyController health = hit.collider.GetComponent<EnemyController>();
            if (health != null)
            {
                int damage = Mathf.CeilToInt( m_currentWeapon.damage * m_damageMultiplier);
                health.TakeDamage( m_currentWeapon.damage );
            }
        }
    }

    public void equipMeleeWeapon( MeleeWeaponSO newMeleeWeapon )
    {
        m_currentWeapon = newMeleeWeapon;   
    }

    public void setDamageMultiplier( float multiple )
    {
        m_damageMultiplier = multiple;
    }
}
