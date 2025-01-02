using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Define;

public class PlayerEffectsManager : MonoBehaviour
{
    PlayerManager m_PlayerManager;

    // 타이머 한정 이펙트
    [Header("Timed Effects")]
    public List<CharacterEffect> timedEffects;
    [SerializeField] float effectTickTimer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_PlayerManager = GetComponent<PlayerManager>();

    }

    // Update is called once per frame
    void Update()
    {
        ProcessAllTimedEffects();
    }

    // 즉발성 효과
    public void ProcessEffectInstantly(CharacterEffect effect)
    {
        effect.ProccessEffect(m_PlayerManager);
    }

    // 틱 단위로 Effect 효과 적용하기
    public void ProcessAllTimedEffects()
    {
        effectTickTimer += Time.deltaTime;

        if (effectTickTimer >= 1)
        {
            effectTickTimer = 0;

            // PROCESS ALL ACTIVE EFFECT OVER GAME TIME
            for (int i = timedEffects.Count - 1; i > 0; i--)
            {
                timedEffects[i].ProccessEffect(m_PlayerManager);
            }

        }
    }
}
