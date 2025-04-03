using GLTFast.Schema;
using System.Collections.Generic;
using UnityEngine;
using Material = UnityEngine.Material;

public class IndicatorHP : Indicator
{
    public Material m;
    public Color c;
    public  override void Start()
    {
        base.Start();

         m = GetComponent<Renderer>().material;
    }

    public void FixedUpdate()
    {
        int currentHealth = m_Player.m_PlayerStatesManager.m_CurrentHealth;
        int maxHealth = m_Player.m_PlayerStatesManager.m_MaxHealth;

        float ratio = (float)currentHealth / maxHealth;

        c = new Color(ratio, ratio, ratio);

        m.color = Color.Lerp(m.color, c, Time.deltaTime);
    }
}
