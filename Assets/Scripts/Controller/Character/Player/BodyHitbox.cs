using Fusion;
using Oculus.Interaction.PoseDetection;
using UnityEngine;

public class BodyHitbox : Hitbox
{
    public Transform m_AttackBodyPart;

    // Update is called once per frame
    void Update()
    {
        transform.position = m_AttackBodyPart.position;
        transform.rotation = m_AttackBodyPart.rotation;
    }
}
