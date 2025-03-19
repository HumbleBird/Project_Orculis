using Fusion;
using UnityEngine;

public class MagicMovableBox : MagicObjectBase, IMoveable
{
    [SerializeField] public bool m_bIsMagicInteract = true;
    public int m_iDamage = 0;

    public bool CanControlMagicObject()
    {
        return m_bIsMagicInteract;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
            return;

        Player enemy =  other.GetComponentInParent<Player>();

        if(enemy != null && enemy != m_Owner)
        {
            enemy.m_PlayerStatesManager.OnHit(attacker: m_Owner, damage : m_iDamage);

            Clear();
        }
    }

    public void Clear()
    {
        m_iDamage = 0;
        m_Owner = null;
    }
}
