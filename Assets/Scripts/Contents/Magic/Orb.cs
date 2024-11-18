using System.Collections;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private ParticleSystem[] m_DestoryParticlePrefabs;
    [SerializeField] private GameObject m_goOrbBody;
    [SerializeField] private GameObject m_goTrail;

    [Header("Ref")]
    private PlayerManager m_Owner;
    private Rigidbody m_Rigidbody;
    private Collider m_Collider;

    [Header("Property")]
    [SerializeField]  private float m_fLiftTime = 1f;
    [SerializeField] private Vector3 m_moveVector;
    [SerializeField] LayerMask m_hitLayerMask;
    [SerializeField]  private float m_fImpulse = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();

        m_hitLayerMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("Character"));

        MoveObject();
    }

    public void SetInfo(PlayerManager player, Transform forwardVector)
    {
        m_Owner = player;

        // 플레이어의 현재 바라보는 방향으로 초기 이동 벡터 설정
        transform.eulerAngles = forwardVector.eulerAngles;
    }

    private void MoveObject()
    {
        m_Rigidbody.AddForce(transform.forward * Time.deltaTime * m_fImpulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            if (other.gameObject.layer != m_hitLayerMask)
                return;

            Debug.Log("hit object name : " + other.gameObject.name);
            m_goOrbBody.gameObject.SetActive(false);
            m_goTrail.gameObject.SetActive(false);

            foreach (var particle in m_DestoryParticlePrefabs)
            {
                particle.Play();
            }

            Destroy(gameObject, m_fLiftTime);
        }
    }
}
