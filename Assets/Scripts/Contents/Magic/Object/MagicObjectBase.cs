using Fusion;
using System.Collections;
using UnityEngine;
using static Define;

public interface IMoveable
{
    public bool CanControlMagicObject();
}



// 마법으로부터 영향을 받는 모든 오브젝트.
// ex) 마법으로 움직이게 된 물체
// ex) 마법으로 생성된 파이어 볼
public abstract class MagicObjectBase : MonoBehaviour
{
    [Header("Ref")]
    public Player m_Owner;


}

