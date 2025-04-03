using Fusion;
using UnityEngine;

public class MagicItem : NetworkBehaviour
{
    // 마법 소환 위치
    // 파티클 소환 위치
    [SerializeField] public Transform m_MuzzleTransform;
    public GameObject m_IndicatorMana;
    public Player m_Owner;
    public float m_fIndicatorManaOffset = 0.01f;

    public override void Spawned()
    {
        m_Owner = GetComponentInParent<Player>();
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        IndicatorMana();
    }

    // 현재 LocalPlayer한테만 보임
    public void IndicatorMana()
    {
        int maxMana = m_Owner.m_PlayerStatesManager.m_MaxMana;
        int currentMana = m_Owner.m_PlayerStatesManager.m_CurrentMana;

        float ratio = ((float)currentMana / maxMana) * m_fIndicatorManaOffset; 
        m_IndicatorMana.transform.localScale = Vector3.Lerp(m_IndicatorMana.transform.localScale,  new Vector3(ratio, ratio, ratio), Time.deltaTime);
    }
}
