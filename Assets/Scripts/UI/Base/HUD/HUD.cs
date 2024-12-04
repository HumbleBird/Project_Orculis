using UnityEngine;

public class HUD : MonoBehaviour
{
    [Header("Ref")]
     PlayerManager m_PlayerManager;

    [Header("State")]
    UI_Stat m_UIState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_PlayerManager = GetComponentInParent<PlayerManager>();

        m_UIState = GetComponentInChildren<UI_Stat>();
    }

    public void RefreshUI()
    {
        m_UIState.RefreshUI();
    }
}
