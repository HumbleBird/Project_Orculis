using UnityEngine;

public class GameManagerEX
{
    public string GetTextTip()
    {
        GameTip gameTip = Managers.Resource.Load<GameTip>("Data/Loading_Tips/GameTip");

        // 새로운 팁 가져오기
        return gameTip.GetNextGameTip();
    }
}
