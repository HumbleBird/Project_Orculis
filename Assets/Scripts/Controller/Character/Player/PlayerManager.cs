using Meta.XR.ImmersiveDebugger.UserInterface;
using NUnit.Framework;
using Oculus.Voice;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static Define;


public class PlayerManager : MonoBehaviour
{
    [Header("Ref")]
    public InputHandler m_InputHandler;
    public AppVoiceExperience voiceExperience;
    public GestureEventProcessor m_GestureEventProcessor;
    public StressReceiver m_StressReceiver; // Camera
    public PlayerStateManager m_PlayerStatesManager;
    public HUD m_HUD;
    public PlayerMagicManager m_PlayerMagicManager;

    [Header("Interactor")]
    public XRBaseInteractor m_RightHandLearFarInteractor;

    [Header("Interactable Object")]
    public XRBaseInteractable m_RightHandInteractableObject;

    [Header("Magic Equippment")]
    public MagicEquippment m_CurrentMagicEquippment;

    [Header("Magic Base")]
    [SerializeField] private GameObject m_MagicStafeMoveParticle;
    [SerializeField] private float m_fParticleTimeInterval = 0.1f;
    public bool isGeneratingParticles = false;

    private void Awake()
    {
        m_InputHandler = GetComponent<InputHandler>();
        m_GestureEventProcessor = GetComponent<GestureEventProcessor>();
        m_PlayerStatesManager = GetComponent<PlayerStateManager>();
        m_PlayerMagicManager = GetComponent<PlayerMagicManager>();

        // Camera
        m_StressReceiver = GetComponentInChildren<StressReceiver>();
        m_HUD = GetComponentInChildren<HUD>();
    }

    void Start()
    {
        StartCoroutine(GenerateMagicMoveParticle());
    }

    void Update()
    {
        voiceExperience.Activate();
    }

    // Meta Voice, Wit.ai를 통해 음성을 입력받아서 해당 스펠의 함수를 실행함.
    public void CheckVoiceMagicSpells(string[] vars)
    {
        // 유효한 값만 추출
        var validValues = vars
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .ToArray();

        // 비었는지 확인
        if (validValues.Any() == false)
            return;

        // 첫 번째 단어 주문
        string spellString = validValues[0];

        // 동적 메서드 호출 (Reflection 활용)
        try
        {
            var methodName = $"CastSpell_{spellString}";
            var method = GetType().GetMethod(methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            if (method != null)
            {
                method.Invoke(this, null);
            }
            else
            {
                Debug.LogWarning($"알 수 없는 주문: {spellString}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"주문 처리 중 오류 발생: {ex.Message}");
        }
    }

    public void CheckRecognition(GestureCompletionData data)
    {
        //    Debug.Log("data similarity : " + data.similarity);
        //    Debug.Log("data gestureName : " + data.gestureName);
        //    Debug.Log("data parts : " + data.parts);
        //    Debug.Log("data gestureID : " + data.gestureID);

        // 얼마나 기록한 제스쳐와 유사한가.
        if(data.similarity > 0.4f)
        {
            StartCoroutine(SpellBoolMotionTimeCheck(data.gestureName));
        }
    }

    public void MoveMagicStaff(bool isMove)
    {
        isGeneratingParticles = isMove;
    }

    public IEnumerator GenerateMagicMoveParticle()
    {
        while(true)
        {
            if(isGeneratingParticles)
            {
                // 파티클 소환
                GameObject go  = Managers.Resource.Instantiate(m_MagicStafeMoveParticle);
                go.transform.position = m_CurrentMagicEquippment.m_MagicSpawnTransform.position;
                go.transform.rotation = m_CurrentMagicEquippment.m_MagicSpawnTransform.rotation;

                yield return new WaitForSeconds(m_fParticleTimeInterval);
            }

            yield return null;
        }
    }

    // 동작에 따른 해당 Bool값 True/False 변경
    private IEnumerator SpellBoolMotionTimeCheck(string spellMotionName)
    {
        if(m_PlayerMagicManager.m_dicMagicSpell.ContainsKey(spellMotionName))
        {
            m_PlayerMagicManager.m_dicMagicSpell[spellMotionName] = true;

            Debug.Log($"Magic Spell Bool Change : {spellMotionName} - True");

            yield return new WaitForSeconds(m_PlayerMagicManager.m_bIsMotionTimeLife);

            m_PlayerMagicManager.m_dicMagicSpell[spellMotionName] = false;

            Debug.Log($"Magic Spell Bool Change : {spellMotionName} - False");
        }
    }

    // Weapon
    public void EquipItem()
    {
        m_CurrentMagicEquippment = GetComponentInChildren<MagicEquippment>();
    }
}
