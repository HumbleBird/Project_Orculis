using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindFirstObjectByType<BaseScene>(); } }
    public Define.Scene m_NextScene;

    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();

        SceneManager.LoadScene(GetSceneName(type));
    }

    // 로딩 화면 거쳤다가 다은 씬으로
    public void LoadingSceneQueueNextScene(Define.Scene nextScene)
    {
        Managers.Clear();

        SceneManager.LoadScene(GetSceneName(Define.Scene.Loading));

        m_NextScene = nextScene;
    }

    public async Task LoadSceneAsync(Define.Scene type)
    {
        string name = GetSceneName(type);

        // 비동기 씬 로드
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // 진행률 계산 (0.0f ~ 1.0f)
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            // UI 업데이트
            //m_LoadingBarFill.fillAmount = progressValue;
            //m_LoadingBarText.text = $"{progressValue * 100:F0}%";

            // 씬 로드가 끝난 경우 처리
            if (operation.progress >= 0.9f)
            {
                // 필요하다면 추가 대기 로직 삽입 가능
                await Task.Delay(500); // 예시: 0.5초 대기
                operation.allowSceneActivation = true;
            }

            await Task.Yield();
        }
    }

    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
