using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [Header("Damage Indicator")]
    [SerializeField] private MeshRenderer _ShipMr;
    [SerializeField] private Color _DamageColor;
    [SerializeField] private float _IndicatorTime = 0.15f;
    private Color _OriginalColor;

    private void Awake()
    {
        _OriginalColor = _ShipMr.material.color;
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