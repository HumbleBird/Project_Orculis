using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Base.Lobby
{
    // UI Room 정보를 Photon에서 받아와서 Instantite로 보여줌
    // 버튼 클릭시 이동
    public class UI_Room : UI_Base  
    {
        public string m_sRoomName;
        public int m_iMaxCount;
        public int m_iCurrentCount;
        public bool m_iIsSeal; // 방 잠금인지 아닌지

        public void EnterRoom()
        {

        }
    }
}


