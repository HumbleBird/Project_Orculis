using Fusion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerSoundManager : NetworkBehaviour
{
    [Header("Ref")]
    public Player m_Player;

    public AudioClip[] m_DamageHumanSounds;

    public override void Spawned()
    {
        m_Player = GetComponent<Player>();
    }

    public void PlayHitSounds()
    {
        Managers.Sound.RandomPlay(m_DamageHumanSounds);
    }
}
