using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DevelopUI : MonoBehaviour
{
    public PlayerManager m_PlayerManager;
    public TMP_Dropdown dropDown;
    public Toggle m_Toggle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Unlock된 스펠 리스트 가져오기

        // Drop 옵션 클리어
        dropDown.ClearOptions();

        List<TMP_Dropdown.OptionData> optionList = new List<TMP_Dropdown.OptionData>();

        var spels = m_PlayerManager.m_PlayerMagicManager.m_UnlockSpells;

        foreach ( var spell in spels)
        {
            optionList.Add(new TMP_Dropdown.OptionData(spell.ToString()));
        }

        // 위에서 생성한 optionList를 dropdown의 옵션 값에 추가
        dropDown.AddOptions(optionList);

        // 현재 dropdown에 선택된 옵션을 0번으로 설정
        dropDown.value = 0;
    }
}
