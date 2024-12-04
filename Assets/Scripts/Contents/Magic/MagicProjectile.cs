using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Define;

// 발사 (구체, 화살, 파이어볼 등)
public class MagicProjectile : MagicObjectBase
{
    [Header("Prefab")]
    [SerializeField] protected GameObject m_ProjectilePrefab;
    [SerializeField] protected GameObject[] m_goBodyParticle;
    [SerializeField] protected ParticleSystem[] m_DestoryParticlePrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        Throw();

        StartCoroutine(TimeOverDestroy());
    }

    private void Throw()
    {
        m_Rigidbody.AddForce(transform.forward * Time.deltaTime * m_fImpulse);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            // other의 레이어가 m_hitLayerMask에 포함되지 않으면 return
            if ((m_hitLayerMask & (1 << other.gameObject.layer)) == 0)
                return;

            if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                var hitObj = other.gameObject.GetComponent<IHitable>();

                if (hitObj != null)
                    hitObj.OnHit(m_Owner, 10);
            }

            // Rigidbody의 움직임 정지
            m_Rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

            // Particle
            if (m_goBodyParticle != null)
            {
                foreach (var particle in m_goBodyParticle)
                {
                    particle.gameObject.SetActive(false);
                }
            }

            if (m_DestoryParticlePrefabs != null)
            {
                foreach (var particle in m_DestoryParticlePrefabs)
                {
                    particle.Play();
                }
            }

            StartCoroutine(TimeOverDestroy(m_fLiftTime));
        }
    }
}
