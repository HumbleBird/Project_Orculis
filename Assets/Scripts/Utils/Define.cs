using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    #region Interface
    public interface IHitable
    {
        // TODO 어떤 마법, 어떤 각도, 적중 부위
        void OnHit(PlayerManager Attacker, int damage);
    }

    #endregion

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
        Game = 3,
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
