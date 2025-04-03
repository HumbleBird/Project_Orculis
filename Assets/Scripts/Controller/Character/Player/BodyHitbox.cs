using Fusion;
using Oculus.Interaction.PoseDetection;
using UnityEngine;

public class BodyHitbox : Hitbox
{
    public Transform m_AttackBodyPart;
    public Vector3 m_OffsetRotation;

    // Update is called once per frame
    public void Update()
    {
        transform.position = m_AttackBodyPart.position;
        transform.rotation = m_AttackBodyPart.rotation *Quaternion.Euler(m_OffsetRotation);
    }
}
