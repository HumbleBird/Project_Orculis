using Michsky.MUIP;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Loading : MonoBehaviour
{
    public ProgressBar m_ProgressBar;
    public TextMeshProUGUI m_GameTipText;

    public float m_fIntervalTipTime = 3f;
    public AudioClip m_AudioClip;

    void Start()
    {
        Managers.Sound.Play(m_AudioClip);
        StartCoroutine(LoadSceneAsync());
        StartCoroutine(UpdateGameTip()); // 1초마다 팁 갱신
    }

    IEnumerator LoadSceneAsync()
    {
        string sceneName = System.Enum.GetName(typeof(Define.Scene), Managers.Scene.m_NextScene);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        //operation.allowSceneActivation = false; // 95%에서 멈추기 위해 false 설정

        while (!operation.isDone)
        {
            m_ProgressBar.currentPercent = operation.progress * 100;
            // 90%에서 5초 멈추기
            if (operation.progress >= 0.85f)
            {

                //Debug.Log("Almost Redady");
                //m_UIManagerProgressBar.label.text = "Almost Ready...";
                yield return new WaitForSeconds(5f);
                Managers.Scene.m_NextScene = Define.Scene.Unknown;
                //operation.allowSceneActivation = true; // 씬 이동 허용
            }

            yield return null;
        }

    }

    IEnumerator UpdateGameTip()
    {
        while (true)
        {
            m_GameTipText.text = Managers.Game.GetTextTip();
            yield return new WaitForSeconds(m_fIntervalTipTime); // 1초마다 갱신
        }
    }
}
