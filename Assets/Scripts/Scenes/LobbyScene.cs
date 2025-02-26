using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    public AudioClip m_EnterenceClip;
    public AudioSource m_Source;

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Lobby;

        m_Source = GetComponent<AudioSource>();
        m_Source.clip = m_EnterenceClip;
        m_Source.Play();
    }

    public override void Clear()
    {

    }
}
