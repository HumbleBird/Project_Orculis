using UnityEngine;

// 플레이어로부터 구현은 되지만 떨어지지 않는 것.
// 화염 방사, 체인 라이트닝 등
public class MagicObject_Fire : MagicObjectBase
{
    [SerializeField] private float m_ColliderInitZPos = 1.5f;
    [SerializeField] private float m_ColliderDestZPos = 7;
    [SerializeField] private float m_fTimeInitToDest = 1;

    // 콜라이더 컴포넌트 참조
    private CapsuleCollider CapsuleCollider; // 예: CapsuleCollider를 사용한다고 가정

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        // CapsuleCollider 컴포넌트를 가져옵니다.
        CapsuleCollider = m_Collider as CapsuleCollider;

        // 초기 center 값 설정
        if (CapsuleCollider != null)
        {
            Vector3 initialCenter = CapsuleCollider.center;
            initialCenter.z = m_ColliderInitZPos;
            CapsuleCollider.center = initialCenter;
        }
        else
        {
            Debug.LogWarning("Collider component not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CapsuleCollider != null)
        {
            // 현재 center 값 가져오기
            Vector3 currentCenter = CapsuleCollider.center;

            // z 축 위치를 선형 보간(Lerp)으로 변경
            currentCenter.z = Mathf.Lerp(currentCenter.z, m_ColliderDestZPos, Time.deltaTime * m_fTimeInitToDest);

            // 변경된 center 값 적용
            CapsuleCollider.center = currentCenter;

            // 목표 위치에 도달했는지 확인
            if (Mathf.Abs(currentCenter.z - m_ColliderDestZPos) < 0.1f)
            {
                //managers.Resource.Destroy(gameObject);
            }
        }
    }

}
