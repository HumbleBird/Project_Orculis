using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Poolable))]
public class MagicStaffMoveParticle : MonoBehaviour
{
    [SerializeField] private float m_fLifeTime = 4;
    bool m_isLive = true;

    private void OnEnable()
    {
        m_isLive = true;
        StartCoroutine(DestoryParticle());
    }

    IEnumerator DestoryParticle()
    {
        yield   return new WaitForSeconds(m_fLifeTime);

        m_isLive = false;

        Managers.Resource.Destroy(gameObject);
    }

    public void Update()
    {
        CheckLifeTime();
    }

    private void CheckLifeTime()
    {
        if(m_isLive == false)
            Managers.Resource.Destroy(gameObject);
    }
}
