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
    [Header("PlayerManager Ref")]
    public InputHandler m_InputHandler;
    public PlayerStatManager m_PlayerStatesManager;
    public PlayerMagicManager m_PlayerMagicManager;
    public PlayerEffectsManager m_PlayerEffectsManager;
    public PlayerEquipmentManager m_PlayerEquipmentManager;

    [Header("UI Ref")]
    public HUD m_HUD;
    public DevelopUI m_DevelopUI;
    public UI_MagicTryResult m_MagicTryResultUI;

    [Header("Other Ref")]
    public AppVoiceExperience voiceExperience;
    public GestureEventProcessor m_GestureEventProcessor;
    public StressReceiver m_StressReceiver; // Camera

    [Header("Interactor")]
    public XRBaseInteractor m_RightHandLearFarInteractor;
    public XRBaseInteractable m_RightHandInteractableObject;

    [Header("Magic Base")]
    [SerializeField] private GameObject m_MagicStafeMoveParticle;
    [SerializeField] private float m_fParticleTimeInterval = 0.1f;
    public bool isGeneratingParticles = false;

    private void Awake()
    {
        m_InputHandler = GetComponent<InputHandler>();
        m_GestureEventProcessor = GetComponent<GestureEventProcessor>();
        m_PlayerStatesManager = GetComponent<PlayerStatManager>();
        m_PlayerMagicManager = GetComponent<PlayerMagicManager>();
        m_PlayerEffectsManager = GetComponent<PlayerEffectsManager>();
        m_PlayerEquipmentManager = GetComponent<PlayerEquipmentManager>();

        // Camera
        m_StressReceiver = GetComponentInChildren<StressReceiver>();
        m_HUD = GetComponentInChildren<HUD>();
        m_MagicTryResultUI = GetComponentInChildren<UI_MagicTryResult>();
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

        m_PlayerMagicManager.SpellFlagCheck(E_SpellCheckType.Chant, spellString);

        // Show UI
        m_MagicTryResultUI.ShowUIPlayersMagicUtterance(spellString);
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
            m_PlayerMagicManager.SpellFlagCheck(E_SpellCheckType.Motion, data.gestureName);
        }
    }

    public void MoveMagicStaff(bool isMove)
    {
        isGeneratingParticles = isMove;
    }

    // 모션 동작을 위한 지팡이 이동 시 반짝거리는 작은 입자 생성
    public IEnumerator GenerateMagicMoveParticle()
    {
        while(true)
        {
            if(isGeneratingParticles && m_PlayerEquipmentManager.m_CurrentMagicEquippment != null)
            {
                // 파티클 소환
                GameObject go  = Managers.Resource.Instantiate(m_MagicStafeMoveParticle);
                go.transform.position = m_PlayerEquipmentManager.m_CurrentMagicEquippment.m_EquipmentEdge_SpawnTransform.position;
                go.transform.rotation = m_PlayerEquipmentManager.m_CurrentMagicEquippment.m_EquipmentEdge_SpawnTransform.rotation;

                yield return new WaitForSeconds(m_fParticleTimeInterval);
            }

            yield return null;
        }
    }


}
