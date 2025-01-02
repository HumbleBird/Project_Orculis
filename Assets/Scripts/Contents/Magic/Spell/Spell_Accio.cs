using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// 지정한 방향에 있는 물건을 끌어들임
/// 지속성 마법
/// </summary>

[CreateAssetMenu(fileName = "Spell Accio", menuName = "Scriptable Object/Spell Accio")]
public class Spell_Accio : Spell
{
    public DrainManaEffect m_DrainManaEffect;
    XRBaseInteractable m_obj;

    public override bool AttempToCastSpell(PlayerManager player)
    {
        if (base.AttempToCastSpell(player) == false)
            return false;

        if (player.m_PlayerMagicManager.m_bIsSelectObject == true)
            return false;

        var list = player.m_RightHandLearFarInteractor.interactablesHovered;
        if (list.Count <= 0)
            return false;

        // 마법으로 상호작용이 가능한가?
        m_obj = list[0] as XRBaseInteractable;
        var magicObj = m_obj.transform.GetComponent<MagicObjectBase>();

        if (magicObj == null && magicObj.CanControlMagicObject() == false)
            return false;
        else
        {
            SuccessfullyCastSpell(player);
            return true;
        }
    }

    public override void SuccessfullyCastSpell(PlayerManager player)
    {
        base.SuccessfullyCastSpell(player);

        // 현재 지속중인 마법 
        player.m_PlayerMagicManager.m_UsingSpells.Add(this);

        player.m_RightHandInteractableObject = m_obj;

        player.m_RightHandLearFarInteractor.interactionManager.SelectEnter(
            (IXRSelectInteractor)player.m_RightHandLearFarInteractor,
            (IXRSelectInteractable)player.m_RightHandInteractableObject);

        player.m_PlayerMagicManager.m_bIsSelectObject = true;

        // Drain Mana
        m_DrainManaEffect.m_iEffectType = Define.E_CharacterEffectType.DrainMana;
        player.m_PlayerEffectsManager.timedEffects.Add(m_DrainManaEffect);
    }

    public override void FailCastSpell(PlayerManager player)
    {
        base.FailCastSpell(player);

        player.m_PlayerMagicManager.ReleaseInteractingObject();
    }
}
