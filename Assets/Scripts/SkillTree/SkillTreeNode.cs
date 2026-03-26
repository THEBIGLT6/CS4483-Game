using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeNode : MonoBehaviour
{
    [SerializeField] private TMP_Text m_skillNameCost;
    [SerializeField] private TMP_Text m_desciption;
    [SerializeField] private Button m_button;
    [SerializeField] private Image m_background;
    [SerializeField] private Image m_border;

    private Skill m_skill;
    private SkillTreeUI m_skillTreeUI;

    public void setSkill( Skill skill, SkillTreeUI skillTreeUI )
    {
        m_skillTreeUI = skillTreeUI;
        m_skill = skill;
        refresh();

        m_skillNameCost.text = $"{skill.skillName} - ${skill.cost}";
        m_desciption.text = skill.description;

        m_button.onClick.AddListener(onClicked);
    }

    public void onClicked()
    { 
        if( m_skill.cost <= GameManager.Instance.getMoney() )
        {
            GameManager.Instance.subtractMoney( m_skill.cost );
            SkillTreeManager.Instance.unlockSkill( m_skill );
            m_skillTreeUI.refreshAllNodes();
        }
        else
        {
            m_skillTreeUI.insufficientFunds();
        }
    }

    public void refresh()
    {
        bool isUnlocked = SkillTreeManager.Instance.isUnlocked( m_skill );
        bool canUnlock = SkillTreeManager.Instance.canUnlock( m_skill );

        if (isUnlocked)
        {
            m_border.color = Color.green;
            m_background.color = Color.white;
            m_button.interactable = false;
        }
        else if (canUnlock)
        {
            m_border.color = Color.white;
            m_background.color = Color.white;
            m_button.interactable = true;
        }
        else
        {
            m_border.color = new Color(0.6f, 0.6f, 0.6f);
            m_background.color = new Color(0.6f, 0.6f, 0.6f);
            m_button.interactable = false;
        }
    }
}
