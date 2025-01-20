using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum E_TipType
{
    MasteringSpells,
    BrewingPotions,
    UtilizingStealth,
    ExploringHogwarts,
    CombatStrategy,
    Quidditch,
    VRControls,
    BalancingwithReality,
    HogwartsHistory,
    InteractingwithNPCs,
    Other
}

[System.Serializable]
public struct strTip
{
    public int m_Num;
    public bool m_canUse;
    public E_TipType tipType;

    [TextArea(3, 10)]
    public string korea_tip;

    [TextArea(3, 10)]
    public string english_tip;
}

[CreateAssetMenu(fileName = "GameTip", menuName = "Loading/Game Tip")]
public class GameTip : ScriptableObject
{
    public List<strTip> m_tips = new List<strTip>();
    private strTip? lastDisplayedTip; // 마지막 표시된 팁 (nullable)

    /// <summary>
    /// 초기화: 모든 팁을 사용할 수 있는 상태로 설정
    /// </summary>
    public void ResetTips()
    {
        for (int i = 0; i < m_tips.Count; i++)
        {
            m_tips[i] = new strTip
            {
                m_Num = m_tips[i].m_Num,
                m_canUse = true,
                tipType = m_tips[i].tipType,
                korea_tip = m_tips[i].korea_tip,
                english_tip = m_tips[i].english_tip
            };
        }
    }

    /// <summary>
    /// 다음 팁을 반환하고, 해당 팁의 상태를 비활성화로 설정
    /// </summary>
    /// <returns>선택된 팁의 영어 설명</returns>
    public string GetNextGameTip()
    {
        // 활성화된 팁 검색
        var activeTip = m_tips.FirstOrDefault(t => t.m_canUse);

        if (activeTip.m_canUse)
        {
            // 활성화된 팁 비활성화
            int tipIndex = m_tips.IndexOf(activeTip);
            m_tips[tipIndex] = new strTip
            {
                m_Num = activeTip.m_Num,
                m_canUse = false,
                tipType = activeTip.tipType,
                korea_tip = activeTip.korea_tip,
                english_tip = activeTip.english_tip
            };

            // 마지막으로 표시된 팁 업데이트
            lastDisplayedTip = activeTip;
            return activeTip.english_tip;
        }
        else
        {
            // 모든 팁을 초기화
            ResetTips();

            // 마지막으로 표시된 팁 비활성화
            if (lastDisplayedTip.HasValue)
            {
                var lastTip = lastDisplayedTip.Value;
                int lastTipIndex = m_tips.FindIndex(t => t.Equals(lastTip));

                if (lastTipIndex >= 0)
                {
                    m_tips[lastTipIndex] = new strTip
                    {
                        m_Num = lastTip.m_Num,
                        m_canUse = false,
                        tipType = lastTip.tipType,
                        korea_tip = lastTip.korea_tip,
                        english_tip = lastTip.english_tip
                    };
                }
            }

            return GetNextGameTip();
        }
    }

    // Unity Editor에서 변경 사항이 발생할 때 호출
    private void OnValidate()
    {
        // 모든 팁의 번호를 자동으로 업데이트
        for (int i = 0; i < m_tips.Count; i++)
        {
            m_tips[i] = new strTip
            {
                m_Num = i + 1, // 1부터 시작하는 번호
                m_canUse = true,
                tipType = m_tips[i].tipType,
                korea_tip = m_tips[i].korea_tip,
                english_tip = m_tips[i].english_tip
            };
        }
    }
}
