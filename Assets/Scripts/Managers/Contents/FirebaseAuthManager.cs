using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using TMPro;
using System;
using static Define;
using Autodesk.Fbx;
using System.Threading.Tasks;

public class FirebaseAuthManager
{
    private FirebaseAuth auth; // 로그인 / 회원가입 등에 사용
    private FirebaseUser user; // 인증이 완료된 유저 정보

    public string UserId => user.UserId;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;

        if (auth.CurrentUser != null)
        {
            LogOut();
        }

        auth.StateChanged += OnChanged;
    }

    public Action<bool> LoginState;

    private void OnChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            bool signed = (auth.CurrentUser != user && auth.CurrentUser != null);
            if (!signed && user != null)
            {
                Debug.Log("로그아웃");
                LoginState?.Invoke(false);
            }

            user = auth.CurrentUser;
            if (signed)
            {
                Debug.Log("로그인");
                LoginState?.Invoke(true);
            }
        }
    }

    public void Create(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("회원가입 취소");
                return;
            }
            if (task.IsFaulted)
            {
                // 실패 이유 -> 이메일이 비정상 / 비밀번호가 너무 간단 / 이미 가입된 이메일 등등
                Debug.LogError("회원가입 실패");
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.LogError("회원가입 완료");
        });
    }

    public async Task<bool> Login(string email, string password)
    {
        return await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("로그인 취소");
                return false;
            }
            if (task.IsFaulted)
            {
                // 실패 이유 -> 이메일이 비정상 / 비밀번호가 너무 간단 / 이미 가입된 이메일 등등
                Debug.LogError("로그인 실패");
                return false;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.LogError("로그인 완료");
            return true;
        });
    }

    public void LogOut()
    {
        auth.SignOut();
        Debug.Log("로그 아웃");
    }
}
