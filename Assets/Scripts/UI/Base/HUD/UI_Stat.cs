using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stat : MonoBehaviour
{
    [Header("Ref")]
    HUD m_HUD;
    PlayerManager m_Player;

    [Header("HP")]
    Image m_HealthBarFill;
    Image m_HealthBardown;

    public int m_iPreHp; // HP Refresh �� HP

    [Header("MP")]
    Image m_ManaBarFill;
    Image m_ManaBarDown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Player = GetComponentInParent<PlayerManager>();
        m_HUD = GetComponentInParent<HUD>();

        m_iPreHp = m_Player.m_PlayerStatesManager.m_CurrentHealth;
    }

    // Update is called once per frame
    public void RefreshUI()
    {
        RefreshHPBar();
        RefreshManaBar();
    }

    void RefreshHPBar()
    {
        // Hp Up
        if (m_iPreHp < m_Player.m_PlayerStatesManager.m_CurrentHealth)
        {
            StartCoroutine(HPUp());
        }
        // HP Down
        else if (m_iPreHp > m_Player.m_PlayerStatesManager.m_CurrentHealth)
        {
            m_HealthBarFill.fillAmount = m_Player.m_PlayerStatesManager.m_CurrentHealth / (float)m_Player.m_PlayerStatesManager.m_MaxHealth;
            StartCoroutine(HPDown());
        }

        m_iPreHp = m_Player.m_PlayerStatesManager.m_CurrentHealth;
    }

    public IEnumerator HPDown()
    {
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            m_HealthBardown.fillAmount -= Time.deltaTime;
            if (m_HealthBardown.fillAmount <= m_Player.m_PlayerStatesManager.m_CurrentHealth / (float)m_Player.m_PlayerStatesManager.m_MaxHealth)
            {
                m_HealthBardown.fillAmount = m_Player.m_PlayerStatesManager.m_CurrentHealth / (float)m_Player.m_PlayerStatesManager.m_MaxHealth;
                yield break;
            }

            yield return null;
        }
    }

    public IEnumerator HPUp()
    {
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            m_HealthBarFill.fillAmount += Time.deltaTime;
            if (m_HealthBarFill.fillAmount >= m_Player.m_PlayerStatesManager.m_CurrentHealth / (float)m_Player.m_PlayerStatesManager.m_MaxHealth)
            {
                m_HealthBarFill.fillAmount = m_Player.m_PlayerStatesManager.m_CurrentHealth / (float)m_Player.m_PlayerStatesManager.m_MaxHealth;
                m_HealthBardown.fillAmount = m_Player.m_PlayerStatesManager.m_CurrentHealth / (float)m_Player.m_PlayerStatesManager.m_MaxHealth;

                yield break;
            }

            yield return null;
        }
    }

    void RefreshManaBar()
    {
        m_ManaBarFill.fillAmount = m_Player.m_PlayerStatesManager.m_CurrentMana / (float)m_Player.m_PlayerStatesManager.m_MaxMana;
    }
}
