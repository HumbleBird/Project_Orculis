using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using static UnityEditor.Experimental.GraphView.GraphView;

[CreateAssetMenu(fileName = "Spell Base", menuName = "Scriptable Object/Spell Base", order = 0)]
public abstract class Spell : ScriptableObject
{
    [Header("Property")]
    [SerializeField] protected string spellName;
    [SerializeField] protected ushort m_Cost;
    public E_SpellActivation m_ESpellActivation;
    public E_SpellType m_SpellType;
    public float m_fCoolTime;
    public string m_sTagline;
    public string m_sDetailDescription;
    public Sprite m_image;

    [Header("Audio Clip")]
    [SerializeField] protected AudioClip m_SpellAttempAudioClip;
    [SerializeField] protected AudioClip m_SpellSuccessAudioClip;
    [SerializeField] protected AudioClip m_SpellFailAudioClip;

    [Header("Camera")]
    [SerializeField] protected float m_fPowerCameraShake;

    // 음성과 동작 두 가지 조건을 모두 만족 했을 때 시도

    public void AttempToCastSpell(PlayerManager player)
    {
        // Equipment Light

        // 각자의 스펠 조건
        bool m_CanAttempToCastSpell = AttempToCastSpellCondition(player);

        if (m_CanAttempToCastSpell == true)
        {
            SuccessfullyCastSpell(player);
        }
        else
        {
            FailCastSpell(player);
        }
    }

    protected virtual bool AttempToCastSpellCondition(PlayerManager player)
    {
        // Check Mana
        if (player.m_PlayerStatesManager.HasEnoughMana(m_Cost) == false)
            return false;

        return true;
    }

    public virtual void SuccessfullyCastSpell(PlayerManager player)
    {
        // Deduct Mana
        player.m_PlayerStatesManager.UseManaForSkill(m_Cost);

        if (m_ESpellActivation == E_SpellActivation.Continuous)
            player.m_PlayerMagicManager.m_UsingSpells.Add(this);

        // Sound
        Managers.Sound.Play(m_SpellSuccessAudioClip);

        // 화면 이펙트
        // PostProceeing?

        // Camera Shake
        player.m_StressReceiver.InduceStress(m_fPowerCameraShake);

        // 스탬프 Light
    }

    public virtual void FailCastSpell(PlayerManager player)
    {
        // Sound
        Managers.Sound.Play(m_SpellFailAudioClip);

        if(m_ESpellActivation == E_SpellActivation.Continuous)
        {
            player.m_PlayerMagicManager.m_UsingSpells.Remove(this);
        }
    }
}
