using Fusion;
using Meta.WitAi.CallbackHandlers;
using Oculus.Voice;
using Projectiles;
using SimpleFPS;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static Define;

public class Player : NetworkBehaviour
{
    //[SerializeField]
    //private DebugInputControl _inputControl;
    [Header("PlayerManager Ref")]
    public HardwareRig m_HardwareRig;
    [HideInInspector] public PlayerStatManager m_PlayerStatesManager;
    [HideInInspector] public PlayerMagicManager m_PlayerMagicManager;
    [HideInInspector] public PlayerEffectsManager m_PlayerEffectsManager;
    [HideInInspector] public PlayerEquipmentManager m_PlayerEquipmentManager;
    [HideInInspector] public CharacterAnimationManager m_CharacterAnimationManager;
    [HideInInspector] public PlayerSoundManager m_PlayerSoundManager;

    [Header("Other Ref")]
    AppVoiceExperience voiceExperience;
    public SceneObjects _sceneObjects;

    [Header("Interactor")]
    public XRBaseInteractor m_RightHandLearFarInteractor;
    public XRBaseInteractor m_LeftHandLearFarInteractor;
    public XRBaseInteractable m_RightHandInteractableObject;

    [Header("Magic Base")]
    [SerializeField] private GameObject m_MagicStafeMoveParticle;
    [SerializeField] private float m_fParticleTimeInterval = 0.03f;
    bool isGeneratingParticles = false;

    [Header("Debug")]
    public bool[] m_AlphaList = new bool[10];

    public override void Spawned()
    {
        _sceneObjects = Runner.GetSingleton<SceneObjects>();

        m_PlayerStatesManager = GetComponent<PlayerStatManager>();
        m_PlayerMagicManager = GetComponent<PlayerMagicManager>();
        m_PlayerEffectsManager = GetComponent<PlayerEffectsManager>();
        m_PlayerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        m_CharacterAnimationManager = GetComponent<CharacterAnimationManager>();
        m_PlayerSoundManager = GetComponent<PlayerSoundManager>();

        StartCoroutine(GenerateMagicMoveParticle());

        if (HasInputAuthority == false)
        {
            name = name + " Orther Player";
        }
        else
        {
            name = name + " Input Player";

            // Voice
            voiceExperience = _sceneObjects.m_AppVoiceExperience;
            var matcher = voiceExperience.GetComponentInChildren<WitResponseMatcher>();
            matcher.onMultiValueEvent.AddListener(CheckVoiceMagicSpells);

            // Mivry
            Mivry mivry = _sceneObjects.m_Mivry;
            mivry.gameObject.SetActive(true);
            mivry.OnGestureCompletion.AddListener(CheckRecognition);
            mivry.LeftHand = m_CharacterAnimationManager.leftHandController.gameObject;
            mivry.RightHand = m_CharacterAnimationManager.rightHandController.gameObject;

            // Set XRBaseInteractor
            m_HardwareRig =  _sceneObjects.m_Rig;
            m_HardwareRig.runner = Runner;
            var networkEvents = Runner.GetComponent<NetworkEvents>();
            networkEvents.OnInput.AddListener(m_HardwareRig.OnInput);

            m_RightHandLearFarInteractor = m_HardwareRig.m_RightHandLearFarInteractor;
            m_LeftHandLearFarInteractor = m_HardwareRig.m_LeftHandLearFarInteractor;
        }
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if (Runner.LocalPlayer == Object.InputAuthority)
        {
           // _inputControl.RequestCursorRelease();
        }
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
        //m_MagicTryResultUI.ShowUIPlayersMagicUtterance(spellString);
    }

    public void CheckRecognition(GestureCompletionData data)
    {
        if (data.gestureID < 0)
        {
            string errMsg = GestureRecognition.getErrorMessage(data.gestureID);
            Debug.Log(errMsg);
            return;
        }

        //Debug.Log("data similarity : " + data.similarity);
        //Debug.Log("data gestureName : " + data.gestureName);
        //Debug.Log("data parts : " + data.parts);
        //Debug.Log("data gestureID : " + data.gestureID);

        // 얼마나 기록한 제스쳐와 유사한가.
        if (data.similarity > 0.2f)
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
            if(isGeneratingParticles && m_PlayerEquipmentManager.m_CurrentWeapon != null)
            {
                // 파티클 소환
                GameObject go  = Managers.Resource.Instantiate(m_MagicStafeMoveParticle);
                go.transform.position = m_PlayerEquipmentManager.m_CurrentWeapon.m_MuzzleTransform.position;
                go.transform.rotation = m_PlayerEquipmentManager.m_CurrentWeapon.m_MuzzleTransform.rotation;

                yield return new WaitForSeconds(m_fParticleTimeInterval);
            }

            yield return null;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (HasInputAuthority)
            voiceExperience.Activate();

        if (GetInput(out RigInput input))
        {
            // Input is processed on InputAuthority and StateAuthority.
            ProcessInput(input);
        }
    }

    private void ProcessInput(RigInput input)
    {
        // Head Gear

        // Right Hand
        if (input.rightHandCommand.Activate)
        {

            MoveMagicStaff(true);
        }
        else
        {
            MoveMagicStaff(false);

        }


        // Right Hand

        // Debug Spell Mater
        Debug_SpellAlphaInput(input.Alpha0, 0);
        Debug_SpellAlphaInput(input.Alpha1, 1);
        Debug_SpellAlphaInput(input.Alpha2, 2);
        Debug_SpellAlphaInput(input.Alpha3, 3);
        Debug_SpellAlphaInput(input.Alpha4, 4);
        Debug_SpellAlphaInput(input.Alpha5, 5);
        Debug_SpellAlphaInput(input.Alpha6, 6);
        Debug_SpellAlphaInput(input.Alpha7, 7);
        Debug_SpellAlphaInput(input.Alpha8, 8);
        Debug_SpellAlphaInput(input.Alpha9, 9);

    }

    private void Debug_SpellAlphaInput(NetworkBool alpha, int count)
    {
        if (alpha && m_AlphaList[count] == false)
        {
            m_AlphaList[count] = true;
            m_PlayerMagicManager.Editor_SuccessTrySpell(count);
        }
        else if (alpha == false && m_AlphaList[count] == true)
        {
            m_AlphaList[count] = false;
        }
    }
}
