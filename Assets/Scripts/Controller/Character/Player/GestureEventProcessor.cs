using UnityEngine;

public class GestureEventProcessor : MonoBehaviour
{
    PlayerManager m_PlayerManager;

    public bool m_bIsFirstRecognition = false;
    public Mivry m_Mivry;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_PlayerManager = GetComponent<PlayerManager>();
    }

    public void OnGestureCompleted(GestureCompletionData data)
    {
        if(data.gestureID < 0)
        {
            string errMsg = GestureRecognition.getErrorMessage(data.gestureID);
            Debug.Log(errMsg);
            return;
        }

        m_PlayerManager.CheckRecognition(data);
    }
}
