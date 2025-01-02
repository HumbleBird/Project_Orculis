using UnityEngine;
using static Define;

public class PlayerStateManager : MonoBehaviour, IHitable
{
    [Header("Ref")]
    private PlayerManager m_PlayerManager;

    [Header("Health Stats")]
    public int m_MaxHealth { private set; get; } = 100;
    public int m_CurrentHealth { private set; get; }

    [Header("Mana Stats")]
    public int m_MaxMana { private set; get; } = 100;
    public int m_CurrentMana { private set; get; }
    public float m_ManaRegenRate { private set; get; } // 초당 마나 회복량

    private float m_ManaRegenTimer = 0f;

    void Start()
    {
        m_PlayerManager = GetComponent<PlayerManager>();
        InitState();
    }

    void Update()
    {
        RegenerateMana();
    }

    public void InitState()
    {
        InitHealth();
        InitMana();
    }

    // 체력 관련 메서드
    public void InitHealth()
    {
        m_CurrentHealth = m_MaxHealth;

        m_PlayerManager.m_HUD.RefreshUI();
    }

    public void ChangeHealth(int healthDelta)
    {
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth + healthDelta, 0, m_MaxHealth);
        if (m_CurrentHealth <= 0)
        {
            OnPlayerDeath();
        }
    }

    private void OnPlayerDeath()
    {
        Debug.Log("Player has died!");
        // 플레이어 사망 처리 로직 (예: Respawn, Game Over 등)
    }

    // 마나 관련 메서드
    public void InitMana()
    {
        m_CurrentMana = m_MaxMana;
    }

    public void ChangeMana(int manaDelta)
    {
        m_CurrentMana = Mathf.Clamp(m_CurrentMana + manaDelta, 0, m_MaxMana);

        m_PlayerManager.m_HUD.RefreshUI();
    }

    private void RegenerateMana()
    {
        if (m_CurrentMana < m_MaxMana)
        {
            m_ManaRegenTimer += Time.deltaTime;
            if (m_ManaRegenTimer >= 1f)
            {
                m_CurrentMana = (ushort)Mathf.Min(m_CurrentMana + m_ManaRegenRate, m_MaxMana);
                m_ManaRegenTimer = 0f;

                m_PlayerManager.m_HUD.RefreshUI();
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

    public void OnHit(PlayerManager Attacker, int damage)
    {
        Debug.Log("On Hit : " + name);

        // HP
        ChangeHealth((ushort)-damage);

        // UI

        // Sound

        // Effect
    }
}
