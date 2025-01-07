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
    [Header("Property")]
    [SerializeField] protected float m_fLifeTime = 20f;

    [Header("Prefab")]
    [SerializeField] protected GameObject m_ProjectilePrefab;
    [SerializeField] protected GameObject[] m_goBodyParticle;
    [SerializeField] protected ParticleSystem[] m_DestoryParticlePrefabs;

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

            StartCoroutine(TimeOverDestroy(m_fLifeTime));
        }
    }

    private void ParticleObjectOnOff(ParticleSystem[] particles)
    {
        if(particles.Count() > 0)
        {
            foreach (var particle in particles)
            {
                particle.Play();
            }
        }
    }

    protected IEnumerator TimeOverDestroy(float destroyTimeDelay = 0)
    {
        yield return new WaitForSeconds(destroyTimeDelay);

        Managers.Resource.Destroy(gameObject);
    }
}
