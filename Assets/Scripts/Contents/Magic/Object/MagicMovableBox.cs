using Fusion;
using UnityEngine;
using static Define;

[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(NetworkObject))]
public class MagicMovableBox : MagicObjectBase, IMoveable
{
    [SerializeField] public bool m_bIsMagicInteract = true;
    [SerializeField] public bool m_bIsAttackable = true;
    public int m_iDamage = 0;
    public Rigidbody m_Rigidbody;
    protected Collider m_Collider;

    [Header("Property")]
    [SerializeField] protected Vector3 m_moveVector;
    [SerializeField] protected LayerMask m_hitLayerMask;

    [Header("RigidBody Property")]
    [SerializeField] protected float m_fImpulse = 1f;

    public virtual void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();

        
        m_hitLayerMask = (1 << LayerMask.NameToLayer("Default")) 
                       | (1 << LayerMask.NameToLayer("Collider")) // 임시 방편으로 넣음
                       | (1 << LayerMask.NameToLayer("Hitable"));
    }

    public bool CanControlMagicObject()
    {
        return m_bIsMagicInteract;
    }

    protected  void OnCollisionEnter(Collision other)
    {
        if (!m_bIsAttackable)
            return;

        if (other.gameObject.layer != m_hitLayerMask)
            return;

        if (m_Owner != null && other.gameObject == m_Owner.gameObject)
            return;

        var isHit = other.gameObject.GetComponentInParent<IHitable>();

        if (isHit != null)
        {
            isHit.OnHit(m_Owner, m_iDamage);
            Clear();
        }
    }

    public void Clear()
    {
        m_iDamage = 0;
        m_Owner = null;
        m_bIsAttackable = false;
    }
}
