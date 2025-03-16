using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UI_Lobby : UI_Scene
{
    public UI_SelectChoise m_UISelectChoise;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }
}
