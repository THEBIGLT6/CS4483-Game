
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrapsUI : MonoBehaviour
{

    private int m_trap1Count;
    private int m_trap2Count;
    private int m_trap3Count;
    private int m_trap4Count;

    private bool m_selectMode;

    [Header("UI Components")]
    [SerializeField] private Image m_background1;
    [SerializeField] private Image m_background2;
    [SerializeField] private Image m_background3;
    [SerializeField] private Image m_background4;
    [SerializeField] private TextMeshProUGUI m_trap1CountText;
    [SerializeField] private TextMeshProUGUI m_trap2CountText;
    [SerializeField] private TextMeshProUGUI m_trap3CountText;
    [SerializeField] private TextMeshProUGUI m_trap4CountText;
    [SerializeField] private Image m_keyPicture1;
    [SerializeField] private Image m_keyPicture2;
    [SerializeField] private Image m_keyPicture3;
    [SerializeField] private Image m_keyPicture4;

    [Header("Trap Prefabs")]
    [SerializeField] private GameObject m_trap1Prefab;
    [SerializeField] private GameObject m_trap2Prefab;
    [SerializeField] private GameObject m_trap3Prefab;
    [SerializeField] private GameObject m_trap4Prefab;
    private GameObject m_selectedTrapPrefab;

    [Header("Placement")]
    [SerializeField] private Transform m_playerCamera;
    private float m_placeDistance = 20f;
    private GameObject m_previewTrap;

    [Header("Shooting")]
    [SerializeField] private RaycastShooting m_shooting;

    void Start()
    {
        m_selectMode = false;

        m_background1.enabled = false;
        m_background2.enabled = false;
        m_background3.enabled = false;
        m_background4.enabled = false;
        m_trap1CountText.text = "";
        m_trap2CountText.text = "";
        m_trap3CountText.text = "";
        m_trap4CountText.text = "";
        m_keyPicture1.enabled = false;
        m_keyPicture2.enabled = false;
        m_keyPicture3.enabled = false;
        m_keyPicture4.enabled = false;
    }

    private void Update()
    {
        // Debug keys for testing
        //if (Input.GetKeyDown(KeyCode.Alpha5)) addTrap(1, 1);
        //if (Input.GetKeyDown(KeyCode.Alpha6)) addTrap(2, 1);
        //if (Input.GetKeyDown(KeyCode.Alpha7)) addTrap(3, 1);
        //if (Input.GetKeyDown(KeyCode.Alpha8)) addTrap(4, 1);

        if( Input.GetKeyDown(KeyCode.Q) )
        {
            openSelectTrap();
        }

        if( m_selectMode )
        {
            if ( m_trap1Count > 0 && Input.GetKeyDown(KeyCode.Alpha1) ) selectTrap(1);
            if ( m_trap2Count > 0 && Input.GetKeyDown(KeyCode.Alpha2) ) selectTrap(2);
            if ( m_trap3Count > 0 && Input.GetKeyDown(KeyCode.Alpha3) ) selectTrap(3);
            if ( m_trap4Count > 0 && Input.GetKeyDown(KeyCode.Alpha4) ) selectTrap(4);
        }

        if( m_previewTrap != null )
        {
            positionPreview();
        }

        if ( m_previewTrap != null && Input.GetMouseButtonDown(0) )
        {
            placeTrap();
        }

    }

    public void addTrap( int trapNumber, int numOfTraps )
    {
        switch( trapNumber )
        {
            case 1:
                m_trap1Count += numOfTraps;
                m_trap1CountText.text = "x" + m_trap1Count.ToString();
                m_background1.enabled = true;
                if( m_selectMode )
                {
                    m_keyPicture1.enabled = true;
                }
                break;

            case 2:
                m_trap2Count += numOfTraps;
                m_trap2CountText.text = "x" + m_trap2Count.ToString();
                m_background2.enabled = true;
                if (m_selectMode)
                {
                    m_keyPicture2.enabled = true;
                }
                break;

            case 3:
                m_trap3Count += numOfTraps;
                m_trap3CountText.text = "x" + m_trap3Count.ToString();
                m_background3.enabled = true;
                if (m_selectMode)
                {
                    m_keyPicture3.enabled = true;
                }
                break;

            case 4:
                m_trap4Count += numOfTraps;
                m_trap4CountText.text = "x" + m_trap4Count.ToString();
                m_background4.enabled = true;
                if (m_selectMode)
                {
                    m_keyPicture4.enabled = true;
                }
                break;
        }
    }

    public void removeTrap( int trapNumber )
    {
        switch( trapNumber )
        {
            case 1:
                m_trap1Count--;
                m_trap1CountText.text = m_trap1Count > 0 ? "x" + m_trap1Count.ToString() : "";
                if (m_trap1Count <= 0)
                {
                    m_background1.enabled = false;
                    m_keyPicture1.enabled = false;
                }
                break;

            case 2:
                m_trap2Count--;
                m_trap2CountText.text = m_trap2Count > 0 ? "x" + m_trap2Count.ToString() : "";
                if (m_trap2Count <= 0)
                {
                    m_background2.enabled = false;
                    m_keyPicture2.enabled = false;
                }
                break;

            case 3:
                m_trap3Count--;
                m_trap3CountText.text = m_trap3Count > 0 ? "x" + m_trap3Count.ToString() : "";
                if (m_trap3Count <= 0)
                {
                    m_background3.enabled = false;
                    m_keyPicture3.enabled = false;
                }
                break;

            case 4:
                m_trap4Count--;
                m_trap4CountText.text = m_trap4Count > 0 ? "x" + m_trap4Count.ToString() : "";
                if (m_trap4Count <= 0)
                {
                    m_background4.enabled = false;
                    m_keyPicture4.enabled = false;
                }
                break;

        }
    }

    private void openSelectTrap()
    {
        m_selectMode = !m_selectMode;

        m_shooting.enabled = !m_selectMode;

        if ( m_selectMode )
        {
            transform.localScale = Vector3.one * 1.25f;
            m_keyPicture1.enabled = m_trap1Count > 0 ? true : false;
            m_keyPicture2.enabled = m_trap2Count > 0 ? true : false;
            m_keyPicture3.enabled = m_trap3Count > 0 ? true : false;
            m_keyPicture4.enabled = m_trap4Count > 0 ? true : false;
        }
        else
        {
            transform.localScale = Vector3.one;
            m_keyPicture1.enabled = false;
            m_keyPicture2.enabled = false;
            m_keyPicture3.enabled = false;
            m_keyPicture4.enabled = false;
        }

    }

    private void selectTrap( int trapNumber )
    {
        switch (trapNumber)
        {
            case 1: m_selectedTrapPrefab = m_trap1Prefab; break;
            case 2: m_selectedTrapPrefab = m_trap2Prefab; break;
            case 3: m_selectedTrapPrefab = m_trap3Prefab; break;
            case 4: m_selectedTrapPrefab = m_trap4Prefab; break;
        }

        if (m_previewTrap != null) Destroy(m_previewTrap);

        m_previewTrap = Instantiate(m_selectedTrapPrefab);
        setPreview(m_previewTrap);
    }

    private void placeTrap()
    {
        Instantiate(m_selectedTrapPrefab, m_previewTrap.transform.position, Quaternion.identity);

        Destroy(m_previewTrap);
        m_previewTrap = null;

        if (m_selectedTrapPrefab == m_trap1Prefab) removeTrap(1);
        if (m_selectedTrapPrefab == m_trap2Prefab) removeTrap(2);
        if (m_selectedTrapPrefab == m_trap3Prefab) removeTrap(3);
        if (m_selectedTrapPrefab == m_trap4Prefab) removeTrap(4);

        m_selectedTrapPrefab = null;
    }

    private void positionPreview()
    {
        Ray ray = new Ray(m_playerCamera.position, m_playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, m_placeDistance))
        {
            // Now cast DOWN from that point to find ground
            RaycastHit groundHit;

            if (Physics.Raycast(hit.point + Vector3.up * 5f, Vector3.down, out groundHit, 10f))
            {
                m_previewTrap.transform.position = groundHit.point;
            }
        }
    }

    void setPreview( GameObject obj )
    {
        // Disable colliders
        Collider[] cols = obj.GetComponentsInChildren<Collider>();
        foreach (var col in cols)
            col.enabled = false;

        // Disable animators
        Animator[] animators = obj.GetComponentsInChildren<Animator>();
        foreach (var anim in animators)
            anim.enabled = false;

        // Disable AudioSources
        AudioSource[] audioSources = obj.GetComponentsInChildren<AudioSource>();
        foreach (var audio in audioSources)
            audio.enabled = false;
    }

}
