using UnityEngine;

public class HUD : MonoBehaviour
{
    [Header("Ref")]
     PlayerManager m_PlayerManager;

    [Header("State")]
    UI_Stat m_UIStat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_PlayerManager = GetComponentInParent<PlayerManager>();

        m_UIStat = GetComponentInChildren<UI_Stat>();

        RefreshUI();
    }

    public void RefreshUI()
    {
        m_UIStat.RefreshUI();
    }
}
