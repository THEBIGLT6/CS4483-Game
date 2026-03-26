using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class SkillTreeUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private GameObject m_upgradeScreen;
    [SerializeField] private GameObject m_hud;
    [SerializeField] private TMP_Text m_balanceText;
    [SerializeField] private Transform m_weaponColumn;
    [SerializeField] private Transform m_trapColumn;
    [SerializeField] private Transform m_playerColumn;
    [SerializeField] private TMP_Text m_insufficientFunds;

    [Header("Shooting")]
    [SerializeField] private RaycastShooting m_raycastShooting;

    [Header("Skill Tree Nodes")]
    [SerializeField] private GameObject m_skillNodePrefab;
    private List<SkillTreeNode> m_skillTreeNodes = new List<SkillTreeNode>();

    void Start()
    {
        CreateUI();
    }

    void CreateUI()
    {
        Dictionary<SkillType, List<Skill >> skills = SkillTreeManager.Instance.getSkills();
        
        foreach (var pair in skills)
        {
            foreach (Skill skill in pair.Value)
            {
                Transform parent = GetColumn(skill.skillType);
                GameObject obj = Instantiate(m_skillNodePrefab, parent);

                SkillTreeNode node = obj.GetComponent<SkillTreeNode>();
                m_skillTreeNodes.Add( node );

                node.setSkill( skill, this );
            }
        }
    }

    public void toggleUpgradeScreen( bool setActive )
    {
        m_insufficientFunds.gameObject.SetActive( false );
        m_hud.SetActive( !setActive );
        m_upgradeScreen.SetActive( setActive );
        m_raycastShooting.enabled = !setActive;

        m_balanceText.text = $"Balance: $ {GameManager.Instance.getMoney().ToString("D4")}";
        
        Cursor.lockState = setActive ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = setActive;
        Time.timeScale = setActive ? 0f : 1f;

        if ( !setActive ) ZombieSpawner.Instance.openStartRound();
    }

    private Transform GetColumn(SkillType type)
    {
        switch (type)
        {
            case SkillType.Weapon: return m_weaponColumn;
            case SkillType.Trap: return m_trapColumn;
            case SkillType.Player: return m_playerColumn;
        }
        return null;
    }

    public void refreshAllNodes()
    {
        foreach (SkillTreeNode node in m_skillTreeNodes)
        {
            node.refresh();
        }

        m_balanceText.text = $"Balance: $ {GameManager.Instance.getMoney().ToString("D4")}";
    }

    public void insufficientFunds()
    {
        StopAllCoroutines();
        StartCoroutine(showInsufficientFunds());
    }

    private IEnumerator showInsufficientFunds()
    {
        m_insufficientFunds.gameObject.SetActive(true);

        float fadeDuration = 0.5f;
        float visibleDuration = 2f;

        // Start invisible
        SetAlpha(0f);

        // Fade In
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            SetAlpha(t / fadeDuration);
            yield return null;
        }

        SetAlpha(1f);

        // Stay visible
        yield return new WaitForSecondsRealtime(visibleDuration);

        // Fade Out
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            SetAlpha(1f - (t / fadeDuration));
            yield return null;
        }

        SetAlpha(0f);
        m_insufficientFunds.gameObject.SetActive(false);
    }

    private void SetAlpha(float alpha)
    {
        Color c = m_insufficientFunds.color;
        c.a = alpha;
        m_insufficientFunds.color = c;
    }
}
