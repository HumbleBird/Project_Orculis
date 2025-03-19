using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using static Define;

public class EnterRoom : XRBaseInteractable
{
    public AudioClip m_BtnClickClip;
    public AudioClip m_ShowMenuUIAudioClip;
    public GameObject m_EnterRoomUI;

    public void Start()
    {
        m_EnterRoomUI.SetActive(false);

    }

    public void ShowMenuUI()
    {
        m_EnterRoomUI.SetActive(true);
        Managers.Sound.Play(m_BtnClickClip);
        Debug.Log("ShowRoomUI");
    }

    public void CloseMenuUI()
    {
        Managers.Sound.Play(m_BtnClickClip);
        Debug.Log("CloseMenuUI");
    }


    public void CreateRoom()
    {
        m_EnterRoomUI.SetActive(false);
        Managers.Sound.Play(m_BtnClickClip);
        Debug.Log("CreateRoom");
    }

    public void JoinRoom()
    {
        Managers.Sound.Play(m_BtnClickClip);
        Debug.Log("JoinRoom");

    }

    public void QuickMatch()
    {
        Managers.Sound.Play(m_BtnClickClip);
        Debug.Log("QuickMatch");
        Managers.Scene.LoadingSceneQueueNextScene(Scene.BattleRoom);
    }

}
