using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// 지정한 방향에 있는 물건을 끌어들임
/// 지속성 마법
/// </summary>

[CreateAssetMenu(fileName = "Spell Accio", menuName = "Scriptable Object/Spell Accio")]
public class Accio : SpellBase
{
    public DrainManaEffect m_DrainManaEffect;
    XRBaseInteractable m_obj;

    // 조건 달성 중간 실패시
    protected override void FailHalfwayChant()
    {
        m_obj = null;
    }

    public override bool ChantCondition()
    {
        // 마법으로 상호작용이 가능한가?
        var list = m_Owner.m_RightHandLearFarInteractor.interactablesHovered;
        if (list.Count <= 0)
            return false;
        
        m_obj = list[0] as XRBaseInteractable;
        var magicObj = m_obj.transform.GetComponent<MagicMovableBox>();

        if (magicObj == null || magicObj.CanControlMagicObject() == false)
            return false;

        return true;
    }

    protected override bool AttempToCastSpellCondition()
    {
        if (base.AttempToCastSpellCondition() == false)
            return false;

        if (m_Owner.m_PlayerMagicManager.m_bIsSelectObject == true)
            return false;

        return true;
    }

    public override void SuccessfullyCastSpell()
    {
        base.SuccessfullyCastSpell();

        // 현재 지속중인 마법 
        m_Owner.m_PlayerMagicManager.m_UsingSpells.Add(this);

        m_Owner.m_RightHandInteractableObject = m_obj;

        m_Owner.m_RightHandLearFarInteractor.interactionManager.SelectEnter( 
            (IXRSelectInteractor)m_Owner.m_RightHandLearFarInteractor,
            (IXRSelectInteractable)m_Owner.m_RightHandInteractableObject);

        m_Owner.m_PlayerMagicManager.m_bIsSelectObject = true;

        // Drain Mana
        m_DrainManaEffect.m_iEffectType = Define.E_CharacterEffectType.DrainMana;
        m_Owner.m_PlayerEffectsManager.timedEffects.Add(m_DrainManaEffect);

        // TODO
        // 광선 연결 후 선으로 끌어 당기기
    }

    public override void FailCastSpell()
    {
        base.FailCastSpell();

        if(m_Owner.m_PlayerMagicManager.m_bIsSelectObject == true)
        {
            m_Owner.m_PlayerMagicManager.ReleaseInteractingObject();
        }
    }
}
