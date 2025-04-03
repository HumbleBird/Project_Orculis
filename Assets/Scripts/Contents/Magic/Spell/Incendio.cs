using Fusion;
using FusionHelpers;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using static Define;
using static Unity.Collections.Unicode;

// Incendio 주문 클래스 - 네트워크 기반 투사체 시스템
public class Incendio : SpellBase
{
    public Transform FireTransform;

    // 투사체 속도
    [SerializeField]
    private float _speed = 50f;

    // 충돌 감지를 위한 레이어 마스크
    [SerializeField]
    private LayerMask _hitMask ;

    // 충돌 시 적용할 힘
    [SerializeField]
    private float _hitImpulse = 50f;

    // 투사체의 기본 수명
    [SerializeField]
    private float _lifeTime = 4f;

    // 충돌 후 지속 시간
    [SerializeField]
    private float _lifeTimeAfterHit = 2f;

    // 투사체 프리팹
    public MagicProjectile m_projectilePrefab;

    // 네트워크에서 동기화되는 투사체 발사 횟수
    [Networked]
    public int _fireCount { get; set; }

    // 네트워크에서 동기화되는 투사체 데이터 배열 (최대 64개 저장 가능)
    [Networked, Capacity(64)]
    private NetworkArray<ProjectileData> _projectileData { get; }

    // 로컬에서 관리하는 투사체 배열 (네트워크와 동기화)
    public MagicProjectile[] _projectiles = new MagicProjectile[64];

    // 렌더링 시 표시할 발사 횟수
    public int _visibleFireCount;

    // 네트워크 객체가 생성될 때 실행됨
    public override void Spawned()
    {
        _visibleFireCount = _fireCount;
        m_projectilePrefab.m_Owner = m_Owner;
    }

    public override void Awake()
    {
        base.Awake();


        _hitMask = (1 << LayerMask.NameToLayer("Hitable"));
    }

    public override void Start()
    {
        // 무기의 총구 위치 가져오기
        FireTransform = m_Owner.m_PlayerEquipmentManager.m_CurrentWeapon.m_MuzzleTransform;
    }

    // 주문 시전 조건 확인 (기본 로직 유지)
    //protected override bool AttempToCastSpellCondition()
    //{
    //    return base.AttempToCastSpellCondition();
    //}

    // 주문이 성공적으로 시전될 경우 실행되는 함수
    public override void SuccessfullyCastSpell()
    {
        base.SuccessfullyCastSpell();

        // 네트워크 배열에 새로운 투사체 데이터 저장
        _projectileData.Set(_fireCount % _projectileData.Length, new ProjectileData()
        {
            FireTick = Runner.Tick,
            FirePosition = FireTransform.position,
            FireVelocity = FireTransform.forward * _speed,
            FinishTick = Runner.Tick + Mathf.RoundToInt(_lifeTime / Runner.DeltaTime),
        });

        _fireCount++; // 발사 횟수 증가
    }


    // 네트워크에서 프레임마다 실행되는 업데이트 함수
    public override void FixedUpdateNetwork()
    {
        int tick = Runner.Tick;

        // 모든 투사체 데이터를 업데이트
        for (int i = 0; i < _projectileData.Length; i++)
        {
            var data = _projectileData[i];

            if (data.IsActive == false)
                continue;
            if (data.FinishTick <= tick)
                continue;

            // 투사체 이동 및 충돌 처리
            UpdateProjectile(ref data, tick);

            _projectileData.Set(i, data);
        }
    }

