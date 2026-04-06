using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Trap")]
public class TrapSkill : Skill
{
    public int trapToAdd; 
    public bool operateIncrease;
    public bool damageIncrease;

    public override void apply()
    {
        if( trapToAdd != 0 )
        {
            TrapsUI.Instance.setTrapAllotment( trapToAdd, 1 );
        }
        else if( operateIncrease )
        {
            TrapsUI.Instance.setOperateUpgrade();
        }
        else if( damageIncrease )
        {
            TrapsUI.Instance.setDamageUpgrade();
        }
    }
}