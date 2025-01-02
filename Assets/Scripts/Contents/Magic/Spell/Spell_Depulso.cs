using UnityEngine;

/// <summary>
/// 물건과 적을 밀어냄. 밀어낸 적들끼리 부딪혀 피해를 입을 수 있음. 퍼즐을 풀 때도 유용
/// </summary>

[CreateAssetMenu(fileName = "Spell Depulso", menuName = "Scriptable Object/Spell Depulso")]
public class Spell_Depulso : Spell
{
    [SerializeField] private float m_fAddForece = 50;
    [SerializeField] private Spell_Accio m_Spell_Accio;

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

        // Acio를 통해 부유 중인 물체가 있다면
        if(player.m_PlayerMagicManager.m_bIsSelectObject)
        {
            player.m_PlayerMagicManager.ReleaseInteractingObject();

            player.m_PlayerMagicManager.MagicObjectTrow(
                player.m_RightHandInteractableObject.gameObject,
                player.m_CurrentMagicEquippment.m_MagicSpawnTransform.forward,
                m_fAddForece,
                ForceMode.Impulse);

            // DrainMana 제거
            player.m_PlayerEffectsManager.timedEffects.Remove(m_Spell_Accio.m_DrainManaEffect);
            player.m_PlayerMagicManager.m_UsingSpells.Remove(m_Spell_Accio);
        }
        else
        {
            // 앞에 있는 물건을 감지해서 날려버림.
        }
    }
}
