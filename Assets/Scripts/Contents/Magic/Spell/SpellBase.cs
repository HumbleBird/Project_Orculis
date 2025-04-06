using Fusion;
using FusionHelpers;
using Oculus.Interaction.PoseDetection.Debug;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using static Define;


public abstract class SpellBase : NetworkBehaviour
{
    protected Player m_Owner;

    [Header("Base Property")]
    [SerializeField] protected ushort m_Cost;
    public E_SpellActivation m_ESpellActivation;
    public E_SpellType m_SpellType;
    [SerializeField] protected float m_fPowerCameraShake;
    public int m_iDamage = 0;

    [Header("Visuals")]
    public Sprite m_icon;
    public string spellName;
    public VideoClip m_UseVideoClip;
    public string m_sDetailDescription;
    public string m_sConditionDescription;

    [Header("CoolTime Property")]
    public float m_fCoolTime { private set; get; } = 10f;
    private float m_fLastCastTime = -Mathf.Infinity;  // 마지막 사용 시간
    [Networked]
    private TickTimer m_CoolTime { get; set; }

    // 스킬 사용 가능 여부
    public bool m_bIsEndCooltime => Time.time >= m_fLastCastTime + m_fCoolTime;

    // 쿨타임 진행률 (0 ~ 1)
    // 0 쿨타임 시작, 1 쿨타임 완료
    public float m_CooldownProgress => Mathf.Clamp01((Time.time - m_fLastCastTime) / m_fCoolTime); 

    // 남은 쿨타임 시간
    public float m_CooldownRemain => Mathf.Max(0, (m_fLastCastTime + m_fCoolTime) - Time.time); 


    [Header("Flag")]
    public bool m_bIsChant = false;
    public float m_fChantConditionTime = 2f;
    public bool m_bIsMotion = false;
    public float m_fMotionConditionTime = 2f;
    public bool m_bIsClearAllCondition => m_bIsChant && m_bIsMotion;

    [Header("Audio Clip")]
    [SerializeField] protected AudioClip m_SpellAttempAudioClip;
    [SerializeField] protected AudioClip m_SpellSuccessAudioClip;
    [SerializeField] protected AudioClip m_SpellFailAudioClip;

    public virtual void Awake()
    {
        m_Owner =GetComponentInParent<Player>();
    }

    public virtual void Start()
    {
    }

    public virtual void Update()
    {
    }



    #region 1. Base Conditoin

    // 기본 음성 조건을 만족했을 경우
    public virtual IEnumerator AchieveChantFlag()
    {
        // 스펠 음성 조건을 만족 못 했을 경우
        if (ChantCondition() == false)
            yield break;

        m_bIsChant = true;

        Debug.Log($"{spellName}의 음성 조건 {m_bIsChant}");

        yield return new WaitForSeconds(m_fChantConditionTime);

        // 모션 조건을 만족하지 못 했을 경우
        if (m_bIsClearAllCondition == false)
        {
            m_bIsChant = false;
            Debug.Log($"{spellName}의 음성 조건 {m_bIsChant}");

            FailHalfwayChant();
        }
    }

    public virtual bool ChantCondition() { return true; }
    protected virtual void FailHalfwayChant() { }

    public IEnumerator AchieveMotionFlag()
    {
        if (MotionCondition() == false)
            yield break;

        m_bIsMotion = true;
        Debug.Log($"{spellName}의 동작 조건 {m_bIsMotion}");

        yield return new WaitForSeconds(m_fMotionConditionTime);
        
        // 모션 조건을 만족하지 못 했을 경우
        if (m_bIsClearAllCondition == false)
        {
            m_bIsMotion = false;
            Debug.Log($"{spellName}의 동작 조건 {m_bIsMotion}");

        }
    }

    protected virtual bool MotionCondition() { return true; }

    #endregion

    #region 2. Spell Condition

    public void AttempToCastSpell()
    {
        // Equipment Light

        // 각자의 스펠 조건

        if (AttempToCastSpellCondition() == true)
            SuccessfullyCastSpell();
        else
            FailCastSpell();
    }




    #endregion

    #region 3. Spell Step by Step

    protected virtual bool AttempToCastSpellCondition()
    {
        if (CheckMana() == false)
            return false;

        //if (m_CoolTime.ExpiredOrNotRunning(Runner) == false)
        //    return false;

        return true;
    }

    public virtual void SuccessfullyCastSpell()
    {
        // Deduct Mana
        m_Owner.m_PlayerStatesManager.UseManaForSkill(m_Cost); 

        if (m_ESpellActivation == E_SpellActivation.Continuous)
            m_Owner.m_PlayerMagicManager.m_UsingSpells.Add(this);

        // Sound
        //Managers.Sound.Play(m_SpellSuccessAudioClip);

        // 화면 이펙트
        // PostProceeing?

        // Camera Shake
        //m_Owner.m_StressReceiver.InduceStress(m_fPowerCameraShake);

        // 스탬프 Light

        // 마법 조건 clear
        ClearCondition();
    }

    public virtual void FailCastSpell()
    {
        // Sound
        Managers.Sound.Play(m_SpellFailAudioClip);

        if(m_ESpellActivation == E_SpellActivation.Continuous)
        {
            m_Owner.m_PlayerMagicManager.m_UsingSpells.Remove(this);
        }

        ClearCondition();
    }

    #endregion

    #region Flag & Conditions

    private void ClearCondition()
    {
        m_bIsChant = m_bIsChant = false;
    }

    private bool CheckMana()
    {
        if (m_Owner.m_PlayerStatesManager.HasEnoughMana(m_Cost) == false)
            return false;

        return true;
    }

    #endregion

}
