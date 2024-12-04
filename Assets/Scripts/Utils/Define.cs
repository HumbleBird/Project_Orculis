using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum E_CastHand
    {
        RightHand,
        LeftHand,
        TwoHand
    }

    public interface IHitable
    {
        // TODO 어떤 마법, 어떤 각도, 적중 부위
        void OnHit(PlayerManager Attacker, int damage);
    }

    #region MagicSpell

    public enum E_SpellType
    {
        Attack,
        Defense,
        Cure,
        Object
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
