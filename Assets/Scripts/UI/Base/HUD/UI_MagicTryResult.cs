using TMPro;
using UnityEngine;

public class UI_MagicTryResult : MonoBehaviour
{
    [Header("Ref")]
    public PlayerManager m_PlayerManager;

    public TextMeshProUGUI m_playersUtterance;
    public TextMeshProUGUI m_Result;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_PlayerManager = GetComponentInParent<PlayerManager>();
    }

    public void ShowUIPlayersMagicUtterance(string text)
    {
        m_playersUtterance.text = text;
    }

    public void ShowUIMagicAttemptResult(bool result)
    {
        string text = result.ToString();
        m_Result.text = text;
    }
}
