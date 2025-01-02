using System.Collections;
using UnityEngine;
using static Define;

// 스펠로부터 생성된 모든 오브젝트의 기본 베이스
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class MagicObjectBase : MonoBehaviour
{
    [Header("Ref")]
    protected PlayerManager m_Owner;
    public Rigidbody m_Rigidbody;
    protected Collider m_Collider;

    [Header("Property")]
    [SerializeField] protected float m_fLiftTime = 20f;
    [SerializeField] protected Vector3 m_moveVector;
    [SerializeField] protected LayerMask m_hitLayerMask;
    [SerializeField] protected bool m_bIsMagicInteract = true;

    [Header("RigidBody Property")]
    [SerializeField] protected float m_fImpulse = 1f;

    [Header("Spell Property")]
    public ushort m_fSpellCost { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();

        m_hitLayerMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Character"));


        StartCoroutine(TimeOverDestroy());
    }

    public virtual void SetInfo(PlayerManager player, Transform trans)
    {
        // 플레이어의 현재 바라보는 방향으로 초기 이동 벡터 설정
        m_Owner = player;
        transform.position = trans.position;
        transform.eulerAngles = trans.eulerAngles;
    }

    protected virtual void OnTriggerEnter(Collider other) { }

    protected IEnumerator TimeOverDestroy(float destroyTimeDelay = 0)
    {
        yield return new WaitForSeconds(destroyTimeDelay);

        Managers.Resource.Destroy(gameObject);
    }

    public bool CanControlMagicObject()
    {
        return m_bIsMagicInteract;
    }
}

