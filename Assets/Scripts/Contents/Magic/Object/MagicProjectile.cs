using Fusion;
using FusionHelpers;
using UnityEngine;

public class MagicProjectile : MagicObjectBase
{
    [SerializeField]
    private GameObject _hitEffect;
    [SerializeField]
    private GameObject _visualsRoot;

    private bool _hitEffectVisible;

    public void ShowHitEffect()
    {
        if (_hitEffectVisible == true)
            return;

        if (_hitEffect != null)
        {
            _hitEffect.SetActive(true);
        }

        if (_visualsRoot != null)
        {
            _visualsRoot.SetActive(false);
        }

        _hitEffectVisible = true;
    }
}
