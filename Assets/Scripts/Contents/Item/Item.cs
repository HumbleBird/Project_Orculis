using UnityEngine;
using static Define;
using Image = UnityEngine.UI.Image;

// 처음에는 ScriptableObject로 사용하다
// DB로 데이터 가져오기

public struct strAdditionalStat
{
    // public int FixedMagicDamage                   ;  // 고정 마법 피해
    // public float MemoryCapacityBonus              ;  // 기억 용량 보너스
    // public float MagicPenetration                 ;  // 마법 관통력
    // public float MagicResistance                  ;  // 마법 저항력
    // public int MagicHealing                       ;  // 마법 치유량
    // public int MagicDamageReduction               ;  // 마법 피해 감소
    // public float MagicDamageBonus                 ;  // 마법 피해 보너스
    // public int Agility                            ;  // 민첩
    // public int DefenseRating                      ;  // 방어 등급
    // public float ArmorPenetration                 ;  // 방어구 관통력
    // public int Willpower                          ;  // 의지
    // public float MovementSpeedBonus               ;  // 이동 속도 보너스
    // public float BeneficialEffectDurationBonus    ;  // 이로운 효과 지속시간 보너스 
    // public float SpellCastingSpeed                ;  // 주문 시전 속도
    // public int SpellPower                         ;  // 주문력
    // public int Knowledge                          ;  // 지식
    // public int MaximumHealth                      ;  // 최대 체력
    // public float MaximumHealthBonus               ;  // 최대 체력 보너스
    // public int AdditionalMemoryCapacity           ;  // 추가 기억 용량
    // public int AdditionalMagicDamage              ;  // 추가 마법 피해
    // public int AdditionalMovementSpeed            ;  // 추가 이동 속도
    // public int ProjectileDamageReduction          ;  // 투사체 피해 감소
    // public float HarmfulEffectDurationBonus       ;  // 해로운 효과 지속시간 보너스
    // public int ActionSpeed                        ;  // 행동 속도
    // public int Vitality;                             // 활력 
}

public class Item : ScriptableObject
{
    public int ItemDbId ;
    public int TemplateId ;
    public string Name ;
    public string Description ;
	public int Count ;
	public int Slot ;
    public Image Icon ;
    // Prefab Path
    public string Path ; 

    public GameObject PrefabObject ; // TODO Del
    public GameObject TempObject; // ON/Off 전용

    public E_ItemType ItemType { get; protected set; }
    public E_ItemElement ItemElement ;
    public bool IsStackable ;
    public bool IsEquipped ;

    public Item(E_ItemType Type)
    {
        ItemType = Type;
    }

    public virtual void Init()
    {
        // ScriptableObject일 
        // 아이템 타입에 따라 리소스에서 가져오기
    }

    // 나중에 데이터 부분에서 빠지기
    public void IsEquip(bool isEquip)
    {
        if (isEquip)
        {
            IsEquipped = true;
            TempObject.SetActive(true);
        }
        else
        {
            IsEquipped = false;
            TempObject.SetActive(false);
        }
    }
}

[CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
public class Weapon : Item
{
    public E_WeaponType E_WeaponType ;
    public int Damage ;
    public strAdditionalStat AdditionalStat ;
    [HideInInspector] public MagicItem m_MagicEquippmentObject;

    public Weapon() : base(E_ItemType.Weapon)
    {
        Init();
    }

    public override  void Init()
    {

        ItemType = E_ItemType.Weapon;
        IsStackable = false;
        Count = 1;

        base.Init();
    }
}

[CreateAssetMenu(fileName = "Item", menuName = "Item/Armor")]
public class Armor : Item
{
    public E_ArrmorType ArmorType ;
    public int DefenceGrade ;
    public strAdditionalStat AdditionalStat ;

    public Armor() : base(E_ItemType.Armor)
    {
        Init();
    }

    public override void Init()
    {
        ItemType = E_ItemType.Armor;
        IsStackable = false;
        Count = 1;

        base.Init();
    }
}

[CreateAssetMenu(fileName = "Item", menuName = "Item/Accessorie")]
public class Accessorie : Item
{
    public E_AccessoriesType AccessorieType ;
    public strAdditionalStat AdditionalStat ;

    public Accessorie() : base(E_ItemType.Accessorie)
    {
        Init();
    }

    public override void Init()
    {
        ItemType = E_ItemType.Accessorie;
        IsStackable = false;
        Count = 1;

        base.Init();
    }
}

[CreateAssetMenu(fileName = "Item", menuName = "Item/Consumable")]
public class Consumable : Item
{
    public E_ConsumableType ConsumableType ;
    public int MaxCount ;

    public Consumable() : base(E_ItemType.Consumable)
    {
        Init();
    }

    public override void Init()
    {
        ItemType = E_ItemType.Consumable;
        IsStackable = true;
        Count = 1;

        base.Init();
    }
}
