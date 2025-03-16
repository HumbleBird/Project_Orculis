using UnityEngine;

public class Indicator : MonoBehaviour
{
    public Player m_Player; // Stat 표시할 Player

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        m_Player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
