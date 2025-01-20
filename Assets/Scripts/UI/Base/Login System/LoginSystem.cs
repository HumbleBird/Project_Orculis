using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static Define;
using System.Threading.Tasks;

public class LoginSystem : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField password;

    public TextMeshProUGUI outputText;

    // Start is called before the first frame update
    void Start()
    {
        Managers.Auth.LoginState += OnChangedState;
        Managers.Auth.Init();
    }

    private void OnChangedState(bool sign)
    {
        outputText.text = sign ? " Login : " : " LogOut : ";
        outputText.text += Managers.Auth.UserId;
    }

    public void Create()
    {
        string e = email.text;
        string p = password.text;

        Managers.Auth.Create(e, p);
    }

    public async void Login()
    {
        // TODO 비동기 Task로 실행
        bool result = await Managers.Auth.Login(email.text, password.text);

        // if awakt 시간이 5초 이상이면 정지

        // 성공 시 씬 이동
        if(result)
        {
            Managers.Scene.LoadingSceneQueueNextScene(Scene.Game);
        }
    }

    public void LogOut()
    {

        Managers.Auth.LogOut();
    }


}
