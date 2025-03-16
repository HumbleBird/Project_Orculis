using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class UI_SkillInfoList : MonoBehaviour
{
    public GameObject m_CreateSkillInfo;
    public GameObject m_PrefabSkillInfo;
    public List<UI_SkillInfo> uI_SkillInfos = new List<UI_SkillInfo>();
    public Transform m_HandTransform;

    public Transform  headset;
    public GameObject target;

    public float thresholdAngle = 30f;
    public float thresholdDuraction = 2f;

    private bool isLooking = false;
    private float showingTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefreshUI();
        transform.position = m_HandTransform.position;
    }

    private void Update()
    {
        FollwPosition();
        ActivateOnLookat();
    }

    public  void RefreshUI()
    {
        // 1. 플레이어 스킬 목록 가져오기
        Player player = GetComponentInParent<Player>();
        List<SpellBase> spells = player.m_PlayerMagicManager.m_UnlockSpells;

        if (spells.Count <= 0)
        {
            Debug.Log("현재 플레이어의 허용된 마법이 없습니다.");
            return;
        }

        // 2. 초기화
        foreach (Transform child in m_CreateSkillInfo.transform)
            Managers.Resource.Destroy(child.gameObject);

        uI_SkillInfos.Clear();

        // 3.
        // Skill Info Sub Item을 각 스킬 마다 배부
        // UnLock된 플레이어 스킬 만큼 자식 산하의 Skill Info Sub Item 생성하기
        for (int i = 0; i < spells.Count; i++)
        {
            GameObject go = Managers.Resource.Instantiate(m_PrefabSkillInfo, m_CreateSkillInfo.transform);
            UI_SkillInfo slot = go.GetOrAddComponent<UI_SkillInfo>();
            slot.m_Spell = spells[i];
            slot.m_iSloNum = i;

            slot.SetInfo();
        }
    }

    public void FollwPosition()
    {
        transform.position = Vector3.Lerp(transform.position, m_HandTransform.position, Time.deltaTime);
    }

    private void ActivateOnLookat()
    {
        var dir = target.transform.position - headset.transform.position;
        var angle = Vector3.Angle(headset.transform.forward, dir);

        if(angle <= thresholdAngle)
        {
            if(!isLooking)
            {
                isLooking = true;
                showingTime = Time.time + thresholdDuraction;
            }
            else
            {

                if(!target.activeSelf && Time.time>= showingTime)
                {
                    target.SetActive(true);
                }
            }
        }
        else
        {
            if(isLooking)
            {
                isLooking = false;
                target.SetActive(false);
            }
        }
    }
}
