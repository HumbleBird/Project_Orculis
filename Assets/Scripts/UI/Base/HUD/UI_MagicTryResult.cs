using TMPro;
using UnityEngine;

public class UI_MagicTryResult : MonoBehaviour
{
    [Header("Ref")]
    public Player m_PlayerManager;

    public TextMeshProUGUI m_playersUtteranceTrySpell;
    public TextMeshProUGUI m_playersUtterancResult;

    public TextMeshProUGUI m_playersBehaviourTrySpell;
    public TextMeshProUGUI m_playersBehaviourResult;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_PlayerManager = GetComponentInParent<Player>();
    }

    public void ShowUIPlayersMagicUtterance(string text)
    {
        m_playersUtteranceTrySpell.text = text;
    }

    public void ShowUIMagicAttemptResultUterreance(bool result)
    {
        string text = result.ToString();
        m_playersUtterancResult.text = text;
    }

    public void ShowUIPlayersMagicBehaviour(string text)
    {
        m_playersBehaviourTrySpell.text = text;
    }

    public void ShowUIMagicAttemptBehaviourResult(bool result)
    {
        string text = result.ToString();
        m_playersBehaviourResult.text = text;
    }
}
