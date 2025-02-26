using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static Define;

[CreateAssetMenu(fileName = "Spell Base", menuName = "Scriptable Object/Spell Base", order = 0)]
public abstract class SpellBase : ScriptableObject
{
    [Header("Ref")]
    public Player m_Owner;

    [Header("Property")]
    public string spellName;
    [SerializeField] protected ushort m_Cost;
    public E_SpellActivation m_ESpellActivation;
    public E_SpellType m_SpellType;
    public float m_fCoolTime;
    public string m_sTagline;
    public string m_sDetailDescription;
    public Sprite m_image;

    [Header("Flag")]
    public bool m_bIsChant = false;
    public float m_fChantConditionTime = 2f;
    public bool m_bIsMotion = false;
    public float m_fMotionConditionTime = 2f;

    [Header("Audio Clip")]
    [SerializeField] protected AudioClip m_SpellAttempAudioClip;
    [SerializeField] protected AudioClip m_SpellSuccessAudioClip;
    [SerializeField] protected AudioClip m_SpellFailAudioClip;

    [Header("Camera")]
    [SerializeField] protected float m_fPowerCameraShake;

    // 음성과 동작 두 가지 조건을 모두 만족 했을 때 시도

    public virtual IEnumerator AchieveChantFlag()
    {
        if (ChantCondition() == false)
            yield break;

        m_bIsChant = true;

        yield return new WaitForSeconds(m_fChantConditionTime);
        m_bIsChant = false;

        FailHalfwayChant();
    }

    protected virtual bool ChantCondition() { return true; }
    protected virtual void FailHalfwayChant() { }

    public IEnumerator AchieveMotionFlag()
    {
        if (MotionCondition() == false)
            yield break;

        m_bIsMotion = true;

        yield return new WaitForSeconds(m_fMotionConditionTime);
        m_bIsMotion = false;
    }

    protected virtual bool MotionCondition() { return true; }

    public void AttempToCastSpell()
    {
        // Equipment Light

        // 각자의 스펠 조건
        bool m_CanAttempToCastSpell = AttempToCastSpellCondition();

        if (m_CanAttempToCastSpell == true)
            SuccessfullyCastSpell();
        else
            FailCastSpell();
    }

    protected virtual bool AttempToCastSpellCondition()
    {
        // Check Mana
        if (m_Owner.m_PlayerStatesManager.HasEnoughMana(m_Cost) == false)
            return false;

        return true;
    }

    public virtual void SuccessfullyCastSpell()
    {
        // Deduct Mana
        m_Owner.m_PlayerStatesManager.UseManaForSkill(m_Cost); 

        if (m_ESpellActivation == E_SpellActivation.Continuous)
            m_Owner.m_PlayerMagicManager.m_UsingSpells.Add(this);

        // Sound
        Managers.Sound.Play(m_SpellSuccessAudioClip);

        // 화면 이펙트
        // PostProceeing?

        // Camera Shake
        m_Owner.m_StressReceiver.InduceStress(m_fPowerCameraShake);

        // 스탬프 Light
    }

    public virtual void FailCastSpell()
    {
        // Sound
        Managers.Sound.Play(m_SpellFailAudioClip);

        if(m_ESpellActivation == E_SpellActivation.Continuous)
        {
            m_Owner.m_PlayerMagicManager.m_UsingSpells.Remove(this);
        }
    }
}