    // 클라이언트에서 투사체의 렌더링을 담당하는 함수
    public override void Render()
    {
        if (_visibleFireCount < _fireCount)
        {
            VisualEffect(); // 발사 효과 적용
        }

        // 새로운 투사체 인스턴스 생성
        for (int i = _visibleFireCount; i < _fireCount; i++)
        {
            int index = i % _projectileData.Length;
            var data = _projectileData[index];

            var previousProjectile = _projectiles[index];
            if (previousProjectile != null)
            {
                Destroy(previousProjectile.gameObject);
            }

            var projectile = Instantiate(m_projectilePrefab, data.FirePosition, Quaternion.LookRotation(data.FireVelocity));

            // 멀티플레이어 모드일 경우 처리
            if (Runner.Config.PeerMode == NetworkProjectConfig.PeerModes.Multiple)
            {
                Runner.MoveToRunnerScene(projectile);
                Runner.AddVisibilityNodes(projectile.gameObject);
            }

            _projectiles[index] = projectile;
        }

        // 투사체 이동 및 상태 업데이트
        float renderTime = Object.IsProxy ? Runner.RemoteRenderTime : Runner.LocalRenderTime;
        float floatTick = renderTime / Runner.DeltaTime;

        for (int i = 0; i < _projectiles.Length; i++)
        {
            var projectile = _projectileData[i];
            var projectileObject = _projectiles[i];

            if (!projectile.IsActive || projectile.FinishTick < floatTick)
            {
                if (projectileObject != null)
                {
                    Destroy(projectileObject.gameObject);
                }
                continue;
            }

            if (projectile.HitPosition != Vector3.zero)
            {
                projectileObject.transform.position = projectile.HitPosition;
                projectileObject.ShowHitEffect();
            }
            else
            {
                projectileObject.transform.position = GetMovePosition(ref projectile, floatTick);
            }
        }

        _visibleFireCount = _fireCount;
    }

    // 투사체의 이동 및 충돌 감지 처리
    private void UpdateProjectile(ref ProjectileData projectileData, int tick)
    {
        if (projectileData.HitPosition != Vector3.zero)
            return;

        var previousPosition = GetMovePosition(ref projectileData, tick - 1f);
        var nextPosition = GetMovePosition(ref projectileData, tick);

        var direction = nextPosition - previousPosition;
        float distance = direction.magnitude;
        direction /= distance; // 방향 벡터 정규화

        var hitOptions = HitOptions.IncludePhysX | HitOptions.IgnoreInputAuthority;

        Debug.DrawRay(previousPosition, direction, Color.red, distance);


        if (Runner.LagCompensation.Raycast(previousPosition, direction, distance, Object.InputAuthority, out var hit, _hitMask, hitOptions))
        {
            projectileData.HitPosition = hit.Point;
            projectileData.FinishTick = tick + Mathf.RoundToInt(_lifeTimeAfterHit / Runner.DeltaTime);

            if (hit.Collider?.attachedRigidbody != null)
            {
                hit.Collider.attachedRigidbody.AddForce(direction * _hitImpulse, ForceMode.Impulse);
            }

            if(hit.Hitbox != null)
            {
                ApplyDamage(hit.Hitbox, hit.Point, Vector3.zero);
            }
        }
    }

    private void ApplyDamage(Hitbox enemyHitbox, Vector3 position, Vector3 direction)
    {
        var enemy = enemyHitbox.Root.GetComponentInParent<IHitable>();
        if (enemy == null)
            return;

        enemy.OnHit(m_Owner, m_iDamage);
    }

    // 특정 시간에서의 투사체 위치 계산
    private Vector3 GetMovePosition(ref ProjectileData data, float currentTick)
    {
        float time = (currentTick - data.FireTick) * Runner.DeltaTime;
        return time <= 0f ? data.FirePosition : data.FirePosition + data.FireVelocity * time;
    }

    // 주문 발사 효과 처리
    public void VisualEffect()
    {
        if (!Runner.GetVisible()) return;
        Managers.Sound.Play(m_SpellSuccessAudioClip);
    }

    // 네트워크에서 동기화되는 투사체 데이터 구조체
    private struct ProjectileData : INetworkStruct
    {
        public bool IsActive => FireTick > 0;
        public int FireTick;
        public int FinishTick;
        public Vector3 FirePosition;
        public Vector3 FireVelocity;
        public Vector3 HitPosition { get; set; }
    }
}
