using UnityEngine;

public abstract class StaticCharacterEffect : ScriptableObject
{
    public int effectID;

    // Static Effect
    // 

    public abstract void AddStaticEffect(Player character);

    public abstract void RemoveStaticEffect(Player character);
}