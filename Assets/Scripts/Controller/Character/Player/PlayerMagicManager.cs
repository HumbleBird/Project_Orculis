using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static Define;


// 마법 주문 동작 원리
// 1차 조건 : 동작, 영창 
// 2차 조건 : 기본 조건 (마나량, 목표물, 환경 등)
// 실패 시 -> 실패 패널티
// 성공 시 -> 주문 발동
public class PlayerMagicManager : NetworkBehaviour
{
    [Header("Ref")]
    Player m_PlayerManager;

    //[Header("Spell Property")]
    //public float m_fTimeReduceSkillCooltime = 1f;

    [Header("Flag")]
    [SerializeField] public bool m_bIsSelectObject { get; set; }
    [SerializeField] public float m_bIsSpellDelayFlagTime = 1.5f;

    [Header("Resources")]
    // 마법 동작 Flag
    public Dictionary<string, bool> m_dicMotionMagicSpell = new Dictionary<string, bool>()
    { 
        {"Accio", false },
        {"Depulso", false }, 
        {"Incendio", false }, 
    };

    // 주문 영창 Flag
    public Dictionary<string, bool> m_dicChatingMagicSpell = new Dictionary<string, bool>()
    {
        {"Accio", false },
        {"Depulso", false },
        {"Incendio", false },
    };


    [Networked]
    [Capacity(32)]
    [UnitySerializeField]
    NetworkLinkedList<SpellBase> m_UnlockSpells { get; }

    public List<SpellBase> m_TempSpells = new List<SpellBase>();

    //[Networked]
    public List<SpellBase> m_UsingSpells { get; set; } = new List<SpellBase>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Spawned()
    {
        m_PlayerManager = GetComponent<Player>();
        GetComponentsInChildren<SpellBase>().Select((spell, i) => m_UnlockSpells.Set(i, spell)).ToList();

        m_TempSpells = GetComponentsInChildren<SpellBase>().ToList();
    }

    // Update is called once per frame
    public void Editor_SuccessTrySpell(int count)
    {
        // spells[i]가 존재할 경우 처리
        if (m_UnlockSpells.Count > count &&  m_UnlockSpells[count] != null)
        {
            m_UnlockSpells[count].ChantCondition();
            m_UnlockSpells[count].SuccessfullyCastSpell();
        }
        else
        {
            Debug.LogWarning($"Spell {count} is not assigned.");
        }
    }

    public void ReleaseInteractingObject()
    {
        m_PlayerManager.m_RightHandLearFarInteractor.interactionManager.SelectExit(
            (IXRSelectInteractor)m_PlayerManager.m_RightHandLearFarInteractor,
            (IXRSelectInteractable)m_PlayerManager.m_RightHandInteractableObject);

        m_PlayerManager.m_PlayerMagicManager.m_bIsSelectObject = false;

        m_PlayerManager.m_RightHandInteractableObject = null;
    }

    public void SpellFlagCheck(E_SpellCheckType type, string spellName)
    {
        SpellBase s = m_UsingSpells.FirstOrDefault(spell => spell.spellName == spellName);

        if (s == null)
            return;

        if(type == E_SpellCheckType.Chant)
            StartCoroutine(s.AchieveChantFlag());
        else
            StartCoroutine(s.AchieveMotionFlag());

        if (s.m_bIsMotion & s.m_bIsChant)
            s.AttempToCastSpell();
    }

    public void AttempSpell(string spellName)
    {
        SpellBase spell = m_UnlockSpells.FirstOrDefault(s => s.name == spellName);
        if (spell != null)
        {
            spell.AttempToCastSpell();
        }
        else
        {
            Debug.LogWarning($"Spell not found: {spellName}");
        }
    }

    public List<SpellBase> UnlockSpellGet()
    {
        return m_TempSpells.ToList<SpellBase>();
    }

}
