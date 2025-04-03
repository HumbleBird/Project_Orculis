using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UI_SkillInfo : UI_Base
{
    public SpellBase m_Spell;

    [Header("UI Property")]
    public int m_iSloNum = -1;
    public Image m_Icon;
    public TextMeshProUGUI m_TextName;
    public TextMeshProUGUI m_Description;
    public TextMeshProUGUI m_SkillCondition;
    public TextMeshProUGUI m_SkillCoolTimeNumber;
    public Image m_SkillCoolTimeImage;
    public VideoPlayer m_VideoPlayer;

    [Header("Render Texture")]
    public RawImage m_RawImage;

    public void SetInfo()
    {
        if(m_Spell == null)
        {
            Debug.Log($"{m_Spell}의 Spell이 없습니다. m_iSlotum : {m_iSloNum}");
            return;
        }

        name = $"UI_SkillInfo ({m_Spell.spellName})";

        m_Icon.sprite = m_Spell.m_icon;
        m_TextName.text = m_Spell.spellName;
        m_Description.text = m_Spell.m_sDetailDescription;
        m_SkillCondition.text = m_Spell.m_sConditionDescription;

        m_SkillCoolTimeNumber.text = ((int)m_Spell.m_CooldownRemain).ToString();
        m_SkillCoolTimeImage.fillAmount = Mathf.Abs(1 - m_Spell.m_CooldownProgress);

        m_SkillCoolTimeNumber.enabled = false;
        m_SkillCoolTimeImage.enabled = false;

        m_VideoPlayer.clip = m_Spell.m_UseVideoClip;
    }

    public void Update()
    {
        if (m_Spell == null)
            return;

        // Check CoolTime
        // 스킬 사용 => 쿨타임 시간 체크
        if(m_Spell.m_CooldownProgress > 0 && m_Spell.m_CooldownProgress > 1)
        {
            m_SkillCoolTimeNumber.enabled = true;
            m_SkillCoolTimeImage.enabled = true;

            m_SkillCoolTimeNumber.text = ((int)m_Spell.m_CooldownRemain).ToString();
            m_SkillCoolTimeImage.fillAmount = Mathf.Abs(1 - m_Spell.m_CooldownProgress);

        }
        // 스킬의 쿨타임이 완료
        else
        {

            m_SkillCoolTimeNumber.enabled = false;
            m_SkillCoolTimeImage.enabled = false;

        }
    }
}
