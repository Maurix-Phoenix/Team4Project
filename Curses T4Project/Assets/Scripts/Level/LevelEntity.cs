//LevelEntity.cs
//by MAURIZIO FISCHETTI

using System;
using System.Collections.Generic;
using UnityEngine;


public class LevelEntity : MonoBehaviour
{
    [Header("Level Entity Info")]
    public string EntityName = "";

    public Rigidbody RB;

    [Header("Looting")]
    public bool CanDropLoot = false;
    [Serializable] public class Loot
    {
        public GameObject PickupPrefab;
        public Vector2Int DropQuantityRange = Vector2Int.one;
    }
    public List<Loot> LootList;
    public float DropRadius = 0.5f;

    [Header("Movement")]
    public float MoveSpeed;
    public bool MovePickUpToPlayer = false;
    private Vector3 Direction;

    public bool IsStopped = false;

    private void OnEnable()
    {
        EventManager EM = GameManager.Instance.EventManager;
        EM.LevelStart += OnLevelStart;
        EM.LevelStop += OnLevelStop;
    }

    private void OnDisable()
    {
        EventManager EM = GameManager.Instance.EventManager;
        EM.LevelStart -= OnLevelStart;
        EM.LevelStop -= OnLevelStop;
    }

    #region Events Methods

    private void OnLevelStart()
    {
        IsStopped = false;
    }

    private void OnLevelStop()
    {
        IsStopped = true;
    }

    #endregion

    protected virtual void Start()
    {

        MoveSpeed = GameManager.Instance.LevelManager.CurrentLevel.LevelSpeed;
        GameManager.Instance.LevelManager.CurrentLevel.LevelObjects.Add(this);

        RB = gameObject.GetComponent<Rigidbody>();
        Direction = Vector3.zero;
    }

    protected virtual void Update()
    {
        if (RB == null)
        {
            Move();
        }

        if (transform.position.x <= T4P.T4Project.XVisualLimit.x)
        {
            gameObject.SetActive(false);
        }

        if(transform.position.y <= T4P.T4Project.YVisualLimit.x || transform.position.y >= T4P.T4Project.YVisualLimit.y)
        {
            gameObject.SetActive(false);
        }


    }

    protected virtual void FixedUpdate()
    {
        if (RB != null)
        {
            Move();
        }
    }

    protected void DropLoot(float multDist = 1f)
    {
        if(CanDropLoot)
        {
            foreach(Loot loot in LootList)
            {
                int dropN = UnityEngine.Random.Range(loot.DropQuantityRange.x, loot.DropQuantityRange.y);
                for(int i = 0; i < dropN; i++)
                {
                    Vector3 spawnPos = T4P.Utilities.RandomPointInCircle(transform.position, DropRadius);
                    if (transform.position.y >= 0 &&  spawnPos.y >= 0)
                    {
                        spawnPos.y = 0;
                    }
                    
                    Pickup pk = Instantiate(loot.PickupPrefab,spawnPos,Quaternion.identity).GetComponent<Pickup>();
                    pk.AttractionDistance *= multDist;
                }
            }
        }
    }

    public void Move()
    {
        if (!IsStopped)
        {
            Direction.x = -1;

            if (MovePickUpToPlayer)
            {
                Direction.x = GameManager.Instance.LevelManager.Player.transform.position.x - transform.position.x;
                Direction.y = GameManager.Instance.LevelManager.Player.transform.position.y - transform.position.y;
                RB.MovePosition(RB.position + Direction.normalized * MoveSpeed * Time.fixedDeltaTime);
                return;
            }

            //move with physics
            if (RB != null && !RB.isKinematic)
            {
                RB.MovePosition(RB.position + Direction.normalized * MoveSpeed * Time.fixedDeltaTime);
            }

            //move without physics
            if (RB == null)
            {
                transform.position += Direction.normalized * MoveSpeed * Time.deltaTime;
            }
        }
    }
}
