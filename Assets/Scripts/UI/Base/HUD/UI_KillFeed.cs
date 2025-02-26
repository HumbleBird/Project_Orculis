using UnityEngine;
using static Define;

    public class UI_KillFeed : MonoBehaviour
    {
        public UIKillFeedItem KillFeedItemPrefab;
        public float ItemLifetime = 6f;
        public Sprite[] WeaponIcons;

        public void ShowKill(string killer, string victim, E_WeaponType weaponType)
        {
            var item = Instantiate(KillFeedItemPrefab, transform);

            item.Killer.text = killer;
            item.Victim.text = victim;
            item.WeaponIcon.sprite = WeaponIcons[(int)weaponType];

            // Kill feed item is fading in time automatically via animation component.
            // Make sure the item gets destroyed after the animation is done.
            Destroy(item.gameObject, ItemLifetime);
        }
    }
