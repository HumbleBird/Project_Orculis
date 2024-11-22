using Meta.XR.ImmersiveDebugger.UserInterface;
using NUnit.Framework;
using Oculus.Voice;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static Define;

public class PlayerManager : MonoBehaviour, IHitable
{
    [Header("Ref")]
    InputHandler m_InputHandler;
    public AppVoiceExperience voiceExperience;

    [Header("Interactor")]
    public XRBaseInteractor m_RightHandLearFarInteractor;

    [Header("Interactable Object")]
    public XRBaseInteractable m_RightHandInteractableObject;

    [Header("Magic")]
    [SerializeField] private Transform RightHandStickTransform;
    [SerializeField] private Orb m_OrbPrefab;
    [SerializeField] private float m_fRepellcioAddForece = 50;

    [Header("Flag")]
    [SerializeField] private bool m_bIsSelectObject;

    private void Awake()
    {
        m_InputHandler = GetComponent<InputHandler>();
    }

    void Start()
    {
    }

    void Update()
    {
        voiceExperience.Activate();

        if(Input.GetKeyDown(KeyCode.Alpha1))
            CastSpell_Incendio();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            CastSpell_Accio();

        if (Input.GetKeyDown(KeyCode.Alpha3))
            CastSpell_Repulsio();
    }

    public void CheckMagicSpells(string[] vars)
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

    public void CastSpell_Incendio()
    {
        Orb orb = Instantiate(m_OrbPrefab, RightHandStickTransform.position, Quaternion.identity);
        orb.SetInfo(this, RightHandStickTransform);
        m_InputHandler.right_select = false;
    }

    // 물건을 끌어 당김
    public void CastSpell_Accio()
    {
        if(m_bIsSelectObject == false && m_RightHandInteractableObject == null)
        {
            if (m_RightHandLearFarInteractor.interactablesHovered.Count > 0)
            {
                m_RightHandInteractableObject = m_RightHandLearFarInteractor.interactablesHovered[0] as XRBaseInteractable;

                m_RightHandLearFarInteractor.interactionManager.SelectEnter(
                    (IXRSelectInteractor)m_RightHandLearFarInteractor,
                    (IXRSelectInteractable)m_RightHandInteractableObject);

                m_bIsSelectObject = true;
            }
        }
    }

    // 물건을 던짐
    public void CastSpell_Repulsio()
    {
        if (m_bIsSelectObject && m_RightHandInteractableObject != null)
        {
            m_RightHandLearFarInteractor.interactionManager.SelectExit(
                (IXRSelectInteractor)m_RightHandLearFarInteractor,
                (IXRSelectInteractable)m_RightHandInteractableObject);

            m_bIsSelectObject = false;

            var obj =  m_RightHandInteractableObject.GetComponent<Magic_Object>();
            obj.Throw(RightHandStickTransform.forward, m_fRepellcioAddForece);

            m_RightHandInteractableObject = null;
        }
    }

    public void OnHit()
    {
        Debug.Log("On Hit : " + name);
    }
}
