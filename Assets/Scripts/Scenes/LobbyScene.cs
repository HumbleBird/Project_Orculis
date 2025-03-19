using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    public AudioClip m_EnterenceClip;
    public AudioClip m_BgmClip;
    public AudioClip m_BattleRoomEnterClip;

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Lobby;

        Managers.Sound.Play(m_EnterenceClip);
        Managers.Sound.Play(m_BgmClip, 1, Define.Sound.Bgm);
    }

    public override void Clear()
    {

    }

    public void PlayClip_BattleRoomEnter()
    {
        Managers.Sound.Play(m_BattleRoomEnterClip);
    }

}
