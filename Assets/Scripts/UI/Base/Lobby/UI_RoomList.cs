using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UI_RoomList : UI_Base
{
    public Transform m_roomParent;

    List<UI_Room> m_Rooms = new List<UI_Room>();

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        // 현재 열려 있는 게임 룸의 정보들을 가져오기
        //var list = Managers.Game.GetRoomList();

        CreateRoomList();

        return true;
    }

    public void CreateRoomList()
    {
        foreach (Transform child in m_roomParent)
            Managers.Resource.Destroy(child.gameObject);

        m_Rooms.Clear();

    }
}
