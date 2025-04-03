using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 스킬의 쿨타임을 나타내는 링의 색상을 변화시킴.
public class IndicatorSkill : Indicator
{
    public List<GameObject> m_RingObj = new List<GameObject>();
    public Dictionary<SpellBase, GameObject> m_DicIndicatorSpells = new Dictionary<SpellBase, GameObject>();
    public Dictionary<GameObject, Material[]> m_DicRingMaterials = new Dictionary<GameObject, Material[]>();

    public override void Start()
    {
        base.Start();

        var unlockspelllist = m_Player.m_PlayerMagicManager.UnlockSpellGet();

        // Temp
        {
            for (int i = 0; i < unlockspelllist.Count; i++)
            {
                m_DicIndicatorSpells.Add(unlockspelllist[i], m_RingObj[i]);

                var rs = m_RingObj[i].GetComponentsInChildren<Renderer>();
                Material[] ms = rs.SelectMany(r => r.materials).ToArray();

                m_DicRingMaterials.Add(m_RingObj[i], ms);
            }
        }
    }

    void Update()
    {
        IndicatorRing();
    }

    // 스킬의 쿨타임에 따라 해당 링의 색상 변화
    void IndicatorRing()
    {
        foreach (var pair in m_DicIndicatorSpells)
        {
            float progress = pair.Key.m_CooldownProgress;
            Color targetColor = new Color(progress, progress, progress);

            if (m_DicRingMaterials.TryGetValue(pair.Value, out Material[] materials))
            {
                foreach (var mat in materials)
                {
                    mat.color = Color.Lerp(mat.color, targetColor, Time.deltaTime);
                }
            }
        }
    }

}
