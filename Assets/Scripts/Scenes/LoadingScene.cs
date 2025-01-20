using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Loading;
    }

    public override void Clear()
    {
    }
}
