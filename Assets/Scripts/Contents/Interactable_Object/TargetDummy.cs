using UnityEngine;
using static Define;

public class TargetDummy : MonoBehaviour, IHitable
{
    public Animation m_Animation;
    public AudioSource m_AudioSource;

    // 누가 공격 했는지, 어떤 공격을 했는지, 누구에게 공격을 했고, 데미지가 얼마인지 나중에 추가 예정 GameManager에서 관리 예정
    public void OnHit(PlayerManager Attacker, int damage)
    {
        Debug.Log("OnHit : " + name);

        m_Animation.Play();
        m_AudioSource.Play();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Animation = GetComponent<Animation>();
        m_AudioSource = GetComponent<AudioSource>();
    }
}
