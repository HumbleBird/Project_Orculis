using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace SimpleFPS
{
    public class UIHealth : MonoBehaviour
    {
        [Header("Hit Effect")]
        public Volume m_HitVolume;
        public float m_fDuration = 0.05f;

        public TextMeshProUGUI Value;
        public Image Progress;
        public GameObject ImmortalityIndicator;
        public GameObject HitTakenEffect;
        public GameObject DeathEffect;
        public Animation HealthProgressAnimation;
        public TextMeshProUGUI HealValue;

        private int _lastHealth = -1;

        public void UpdateHealth(Player player)
        {
            PlayerStatManager playerStat = player.m_PlayerStatesManager;
            if (playerStat == null)
                return;

            //ImmortalityIndicator.SetActive(health.IsImmortal);

            int currentHealth = Mathf.CeilToInt(playerStat.m_CurrentHealth);

            // Update UI only when health actually changed.
            if (currentHealth == _lastHealth)
                return;
             
            //Value.text = currentHealth.ToString();

            float progress = (float)playerStat.m_CurrentHealth / playerStat.m_MaxHealth;
            //Progress.fillAmount = progress;
            //SampleHealthProgressAnimation(progress);

            if (currentHealth < _lastHealth)
            {
                StartCoroutine(HitEffectScreen());
            }

            //DeathEffect.SetActive(health.IsAlive == false);

            _lastHealth = currentHealth;
        }

        public IEnumerator HitEffectScreen()
        {
            m_HitVolume.enabled = true;

            yield return new WaitForSeconds(m_fDuration);

            m_HitVolume.enabled = false;
        }

        public void ShowHeal(float value)
        {
            HealValue.text = $"+{Mathf.RoundToInt(value)} HP";

            // Restart the animation.
            HealValue.gameObject.SetActive(false);
            HealValue.gameObject.SetActive(true);
        }

        private void Awake()
        {
            //HealValue.gameObject.SetActive(false);
        }

        /// <summary>
        /// Coloring of the health bar is done through animation.
        /// Sample animation at correct time to achieve desired health bar state.
        /// </summary>
        private void SampleHealthProgressAnimation(float normalizedTime)
        {
            var animationState = HealthProgressAnimation[HealthProgressAnimation.clip.name];

            // Make sure the animation is affecting objects.
            animationState.weight = 1f;
            animationState.enabled = true;

            animationState.normalizedTime = normalizedTime;
            HealthProgressAnimation.Sample();

            animationState.enabled = false;
        }
    }
}
