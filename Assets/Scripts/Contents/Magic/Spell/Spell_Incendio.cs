using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[CreateAssetMenu(fileName = "Spell Incendio", menuName = "Scriptable Object/Spell Incendio")]
public class Spell_Incendio : Spell
{
    public GameObject m_IncendioPrefab;

    protected override bool AttempToCastSpellCondition(PlayerManager player)
    {
        return base.AttempToCastSpellCondition(player);
    }

    public override void SuccessfullyCastSpell(PlayerManager player)
    {
        base.SuccessfullyCastSpell(player);


        // Prefab 소환
        GameObject go = Managers.Resource.Instantiate(m_IncendioPrefab);
        var obj = go.GetComponent<MagicObjectBase>();
        obj.SetInfo(player, player.m_PlayerEquipmentManager.m_CurrentMagicEquippment.m_EquipmentEdge_SpawnTransform);
    }
}
