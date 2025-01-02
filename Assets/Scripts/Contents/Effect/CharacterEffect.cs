using UnityEngine;
using static Define;

public abstract class CharacterEffect : ScriptableObject
{
    public E_CharacterEffectType m_iEffectType;
    public float m_fEffectDuration = 10f; // -1 : Infinite Time
    public bool m_isPersistentEffect;
    public bool m_isStack = false; // Effect가 중첩이 가능한지 여부
    public int m_iStackCount = 0; // 중첩 Count

    // 1회 실행 인지 연속 실행인지
    public abstract void ProccessEffect(PlayerManager player);
}