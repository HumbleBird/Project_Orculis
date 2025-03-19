using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using static Define;

public class EnterRoom : XRBaseInteractable
{
    public AudioClip m_BtnClickClip;
    public AudioClip m_ShowMenuUIAudioClip;
    public GameObject m_EnterRoomUI;

    public bool m_bIsShowingEnterRoomUI = false;

    public void Start()
    {
        m_EnterRoomUI.SetActive(false);
    }

    public void SelectMenuObject()
    {
        m_bIsShowingEnterRoomUI = !m_bIsShowingEnterRoomUI;
        ShowAndCloseMenuUI();
    }

    public void ShowAndCloseMenuUI()
    {
        if(!m_bIsShowingEnterRoomUI)
        {
            m_EnterRoomUI.SetActive(false);
            Managers.Sound.Play(m_BtnClickClip);
            Debug.Log("CloseMenuUI");
        }
        else
        {
            m_EnterRoomUI.SetActive(true);
            Managers.Sound.Play(m_BtnClickClip);
            Debug.Log("ShowRoomUI");
        }
    }

    public void CreateRoom()
    {
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
