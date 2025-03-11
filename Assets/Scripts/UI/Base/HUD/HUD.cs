using UnityEngine;

public class HUD : MonoBehaviour
{
    [Header("Ref")]
    Player m_PlayerManager;

    [Header("State")]
    UI_Stat m_UIStat;
    public UI_KillFeed m_KillFeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        m_PlayerManager = GetComponentInParent<Player>();
        m_UIStat = GetComponentInChildren<UI_Stat>();
        m_KillFeed = GetComponentInChildren<UI_KillFeed>();

        m_UIStat.Init();


        RefreshUI();
    }

    public void Update()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        m_UIStat.RefreshUI();
    }
}
