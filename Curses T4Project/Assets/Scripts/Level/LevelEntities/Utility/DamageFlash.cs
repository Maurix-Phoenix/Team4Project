//DamageFlash.cs
//by ANTHONY FEDELI

using UnityEngine;

/// <summary>
/// DamageFlash.cs is required for visualize the damage inflict to the player.
/// </summary>

public class DamageFlash : MonoBehaviour
{
    [Header("Damage Indicator")]
    [SerializeField] private MeshRenderer _ShipMr;
    [SerializeField] private Color _DamageColor;
    [SerializeField] private float _IndicatorTime = 0.15f;
    private Color _OriginalColor;

    private void OnEnable()
    {
        _OriginalColor = _ShipMr.material.color;
    }

    private void Start()
    {
        
    }

    public void DamageIndicatorStart()
    {
        _ShipMr.material.color = _DamageColor;
        Invoke("DamageIndicatorEnd", _IndicatorTime);
    }

    public void DamageIndicatorEnd()
    {
        _ShipMr.material.color = _OriginalColor;
    }
}