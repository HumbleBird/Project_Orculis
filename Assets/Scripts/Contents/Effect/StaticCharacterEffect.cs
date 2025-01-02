using UnityEngine;

public abstract class StaticCharacterEffect : ScriptableObject
{
    public int effectID;

    // Static Effect
    // 

    public abstract void AddStaticEffect(PlayerManager character);

    public abstract void RemoveStaticEffect(PlayerManager character);
}