using UnityEngine;

[CreateAssetMenu(fileName = "Spell Incendio", menuName = "Scriptable Object/Spell Incendio")]
public class Spell_Incendio : Spell
{
    public GameObject m_IncendioPrefab;

    public override bool AttempToCastSpell(PlayerManager player)
    {
        if (base.AttempToCastSpell(player) == false)
            return false;

        SuccessfullyCastSpell(player);

        return true;
    }

    public override void SuccessfullyCastSpell(PlayerManager player)
    {
        base.SuccessfullyCastSpell(player);

        // Prefab 소환
        GameObject go = Managers.Resource.Instantiate(m_IncendioPrefab);
        var obj = go.GetComponent<MagicObjectBase>();
        obj.SetInfo(player, player.m_CurrentMagicEquippment.m_MagicSpawnTransform);
    }
}
