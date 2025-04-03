public partial class Define
{
    #region Interface
    public interface IHitable
    {
        // TODO 어떤 마법, 어떤 각도, 적중 부위
        void OnHit(Player Attacker, int damage);
    }

    #endregion

    #region Network

    public enum RigPart
    {
        None,
        Headset,
        LeftController,
        RightController,
        Undefined
    }

    public enum RunnerExpectations
    {
        NoRunner, // For offline usages
        PresetRunner,
        DetectRunner // should not be used in multipeer scenario
    }

    public enum EGameplayState
    {
        Skirmish = 0,
        Running = 1,
        Finished = 2,
    }

    #endregion

    #region Item

    public enum E_ItemType
    {
        None = 0,
        Weapon = 1,
        Armor = 2,
        Accessorie = 3,
        Consumable = 4
    }

    public enum E_WeaponType
    {
        None = 0,
        Staff = 1,
        MagicBook = 2,
        CrystalSwrod = 3,
        CrystalOrb = 4,
    }

    public enum E_ArrmorType
    {
        None = 0,
        Head  ,
        Chest ,
        Hands ,
        Legs  ,
        Shoose
    }

    public enum E_AccessoriesType
    {
        None,
        Rings,
        Necklaces,
        Cloak
    }

    public enum E_ConsumableType
    {
        None,
        MagicReagent,
        Throwing,
        SummoningStone
    }

    public enum E_ItemGrade
    {
        Junk,
        Poor,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Unique
    }

    public enum E_ItemElement
    {
        None,
        Fire,
        Water,
        Wind,
        Earth,
        Electricity,
        Dark,
        Light,
    }

    #endregion

    #region Spell

    public enum E_SpellActivation
    {
        Instant, // 즉발성
        Continuous // 지속성
    }

    public enum E_SpellCheckType
    {
        Chant,
        Motion
    }

    #endregion

    public enum E_CharacterEffectType
    {
        DrainMana = 0,

    }

    public enum E_CastHand
    {
        RightHand,
        LeftHand,
        TwoHand
    }



    #region MagicSpell

    public enum E_SpellType
    {
        // 제어 
        Control,

        // 위력
        Force,

        // 피해
        Damage,

        // 필수
        Essential,

        // 변환
        Transfiguration,

        // 용서받지 못할 저주
        UnforgiveableCurse,

        // 유용
        Utility
    }

    #endregion

    #region Base

    public enum E_TeamId
    {
        Player = 0,
        Monster = 1,
        NPC = 2,
    }

    public enum E_RandomSoundType
    {
        Damage,
        Block,
        WeaponWhoose
    }

    public enum Scene
    {
        Unknown = 0,
        Start = 1,
        Lobby = 2,
        BattleRoom = 3,
        Login = 4,
        Loading = 5,
    }

    public enum Sound
    {
        Bgm = 0,
        Effect = 1,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
        
    }

    public enum CursorType
    {
        None,
        Arrow,
        Hand,
        Look,
    }
    #endregion
}
