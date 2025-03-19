using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static Define;


public class GameScene : BaseScene
{
    public bool m_isEquipVR;
    public Transform m_PlayerHansTransform;
    public GameObject m_XRDeivce;

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.BattleRoom;


    }

    public void Start()
    {
        if(m_isEquipVR)
        {
            if(m_PlayerHansTransform != null)
              m_PlayerHansTransform.position = new Vector3(0, -0.1f, 0);
            m_XRDeivce.SetActive(false);
        }
        else
        {
            if(m_PlayerHansTransform != null)
                m_PlayerHansTransform.position = new Vector3(0, 1.2f, 0);
            m_XRDeivce.SetActive(true);

        }
    }

    public override void Clear()
    {
        
    }
}
