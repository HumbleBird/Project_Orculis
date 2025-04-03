using Fusion;
using Oculus.Interaction;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class UI_SkillInfoList : MonoBehaviour
{
    public GameObject m_CreateSkillInfo;
    public GameObject m_PrefabSkillInfo;
    public List<UI_SkillInfo> uI_SkillInfos = new List<UI_SkillInfo>();

    [Header("Transfrom")]
    public Transform m_DestTransform;
    public Vector3 m_DestOffestPosition;
    public Vector3 m_DestOffesRotation;

    [Header("Camera")]
    public List<RenderTexture> renderTextures = new List<RenderTexture>();

    public Transform  headset;
    public GameObject target;
    public Player m_Player;

    public float thresholdAngle = 30f;
    public float thresholdDuraction = 2f;

    private bool isLooking = false;
    private float showingTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefreshUI();

        m_Player = GetComponentInParent<Player>();
        if(m_Player.HasInputAuthority == false)
            gameObject.SetActive(false);
        else
        {
            gameObject.SetActive(true);
            transform.position = m_DestTransform.position;
        }
    }

    private void Update()
    {
        //FollwPosition();
        ActivateOnLookat();
    }

    public  void RefreshUI()
    {
        // 1. 플레이어 스킬 목록 가져오기
        Player player = GetComponentInParent<Player>();
        var spells = player.m_PlayerMagicManager.UnlockSpellGet();

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
            slot.m_RawImage.texture = renderTextures[i];
            slot.m_VideoPlayer.targetTexture = renderTextures[i];

            slot.SetInfo();
        }
    }

    public void FollwPosition()
    {
        Vector3 dest = m_DestTransform.position + m_DestOffestPosition;
        Quaternion rotation = m_DestTransform.rotation * Quaternion.Euler(m_DestOffesRotation);

        transform.position = dest;
        transform.rotation = rotation;
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
