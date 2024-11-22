using UnityEngine;
using static Define;

public class Magic_Object : MonoBehaviour
{

    /// <summary>
    ///  Repulsio 같이 물건이 던져 졌을 때 데미지 판정이 들어간다.
    /// </summary>
    /// 

    public bool m_isHitable = false;

    public Collider m_Collider;
    public Rigidbody m_Rigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Collider = GetComponent<Collider>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void OnHitableTirgger()
    {
        m_isHitable = true;
    }

    public void OffHitableTirgger()
    {
        m_isHitable = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision != null)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                var hitObj = collision.gameObject.GetComponent<IHitable>();

                if (hitObj != null)
                    hitObj.OnHit();
            }

            if(m_isHitable)
            {
                m_isHitable = false;    
            }
        }
    }

    public void Throw(Vector3 vec, float power, ForceMode mode = ForceMode.Force)
    {
        m_Rigidbody.AddForce(vec * Time.deltaTime * power, mode);
    }

}
