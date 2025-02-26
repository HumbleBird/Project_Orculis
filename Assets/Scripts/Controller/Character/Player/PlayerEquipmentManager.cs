using Fusion;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class PlayerEquipmentManager : NetworkBehaviour
{
    [Header("Ref")]
    [HideInInspector] public Player m_PlayerManager;

    [Header("Transform")]
    [SerializeField] private Transform m_PlayerRightHandTransform;
    [SerializeField] private Transform m_PlayerLeftHandTransform;

    [Header("Magic Equippment")]
    public List<Item> m_Items = new List<Item>();
    public Weapon m_CurrentWeapon;

    private void Awake()
    {
        m_PlayerManager = GetComponent<Player>();

        // ItemLoad
        ItemInit();
    }
    
    private void ItemInit()
    {
        // 아이템 로드
        ItemLoad();

        // 현재 장착 중인 아이템 enable
        EquipItem();
    }

    private void ItemLoad()
    {
        GameObject i = null;
        foreach (var item in m_Items)
        {
            if(item.PrefabObject != null)
            {
                Transform tr = ItemLoadType(item);

                item.TempObject = Managers.Resource.Instantiate(item.PrefabObject, tr);

            }
            else if (item.Path.Length > 0)
            {
                Managers.Resource.Instantiate(item.Path);
            }
            else
            {
                Debug.LogError("현재 로드할 아이템이 없습니다.");
                return;
            }

            item.TempObject.SetActive(false);
        }
    }

    private Transform ItemLoadType(Item item)
    {
        Transform tr = null;

        switch (item.ItemType)
        {
            case Define.E_ItemType.Weapon:
                tr = m_PlayerRightHandTransform;
                break;
            case Define.E_ItemType.Armor:
                break;
            case Define.E_ItemType.Accessorie:
                break;
            case Define.E_ItemType.Consumable:
                break;
        }

        return tr;
    }

    public void EquipItem()
    {
        // 가장 마지막에 장착 중이었던 아이템 정보 로드

        // 없다면 현재 인벤토리의 1번 아이템 장착
        {
            // Weapon
            ItemEquip(E_ItemType.Weapon);
        }
    }

    private void ItemEquip(E_ItemType type)
    {
        Item i = m_Items
                    .Where(m_Items => m_Items.ItemType == type)
                    .FirstOrDefault();

        if (i != null)
        {
            switch (type)
            {
                case E_ItemType.Weapon:
                    m_CurrentWeapon = (Weapon)i;
                    break;
                case E_ItemType.Armor:
                    break;
                case E_ItemType.Accessorie:
                    break;
                case E_ItemType.Consumable:
                    break;
                default:
                    break;
            }

            i.IsEquip(true);

            Debug.Log($"Item Find Success [Name : {i.Name}], [Parts : {i.ItemType}]");
        }
        else
        {
            Debug.LogError($"Item Find Fail [Name : {i.Name}], [Parts : {i.ItemType}]");
        }
    }
}
