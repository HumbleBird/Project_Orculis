using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    [Header("Ref")]
    [HideInInspector] public PlayerManager m_PlayerManager;

    [Header("Magic Equippment")]
    public MagicEquippment m_CurrentMagicEquippment;

    private void Awake()
    {
        m_PlayerManager = GetComponent<PlayerManager>();
        EquipItem();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Weapon
    public void EquipItem()
    {
        m_CurrentMagicEquippment = GetComponentInChildren<MagicEquippment>();
    }
}
