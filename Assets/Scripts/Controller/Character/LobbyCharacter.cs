using UnityEngine;

public class LobbyCharacter : MonoBehaviour
{
    // 로비에서 잠시 솔플로 쓸 때 사용하는 스크립트
    // 나중에 네트워크로 변경하면 삭제 하고 Network Rig로 배틀룸과 똑같이 적용해버리기 지금은 임시로

    public Transform m_HardRigHeadset;
    public Transform m_ModelTransform;
    public Vector3 m_Offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_ModelTransform.position = m_HardRigHeadset.position + m_Offset;

        Vector3 newRotation = m_ModelTransform.rotation.eulerAngles; // 현재 회전을 Euler 각도로 변환
        newRotation.y = m_HardRigHeadset.rotation.eulerAngles.y; // y축 값만 변경
        m_ModelTransform.rotation = Quaternion.Euler(newRotation); // 다시 Quaternion으로 변환하여 적용
    }
}
