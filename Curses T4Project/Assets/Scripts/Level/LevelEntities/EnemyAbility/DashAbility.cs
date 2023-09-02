//DashAbility.cs
//by MAURIZIO FISCHETTI and ANTHONY FEDELI

using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

/// <summary>
/// DashAbility.cs give to the gameobject the ability of be aggroed by the player when enter in area to be charged.
/// </summary>

public class DashAbility : MonoBehaviour
{
    [Header("Aggro Conditions")]
    [SerializeField] private GameObject _AggroAlert;
    [SerializeField] private Color _NoAggroColor;
    [SerializeField] private Color _AggroColor;
    [SerializeField] private bool _AggroAtStart = false;
    [SerializeField] private bool _Aggroed = false;
    [SerializeField] private float _DashSpeedMultiplier = 2.0f;

    public bool Aggroed { get { return _Aggroed; } }

    [Header("Aggro Range")]
    [SerializeField] private bool _ShowAggroRange = true;
    [SerializeField] private float _AggroRange = 8.0f;
    [SerializeField] private float _AggroAreaHeight = 4.5f;

    [Header("Shark Pack Conditions")]
    [SerializeField] private float _RangeToAttack = 1.6f;

    private BoxCollider _SharkAggroArea;


    void Start()
    {
        if (gameObject.GetComponent<SeaMonster>() != null)
        {
            _AggroAlert.GetComponent<MeshRenderer>().material.color = _NoAggroColor;
            if (gameObject.GetComponent<SeaMonster>().IsSharkPack)
            {
                _SharkAggroArea = GetComponent<BoxCollider>();
                _SharkAggroArea.center = new Vector3(-_AggroRange / 2, 0f, 0f);
                _SharkAggroArea.size = new Vector3(_AggroRange, _AggroAreaHeight, 6f);
            }
        }

        if (_AggroAtStart)
        {
            _Aggroed = true;
        }
    }

    void Update()
    {
        Aggro();
        AggroAlert();
    }

    private void Aggro()
    {
        if (gameObject.GetComponent<SeaMonster>() != null)
        {
            //if the ray hit the player then aggro
            if (!gameObject.GetComponent<SeaMonster>().IsSharkPack && Physics.Raycast(transform.position, Vector3.left, out RaycastHit hit, _AggroRange, layerMask: LayerMask.GetMask("Player")))
            {
                _Aggroed = true;
            }

            //if in aggro multiply the speed
            if (_Aggroed)
            {
                gameObject.GetComponent<SeaMonster>().MoveSpeed = GameManager.Instance.LevelManager.CurrentLevel.LevelSpeed * _DashSpeedMultiplier;
            }
        }
        else if (gameObject.GetComponent<EnemyShip>() != null)
        {
            //if the ray hit the player then aggro
            if (Physics.Raycast(transform.position, Vector3.left, out RaycastHit hit, _AggroRange, layerMask: LayerMask.GetMask("Player")))
            {
                _Aggroed = true;
            }

            //if in aggro multiply the speed
            if (_Aggroed)
            {
                gameObject.GetComponent<EnemyShip>().MoveSpeed = GameManager.Instance.LevelManager.CurrentLevel.LevelSpeed * _DashSpeedMultiplier;
            }
        }
    }

    private void AggroAlert()
    {
        if (gameObject.GetComponent<SeaMonster>() != null)
        {
            if (_Aggroed)
            {
                _AggroAlert.GetComponent<MeshRenderer>().material.color = _AggroColor;
                _AggroAlert.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", _AggroColor);
            }
            else
            {
                _AggroAlert.GetComponent<MeshRenderer>().material.color = _NoAggroColor;
                _AggroAlert.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", _NoAggroColor);
            }
        }
    }

    private void OnValidate()
    {
        if (gameObject.GetComponent<SeaMonster>() != null)
        {
            if (gameObject.GetComponent<SeaMonster>().IsSharkPack)
            {
                _SharkAggroArea = GetComponent<BoxCollider>();
                _SharkAggroArea.center = new Vector3(-_AggroRange / 2, 0f, 0f);
                _SharkAggroArea.size = new Vector3(_AggroRange, _AggroAreaHeight, 6f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.GetComponent<SeaMonster>() != null && gameObject.GetComponent<SeaMonster>().IsSharkPack)
        {
            _SharkAggroArea = GetComponent<BoxCollider>();
            _SharkAggroArea.center = new Vector3(-_RangeToAttack / 2, 0f, 0f);
            _SharkAggroArea.size = new Vector3(_RangeToAttack, _AggroAreaHeight, 6f);
        }
        _Aggroed = true;
    }

    private void OnDrawGizmos()
    {
        if (_ShowAggroRange)
        {
            Gizmos.color = Color.yellow;
            if (gameObject.GetComponent<SeaMonster>() != null)
            {
                if (!gameObject.GetComponent<SeaMonster>().IsSharkPack)
                {
                    Gizmos.DrawLine(transform.position, new Vector3(transform.position.x - _AggroRange, transform.position.y, transform.position.z));
                }
                else
                {
                    if (_Aggroed)
                    {
                        _SharkAggroArea = GetComponent<BoxCollider>();
                        _SharkAggroArea.center = new Vector3(-_RangeToAttack / 2, 0f, 0f);
                        _SharkAggroArea.size = new Vector3(_RangeToAttack, _AggroAreaHeight, 6f);
                    }
                    else
                    {
                        _SharkAggroArea = GetComponent<BoxCollider>();
                        _SharkAggroArea.center = new Vector3(-_AggroRange / 2, 0f, 0f);
                        _SharkAggroArea.size = new Vector3(_AggroRange, _AggroAreaHeight, 6f);
                    }
                    Gizmos.DrawLine(transform.position + new Vector3(0f, _SharkAggroArea.size.y / 2f, 0), transform.position + new Vector3(0f, -_SharkAggroArea.size.y / 2f, 0));
                    Gizmos.DrawLine(transform.position + new Vector3(0f, -_SharkAggroArea.size.y / 2f, 0), transform.position + new Vector3(-_SharkAggroArea.size.x, -_SharkAggroArea.size.y / 2f, 0));
                    Gizmos.DrawLine(transform.position + new Vector3(-_SharkAggroArea.size.x, -_SharkAggroArea.size.y / 2f, 0), transform.position + new Vector3(-_SharkAggroArea.size.x, _SharkAggroArea.size.y / 2f, 0));
                    Gizmos.DrawLine(transform.position + new Vector3(-_SharkAggroArea.size.x, _SharkAggroArea.size.y / 2f, 0), transform.position + new Vector3(0f, _SharkAggroArea.size.y / 2f, 0));
                }
            }
            else if (gameObject.GetComponent<EnemyShip>() != null)
            {
                Gizmos.DrawLine(transform.position, new Vector3(transform.position.x - _AggroRange, transform.position.y, transform.position.z));
            }
        }
    }
}
