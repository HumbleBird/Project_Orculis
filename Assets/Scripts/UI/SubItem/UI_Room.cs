using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

// UI Room 정보를 Photon에서 받아와서 Instantite로 보여줌
// 버튼 클릭시 이동
public class UI_Room : UI_Base  
{
    public string m_sRoomName;
    public int m_iMaxCount;
    public int m_iCurrentCount;
    public bool m_iIsSeal; // 방 잠금인지 아닌지

    // 버튼을 누르면 해당 방으로 입장
    public void EnterRoom()
    {
        // Check Count
        if (m_iCurrentCount == m_iMaxCount)
            return;
    }
}


