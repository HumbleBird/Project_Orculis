using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 지속적으로 현재 마나를 줄임
/// 마나가 0이하가 된다면 현재 지속중인 마법들을 전부 해제함
/// </summary>

[CreateAssetMenu(menuName = "Character Effects/Mana Drain")]
public class DrainManaEffect : CharacterEffect
{
    // 틱 당 없어지는 마나 양
    public ushort m_fDrainManaAmountTick = 1;

    public override void ProccessEffect(PlayerManager player)
    {
        // 현재 플레이어가 마나가 0보다 많은가?
        if(player.m_PlayerStatesManager.m_CurrentMana <= 0)
        {
            player.m_PlayerEffectsManager.timedEffects.Remove(this);

            // 만약 현재 영창 지속중인 상태라면 해제
            // ex) 무기에 마나를 이용해 지속적인 강화
            // ex) 마나를 이용해 물건을 들고 있는 상태라면

            var spells = player.m_PlayerMagicManager.m_UsingSpells;
            foreach (var spell in spells)
                spell.FailCastSpell(player);
        }
        else
        {
            player.m_PlayerStatesManager.ChangeMana(-m_fDrainManaAmountTick);
        }
    }
}