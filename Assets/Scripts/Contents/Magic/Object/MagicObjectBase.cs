using Fusion;
using System.Collections;
using UnityEngine;
using static Define;

public interface IMoveable
{
    public bool CanControlMagicObject();
}

// 마법으로부터 영향을 받는 모든 오브젝트.
// ex) 마법으로 움직이게 된 물체
// ex) 마법으로 생성된 파이어 볼
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(NetworkObject))]
public abstract class MagicObjectBase : MonoBehaviour
{
    [Header("Ref")]
    public Player m_Owner;
    public Rigidbody m_Rigidbody;
    protected Collider m_Collider;

    [Header("Property")]
    [SerializeField] protected Vector3 m_moveVector;
    [SerializeField] protected LayerMask m_hitLayerMask;

    [Header("RigidBody Property")]
    [SerializeField] protected float m_fImpulse = 1f;

    [Header("Spell Property")]
    public ushort m_fSpellCost { get; set; }

    public virtual void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();

        m_hitLayerMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Character"));
    }

    protected abstract void OnTriggerEnter(Collider other);
}

