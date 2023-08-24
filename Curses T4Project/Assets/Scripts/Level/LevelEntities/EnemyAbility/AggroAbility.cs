//AggroAbility.cs
//by MAURIZIO FISCHETTI and ANTHONY FEDELI

using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

/// <summary>
/// AggroAbility.cs give to the gameobject the ability of be aggroed by the player when enter in area to be charged.
/// </summary>

public class AggroAbility : MonoBehaviour
{
    [Header("Aggro Conditions")]
    [SerializeField] private bool _AggroAtStart = false;
    [SerializeField] private bool _Aggroed = false;
    [SerializeField] private float _AggroSpeedMultiplier = 2.0f;

    public bool Aggroed { get { return _Aggroed; } }

    [Header("Aggro Dimension with Shark Pack")]
    [SerializeField] private float _AggroAreaRange = 8.0f;
    [SerializeField] private float _AggroAreaRangeToAttack = 1.6f;
    [SerializeField] private float _AggroAreaHeight = 4.5f;

    [Header("No Shark Pack Conditions")]
    [SerializeField] private float _CollisionAreaHeight = 1f;
    [SerializeField] private float _CollisionAreaRange = 1f;

    private BoxCollider _SharkAggroArea;


    void Start()
    {
        if (_AggroAtStart)
        {
            _Aggroed = true;
        }

        if (gameObject.GetComponent<SeaMonster>() != null)
        {
            if (gameObject.GetComponent<SeaMonster>().IsSharkPack)
            {
                _SharkAggroArea = GetComponent<BoxCollider>();
                _SharkAggroArea.center = new Vector3(-_AggroAreaRange / 2, 0f, 0f);
                _SharkAggroArea.size = new Vector3(_AggroAreaRange, _AggroAreaHeight, 6f);
            }
            else
            {
                _SharkAggroArea = GetComponent<BoxCollider>();
                _SharkAggroArea.center = new Vector3(0, 0.1f, 0f);
                _SharkAggroArea.size = new Vector3(_CollisionAreaRange, _CollisionAreaHeight, 6f);
            }
        }
    }

    void Update()
    {
        //if the ray hit the player then aggro
        if (!gameObject.GetComponent<SeaMonster>().IsSharkPack && Physics.Raycast(transform.position, Vector3.left, out RaycastHit hit, _AggroAreaRange, layerMask: LayerMask.GetMask("Player")))
        {
            _Aggroed = true;
        }

        //if in aggro multiply the speed
        if (_Aggroed)
        {
            gameObject.GetComponent<SeaMonster>().MoveSpeed = GameManager.Instance.LevelManager.CurrentLevel.LevelSpeed * _AggroSpeedMultiplier;
        }
    }

    private void OnValidate()
    {
        if (gameObject.GetComponent<SeaMonster>() != null)
        {
            if (gameObject.GetComponent<SeaMonster>().IsSharkPack)
            {
                _SharkAggroArea = GetComponent<BoxCollider>();
                _SharkAggroArea.center = new Vector3(-_AggroAreaRange / 2, 0f, 0f);
                _SharkAggroArea.size = new Vector3(_AggroAreaRange, _AggroAreaHeight, 6f);
            }
            else
            {
                _SharkAggroArea = GetComponent<BoxCollider>();
                _SharkAggroArea.center = new Vector3(0, 0.1f, 0f);
                _SharkAggroArea.size = new Vector3(_CollisionAreaRange, _CollisionAreaHeight, 6f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.GetComponent<SeaMonster>() != null && gameObject.GetComponent<SeaMonster>().IsSharkPack)
        {
            _SharkAggroArea = GetComponent<BoxCollider>();
            _SharkAggroArea.center = new Vector3(-_AggroAreaRangeToAttack / 2, 0f, 0f);
            _SharkAggroArea.size = new Vector3(_AggroAreaRangeToAttack, _AggroAreaHeight, 6f);
        }
        _Aggroed = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (gameObject.GetComponent<SeaMonster>() != null)
        {
            if (!gameObject.GetComponent<SeaMonster>().IsSharkPack)
            {
                Gizmos.DrawLine(transform.position, new Vector3(transform.position.x - _AggroAreaRange, transform.position.y, transform.position.z));
            }
            else
            {
                if (_Aggroed)
                {
                    _SharkAggroArea = GetComponent<BoxCollider>();
                    _SharkAggroArea.center = new Vector3(-_AggroAreaRangeToAttack / 2, 0f, 0f);
                    _SharkAggroArea.size = new Vector3(_AggroAreaRangeToAttack, _AggroAreaHeight, 6f);
                }
                else
                {
                    _SharkAggroArea = GetComponent<BoxCollider>();
                    _SharkAggroArea.center = new Vector3(-_AggroAreaRange / 2, 0f, 0f);
                    _SharkAggroArea.size = new Vector3(_AggroAreaRange, _AggroAreaHeight, 6f);
                }
                Gizmos.DrawLine(transform.position + new Vector3(0f, _SharkAggroArea.size.y / 2f, 0), transform.position + new Vector3(0f, -_SharkAggroArea.size.y / 2f, 0));
                Gizmos.DrawLine(transform.position + new Vector3(0f, -_SharkAggroArea.size.y / 2f, 0), transform.position + new Vector3(-_SharkAggroArea.size.x, -_SharkAggroArea.size.y / 2f, 0));
                Gizmos.DrawLine(transform.position + new Vector3(-_SharkAggroArea.size.x, -_SharkAggroArea.size.y / 2f, 0), transform.position + new Vector3(-_SharkAggroArea.size.x, _SharkAggroArea.size.y / 2f, 0));
                Gizmos.DrawLine(transform.position + new Vector3(-_SharkAggroArea.size.x, _SharkAggroArea.size.y / 2f, 0), transform.position + new Vector3(0f, _SharkAggroArea.size.y / 2f, 0));
            }
        }
    }

}
