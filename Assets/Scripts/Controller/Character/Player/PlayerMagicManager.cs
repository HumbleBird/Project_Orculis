using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static Define;


// 마법 주문 동작 원리
// 1차 조건 : 동작, 영창 
// 2차 조건 : 기본 조건 (마나량, 목표물, 환경 등)
// 실패 시 -> 실패 패널티
// 성공 시 -> 주문 발동
public class PlayerMagicManager : MonoBehaviour
{
    [Header("Ref")]
    PlayerManager m_PlayerManager;

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

    public List<Spell> m_lockSpells = new List<Spell>();
    public List<Spell> m_UnlockSpells = new List<Spell>();
    public List<Spell> m_UsingSpells = new List<Spell>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_PlayerManager = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // 숫자 키 입력 처리 (Alpha1 ~ Alpha4)
        for (int i = 0; i < m_UnlockSpells.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                // spells[i]가 존재할 경우 처리
                if (m_UnlockSpells[i] != null)
                {
                    m_UnlockSpells[i].SuccessfullyCastSpell(m_PlayerManager);
                }
                else
                {
                    Debug.LogWarning($"Spell {i} is not assigned.");
                }
            }
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
    
    public void MagicObjectTrow(GameObject prefab, float power, ForceMode mode)
    {
        var obj =  prefab.GetComponent<Rigidbody>();
        obj.AddForce(m_PlayerManager.m_PlayerEquipmentManager.m_CurrentMagicEquippment.m_EquipmentEdge_SpawnTransform.forward* power);
    }

    public void SpellFlagCheck(E_SpellCheckType type, string spellName)
    {
        if(type == E_SpellCheckType.Chant)
        {
            if (m_dicChatingMagicSpell.ContainsKey(spellName))
            {
                m_dicChatingMagicSpell[spellName] = true;

                Debug.Log($"Chanting Magic Spell Bool Change : {spellName} - True");

                StartCoroutine(DelayChangeFlag(type, spellName));

                // Show UI
                m_PlayerManager.m_MagicTryResultUI.ShowUIMagicAttemptResult(true);
            }
            else
            {
                // Show UI
                m_PlayerManager.m_MagicTryResultUI.ShowUIMagicAttemptResult(false);
            }
        }

        else if (type == E_SpellCheckType.Motion)
        {
            if (m_dicMotionMagicSpell.ContainsKey(spellName))
            {
                m_dicMotionMagicSpell[spellName] = true;

                Debug.Log($"Motion Magic Spell Bool Change : {spellName} - True");

                StartCoroutine(DelayChangeFlag(type, spellName));
            }
        }

        // Attemp Spell
        if (m_dicMotionMagicSpell[spellName] == true && m_dicChatingMagicSpell[spellName] == true)
        {
            AttempSpell(spellName);
        }
    }

    public void AttempSpell(string spellName)
    {
        Spell spell = m_UnlockSpells.Find(s => s.name == spellName);
        if (spell != null)
        {
            spell.AttempToCastSpell(m_PlayerManager);
        }
        else
        {
            Debug.LogWarning($"Spell not found: {spellName}");
        }
    }

    public void SimpleAttempSpell()
    {
        bool canUse = m_PlayerManager.m_DevelopUI.m_Toggle.isOn;

        int currentSpell = m_PlayerManager.m_DevelopUI.dropDown.value;
        Spell spell = m_UnlockSpells[currentSpell];


        if (spell != null && canUse)
        {
            spell.AttempToCastSpell(m_PlayerManager);
        }
        else
        {
            Debug.LogWarning($"Spell not found: {spell.name}");
        }
    }

    IEnumerator DelayChangeFlag(E_SpellCheckType type, string spellName)
    {
        yield return new WaitForSeconds(m_bIsSpellDelayFlagTime);

        if(type == E_SpellCheckType.Chant)
        {
            m_dicMotionMagicSpell[spellName] = false;

            Debug.Log($"Chant Magic Spell Bool Change : {spellName} - false");
        }
        else if (type == E_SpellCheckType.Chant)
        {
            m_dicMotionMagicSpell[spellName] = false;

            Debug.Log($"Motion Magic Spell Bool Change : {spellName} - false");
        }
    }
}
