using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using debug = UnityEngine.Debug;

public class UI_Loading2 : MonoBehaviour
{
    public UIManagerProgressBar m_UIManagerProgressBar;
    public TextMeshProUGUI m_GameTipText;

    Stopwatch m_stopwatch = new Stopwatch();

    void Start()
    {
        StartCoroutine(LoadSceneAsync());

        m_stopwatch.Reset();
        m_stopwatch.Start();
    }

    IEnumerator LoadSceneAsync()
    {
        string name = System.Enum.GetName(typeof(Define.Scene), Managers.Scene.m_NextScene);

        AsyncOperation operation = SceneManager.LoadSceneAsync(name);

        //operation.allowSceneActivation = false; 

        // Show Tip
        string todoShowtip = Managers.Game.GetTextTip();
        m_GameTipText.text = todoShowtip.ToString();

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            m_UIManagerProgressBar.bar.fillAmount = progressValue;
            m_UIManagerProgressBar.label.text = progressValue.ToString();

            if (m_stopwatch.ElapsedMilliseconds > 5f)
            {
                // Show Tip
                todoShowtip = Managers.Game.GetTextTip();
                m_GameTipText.text = todoShowtip.ToString();

                m_stopwatch.Reset();
                m_stopwatch.Start();
            }

            // Pause at 95% for 5 seconds
            //if (progressValue >= 0.90f && !operation.allowSceneActivation)
            //{
            //    m_UIManagerProgressBar.label.text = "Almost Ready...";
            //    yield return new WaitForSeconds(5f);
            //    operation.allowSceneActivation = true; // Allow the scene to finish loading
            //}

            yield return null;
        }

        Managers.Scene.m_NextScene = Define.Scene.Unknown;
    }
}

