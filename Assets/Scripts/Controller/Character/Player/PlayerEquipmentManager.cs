using Fusion;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Casters;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals;
using static Define;

// 플레이어의 장비는 인벤토리에서 전부 관리하며 이곳에서는 장착 여부를 확인하고 스테이터스를 조정한다.

public class PlayerEquipmentManager : MonoBehaviour
{
    [Header("Ref")]
    [HideInInspector] public Player m_PlayerManager;

    [Header("Transform")]
    [SerializeField] private Transform m_PlayerRightHandTransform;
    [SerializeField] private Transform m_PlayerLeftHandTransform;

    [Header("Magic Equippment")]
    public List<MagicItem> m_InventoryItems = new List<MagicItem>(); // 임시 인벤토리 창
    public MagicItem m_CurrentWeapon;

    public bool m_UsingRightHandWeapon;
    public bool m_UsingLeftHandWeapon;

    private void Start()
    {
        m_PlayerManager = GetComponent<Player>();
        NearFarInteractorRaySet();
    }

    public void NearFarInteractorRaySet()
    {
        XRBaseInteractor interactor = null;

        if (m_UsingRightHandWeapon)
        {
            interactor = m_PlayerManager.m_HardwareRig.m_RightHandLearFarInteractor;
        }
        
        else if(m_UsingLeftHandWeapon)
        {
            interactor = m_PlayerManager.m_HardwareRig.m_LeftHandLearFarInteractor;
        }

        var caster = interactor.GetComponent<CurveInteractionCaster>();
        var visual = interactor.GetComponentInChildren<CurveVisualController>();

        Transform weaponEdge = m_CurrentWeapon.m_EquipmentEdge_SpawnTransform;

        caster.castOrigin = weaponEdge;
        visual.lineOriginTransform = weaponEdge;
    }
}
