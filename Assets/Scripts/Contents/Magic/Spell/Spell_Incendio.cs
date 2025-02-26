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
        GameObject go = Managers.Resource.Instantiate(m_IncendioPrefab);
        var obj = go.GetComponent<MagicObjectBase>();
        obj.SetInfo(m_Owner, m_Owner.m_PlayerEquipmentManager.m_CurrentWeapon.m_MagicEquippmentObject.m_EquipmentEdge_SpawnTransform);
    }
}
