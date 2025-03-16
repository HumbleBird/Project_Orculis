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

    public List<SpellBase> m_lockSpells = new List<SpellBase>();
    public List<SpellBase> m_UnlockSpells = new List<SpellBase>();
    public List<SpellBase> m_UsingSpells = new List<SpellBase>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Spawned()
    {
        m_PlayerManager = GetComponent<Player>();

        foreach (var spell in m_UnlockSpells)
            spell.m_Owner = m_PlayerManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (HasInputAuthority == false)
            return;

        // 숫자 키 입력 처리 (Alpha1 ~ Alpha4)
        for (int i = 0; i < m_UnlockSpells.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                // spells[i]가 존재할 경우 처리
                if (m_UnlockSpells[i] != null)
                {
                    m_UnlockSpells[i].ChantCondition();
                    m_UnlockSpells[i].SuccessfullyCastSpell();
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
        obj.AddForce(m_PlayerManager.m_PlayerEquipmentManager.m_CurrentWeapon.m_EquipmentEdge_SpawnTransform.forward* power);
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
        SpellBase spell = m_UnlockSpells.Find(s => s.name == spellName);
        if (spell != null)
        {
            spell.AttempToCastSpell();
        }
        else
        {
            Debug.LogWarning($"Spell not found: {spellName}");
        }
    }

    public void NetworkSpawnMagicObject(GameObject obj, Transform trans)
    {
        Runner.Spawn(obj, trans.position, trans.rotation);
    }

}
