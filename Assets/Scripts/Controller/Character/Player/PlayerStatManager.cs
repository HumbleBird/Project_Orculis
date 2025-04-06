using Fusion;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using static Define;

public class PlayerStatManager : NetworkBehaviour, IHitable
{
    [Header("Ref")]
    private Player m_PlayerManager;

    //[Header("Health")]
    [Networked]
    public int m_MaxHealth { set; get; } = 100;

    [SerializeField]
    [Networked, OnChangedRender(nameof(OnChangedHealth))]
    public int m_CurrentHealth { set; get; }
    public int m_LastHeath;


    //[Header("Mana")]
    public int m_MaxMana {  set; get; } = 100;

    [Networked]
    public int m_CurrentMana {  set; get; }


    //[Header("Mana Regent")]
    [Networked]
    public float m_ManaRegenRate {  set; get; } // 초당 마나 회복량

    [Networked]
    public float m_ManaRegenTimer { get; set; } = 0f;


    public override void Spawned()
    {
        InitNetworkState();
    }

    void Awake()
    {
        m_PlayerManager = GetComponent<Player>();
    }

    public override void FixedUpdateNetwork()
    {
        RegenerateMana();
    }

    public void InitNetworkState()
    {
        InitHealth();
        InitMana();
    }

    // 체력 관련 메서드
    public void InitHealth()
    {
        if (HasStateAuthority == false)
            return;

        m_LastHeath = m_MaxHealth;
        m_CurrentHealth = m_MaxHealth;
    }

    private void OnPlayerDeath()
    {
        Debug.Log("Player has died!");
        // 플레이어 사망 처리 로직 (예: Respawn, Game Over 등)
    }

    // 마나 관련 메서드
    public void InitMana()
    {
        if (HasStateAuthority == false)
            return;

        m_CurrentMana = m_MaxMana;
    }

    public void ChangeMana(int manaDelta)
    {
        if (HasStateAuthority == false)
            return;

        m_CurrentMana = Mathf.Clamp(m_CurrentMana + manaDelta, 0, m_MaxMana);

        //m_PlayerManager.m_HUD.RefreshUI();
    }

    private void RegenerateMana()
    {
        if (HasStateAuthority == false)
            return;

        if (m_CurrentMana < m_MaxMana)
        {
            m_ManaRegenTimer += Time.deltaTime;
            if (m_ManaRegenTimer >= 1f)
            {
                m_CurrentMana = (ushort)Mathf.Min(m_CurrentMana + m_ManaRegenRate, m_MaxMana);
                m_ManaRegenTimer = 0f;

                //m_PlayerManager.m_HUD.RefreshUI();
            }
        }
    }

    // 상태 확인 메서드
    public bool IsHealthFull()
    {
        return m_CurrentHealth == m_MaxHealth;
    }

    public bool IsManaFull()
    {
        return m_CurrentMana == m_MaxMana;
    }

    public bool HasEnoughMana(int cost)
    {
        return m_CurrentMana >= cost;
    }

    // Example: 특정 스킬 발동 시 마나 차감
    public bool UseManaForSkill(int cost)
    {
        if (HasEnoughMana(cost))
        {
            ChangeMana(-cost);
            return true;
        }
        Debug.Log("Not enough mana!");

        return false;
    }

    public void OnHit(Player attacker, int damage)
    {
        Debug.Log("On Hit : " + name);

        // HP
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth - damage, 0, m_MaxHealth);
    }

    public void OnChangedHealth()
    {
        if (HasInputAuthority == false)
            return;

        // 처음 Init 방지
        if (m_CurrentHealth == m_LastHeath)
            return;

        m_LastHeath = m_CurrentHealth;

        Debug.Log("공격당함");
        HitSound();
    }

    public void HitSound()
    {
        m_PlayerManager.m_PlayerSoundManager.PlayHitSounds();
    }
}
