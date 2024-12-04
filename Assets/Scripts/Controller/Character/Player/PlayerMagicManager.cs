using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PlayerMagicManager : MonoBehaviour
{
    [Header("Ref")]
    PlayerManager m_PlayerManager;

    [Header("Spell Common")]
    [SerializeField] AudioClip m_SuccessAudioClip;
    [SerializeField] AudioClip m_FailAudioClip;

    [Header("Spell_Accio & Repulsio")]
    [SerializeField] private ushort m_iAccioCost;
    [SerializeField] private float m_fRepellcioAddForece = 50;
    [SerializeField] AudioClip m_AccioSuccessAudioClip;
    [SerializeField] AudioClip m_AccioFailAudioClip;
    [SerializeField] AudioClip m_AccioRepullsioAudioClip;
    // PlayerManager의 m_RightHandInteractableObject

    [Header("Spell_Incendio")]
    [SerializeField] private GameObject m_IncendioPrefab;
    [SerializeField] private ushort m_iIncendioCost;

    [Header("Spell_Volate Ascendere")]
    [SerializeField] private ushort m_iVolateAscendereCost;

    [Header("Flag")]
    [SerializeField] private bool m_bIsSelectObject;
    [SerializeField] public float m_bIsMotionTimeLife = 2f;

    [Header("Resources")]
    // 동작이 필요한 마법의 Flag
    public Dictionary<string, bool> m_dicMagicSpell = new Dictionary<string, bool>()
    { {"Volate Ascendere", false } };
    
    public Coroutine m_currentCoroutin; // 후에 스크립터블 오브젝트로 변경 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_PlayerManager = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
             CastSpell_Accio();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            CastSpell_Repulsio();

        if (Input.GetKeyDown(KeyCode.Alpha3))
            CastSpell_Incendio();

        if (Input.GetKeyDown(KeyCode.Alpha4))
             CastSpell_VolateAscendere();
    }

    public bool TryCastSpell(ushort cost)
    {
        // Check Cost
        if (m_PlayerManager.m_PlayerStatesManager.UseManaForSkill(cost) == false)
        {
            // TODO UI
             
            return false;
        }

        return true;
    }

    // 마법 실행 매커니즘
    // 마법 시도 -> 성공/여부 -> 성공시 성공 판정

    // 화염을 발사해 공격하는 마법

    // 물건을 끌어 당김
    public void CastSpell_Accio()
    {
        // Check()
        if (TryCastSpell(m_iIncendioCost) == false)
            return;

        // Cast
        if (m_bIsSelectObject == false && m_PlayerManager.m_RightHandInteractableObject == null)
        {
            if (m_PlayerManager.m_RightHandLearFarInteractor.interactablesHovered.Count > 0)
            {
                SelectInteractObject();

                // 지속적으로 마나가 빨려들어감
                m_currentCoroutin = StartCoroutine(DrainManaOverTime());

                // Sound
                Managers.Sound.Play(m_AccioSuccessAudioClip);

                // Effect
                // 마법 봉 끝이 빛나야 함 (최초의 노란색. 이후에 파란색)
                // 화면 이펙트

            }
        }
    }

    IEnumerator DrainManaOverTime(float time = 1)
    {
        while (true)
        {
            m_PlayerManager.m_PlayerStatesManager.UseManaForSkill((ushort)time);

            //checked Mana
            if (m_PlayerManager.m_PlayerStatesManager.m_CurrentMana <= 0)
            {
                ReleaseInteractingObject();
                Managers.Sound.Play(m_AccioFailAudioClip);

                yield break;
            }

            yield return null;
        }
    }

    private void SelectInteractObject()
    {
        m_PlayerManager.m_RightHandInteractableObject = m_PlayerManager.m_RightHandLearFarInteractor.interactablesHovered[0] as XRBaseInteractable;

        m_PlayerManager.m_RightHandLearFarInteractor.interactionManager.SelectEnter(
            (IXRSelectInteractor)m_PlayerManager.m_RightHandLearFarInteractor,
            (IXRSelectInteractable)m_PlayerManager.m_RightHandInteractableObject);

        m_bIsSelectObject = true;
    }

    private void ReleaseInteractingObject()
    {
        m_PlayerManager.m_RightHandLearFarInteractor.interactionManager.SelectExit(
            (IXRSelectInteractor)m_PlayerManager.m_RightHandLearFarInteractor,
            (IXRSelectInteractable)m_PlayerManager.m_RightHandInteractableObject);

        m_bIsSelectObject = false;

        m_PlayerManager.m_RightHandInteractableObject = null;
    }

    // 물건을 던짐
    public void CastSpell_Repulsio()
    {
        // Check()
        if (TryCastSpell(m_iIncendioCost) == false)
            return;

        if (m_bIsSelectObject && m_PlayerManager.m_RightHandInteractableObject != null)
        {
            ReleaseInteractingObject();

            var obj = m_PlayerManager.m_RightHandInteractableObject.GetComponent<iMagicInteractableObject>();
            obj.Throw(m_PlayerManager.m_CurrentMagicEquippment.m_MagicSpawnTransform.forward, m_fRepellcioAddForece, ForceMode.Impulse);

            // Drain Mana 코루틴 제거
            if (m_currentCoroutin != null)
                StopCoroutine(m_currentCoroutin);

            Managers.Sound.Play(m_AccioRepullsioAudioClip);
        }
    }


    public void CastSpell_Incendio()
    {
        // Check()
        if (TryCastSpell(m_iIncendioCost) == false)
            return;

        // Prefab 소환
        GameObject go = Managers.Resource.Instantiate(m_IncendioPrefab);
        var obj = go.GetComponent<MagicObjectBase>();
        obj.SetInfo(m_PlayerManager, m_PlayerManager.m_CurrentMagicEquippment.m_MagicSpawnTransform);
        m_PlayerManager.m_InputHandler.right_select = false;

        // Camera

        // Window Effect

        // UI

        // Resource Mana
    }



    // 볼라테 아센데레 : 충격을 주는 마법
    public void CastSpell_VolateAscendere()
    {
        // Check()
        if (TryCastSpell(m_iIncendioCost) == false)
            return;

        if (m_dicMagicSpell["Volate Ascendere"] == true)
        {
            Debug.Log("볼라테 아센데레 실행");
        }
    }
}
