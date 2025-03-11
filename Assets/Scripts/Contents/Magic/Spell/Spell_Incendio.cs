using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[CreateAssetMenu(fileName = "Spell Incendio", menuName = "Scriptable Object/Spell Incendio")]
public class Spell_Incendio : SpellBase
{
    public GameObject m_IncendioPrefab;

    protected override bool AttempToCastSpellCondition()
    {
        return base.AttempToCastSpellCondition();
    }

    public override void SuccessfullyCastSpell()
    {
        base.SuccessfullyCastSpell();

        // Prefab 소환
        Transform tr = m_Owner.m_PlayerEquipmentManager.m_CurrentWeapon.m_EquipmentEdge_SpawnTransform;

        var t = m_IncendioPrefab.GetComponent<MagicObjectBase>();
        t.m_Owner = m_Owner;
        m_Owner.m_PlayerMagicManager.NetworkSpawnMagicObject(m_IncendioPrefab.gameObject, tr);
    }
}
