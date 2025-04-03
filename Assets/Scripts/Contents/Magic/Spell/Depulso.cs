using SimpleFPS;
using Unity.XR.CoreUtils;
using UnityEngine;

/// <summary>
/// 물건과 적을 밀어냄. 밀어낸 적들끼리 부딪혀 피해를 입을 수 있음. 퍼즐을 풀 때도 유용
/// </summary>

public class Depulso : SpellBase
{
    [SerializeField] private float m_fAddForece = 50;
    [SerializeField] private Accio m_Spell_Accio;

    protected override bool AttempToCastSpellCondition()
    {
        return base.AttempToCastSpellCondition();
    }

    public override void SuccessfullyCastSpell()
    {
        base.SuccessfullyCastSpell();

        // Acio를 통해 부유 중인 물체가 있다면
        if(m_Owner.m_PlayerMagicManager.m_bIsSelectObject)
        { 
            MagicObjectTrow(
                m_Owner.m_RightHandInteractableObject.gameObject,
                m_fAddForece,
                ForceMode.Impulse);

            // DrainMana 제거
            m_Owner.m_PlayerEffectsManager.timedEffects.Remove(m_Spell_Accio.m_DrainManaEffect);
            m_Owner.m_PlayerMagicManager.m_UsingSpells.Remove(m_Spell_Accio);

            // MagicThrow 뒤에 있어야 함.
            m_Owner.m_PlayerMagicManager.ReleaseInteractingObject();
        }
        else
        {
            // 앞에 있는 물건을 감지해서 날려버림.
        }
    }

    public void MagicObjectTrow(GameObject prefab, float power, ForceMode mode)
    {
        MagicMovableBox obj = prefab.GetComponent<MagicMovableBox>();

        // Set Info
        obj.m_iDamage = m_iDamage;
        obj.m_Owner = m_Owner;
        obj.m_bIsAttackable = true;

        // RigidBody
        obj.m_Rigidbody.AddForce(m_Owner.m_PlayerEquipmentManager.m_CurrentWeapon.m_MuzzleTransform.forward * power);
    }

}
