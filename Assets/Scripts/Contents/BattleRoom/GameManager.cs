using Fusion;
using SimpleFPS;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    // VFX
    [Header("Hit Effect")]
    public Volume target;
    public float m_fDuration = 0.05f;
    public AudioClip[] m_DamageHumanSounds;

    public IEnumerator HitEffectScreen()
    {
        target.enabled = true;

        yield return new WaitForSeconds(m_fDuration);

        target.enabled = false;
    }

    public void HitEffect()
    {
        StartCoroutine(HitEffectScreen());

        Managers.Sound.RandomPlay(m_DamageHumanSounds);
    }
}
