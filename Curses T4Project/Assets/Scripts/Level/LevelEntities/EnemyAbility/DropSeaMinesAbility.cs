//DropSeaMines.cs
//by ANTHONY FEDELI

using UnityEngine;

/// <summary>
/// DropSeaMines.cs give to the gameobject the ability of drop a bunch of SeaMines into the ocean.
/// </summary>

public class DropSeaMines : MonoBehaviour
{
    [SerializeField] private GameObject _SeaMine;
    [SerializeField] private Transform _MineDropLocation;
    [SerializeField] private bool _IsMineCounted = false;
    [SerializeField] private int _MineToDrop = 3;
    [SerializeField] private float _TimeBetweenDrop = 1;

    private float _DropTimer;

    private void Awake()
    {
        _DropTimer = _TimeBetweenDrop;
    }

    private void Update()
    {
        DropSea();
    }

    private void DropSea()
    {
        if (!gameObject.GetComponent<EnemyShip>().IsStopped && !gameObject.GetComponent<EnemyShip>().IsDead && (gameObject.transform.position.x < 15 && gameObject.transform.position.x > 0))
        {
            if (_IsMineCounted)
            {
                if (_MineToDrop > 0)
                {
                    _DropTimer += Time.deltaTime;
                    if (_DropTimer >= _TimeBetweenDrop )
                    {
                        _DropTimer = 0;
                        _MineToDrop--;
                        Instantiate(_SeaMine, _MineDropLocation);
                    }
                }
            }
            else
            {
                _DropTimer += Time.deltaTime;
                if (_DropTimer >= _TimeBetweenDrop)
                {
                    _DropTimer = 0;
                    Instantiate(_SeaMine, _MineDropLocation);
                }
            }
        }
    }
}