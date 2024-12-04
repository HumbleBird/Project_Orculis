using System.Collections;
using UnityEngine;
using static Define;

// 던질 수 있는 오브젝트
public interface iMagicInteractableObject
{
    public void Throw(Vector3 pos, float power, ForceMode mode);
}

// 스펠로부터 생성된 모든 오브젝트의 기본 베이스
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class MagicObjectBase : MonoBehaviour
{
    [Header("Ref")]
    protected PlayerManager m_Owner;
    protected Rigidbody m_Rigidbody;
    protected Collider m_Collider;

    [Header("Property")]
    [SerializeField] protected float m_fLiftTime = 10f;
    [SerializeField] protected Vector3 m_moveVector;
    [SerializeField] protected LayerMask m_hitLayerMask;
    [SerializeField] protected float m_fImpulse = 1f;
    public ushort m_fSpellCost { get; set; }

    [Header("Sound")]
    [SerializeField] protected AudioClip m_AudioClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();

        m_hitLayerMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Character"));

        // Sound
        if (m_AudioClip != null)
            Managers.Sound.Play(m_AudioClip);
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


}

