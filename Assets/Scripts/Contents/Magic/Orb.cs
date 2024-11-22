using Oculus.Interaction;
using System.Collections;
using UnityEngine;
using static Define;

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
    [SerializeField]  private float m_fLiftTime = 10f;
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

        StartCoroutine(TimeOverDestroy());
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
            // other의 레이어가 m_hitLayerMask에 포함되지 않으면 return
            if ((m_hitLayerMask & (1 << other.gameObject.layer)) == 0)
                return;

            if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                var hitObj = other.gameObject.GetComponent<IHitable>();

                if (hitObj != null)
                    hitObj.OnHit();
            }

            // Rigidbody의 움직임 정지
            m_Rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

            m_goOrbBody.gameObject.SetActive(false);
            m_goTrail.gameObject.SetActive(false);

            foreach (var particle in m_DestoryParticlePrefabs)
            {
                particle.Play();
            }

            Destroy(gameObject, m_fLiftTime);
        }
    }

    private IEnumerator TimeOverDestroy()
    {
        yield return new WaitForSeconds(m_fLiftTime);

        Destroy(gameObject);
    }
}
