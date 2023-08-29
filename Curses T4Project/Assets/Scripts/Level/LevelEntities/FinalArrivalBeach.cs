//FinalArrivalBeachl.cs
//by ANTHONY FEDELI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FinalArrivalBeach.cs manages the behaviour of the Final Arrival Beach at the end of the level.
/// </summary>

public class FinalArrivalBeach : LevelEntity
{
    [Header("References")]
    [SerializeField] private BoxCollider _TriggerToMovePlayer;

    [Header("Final Arrival Beach Variables")]
    [SerializeField] private Vector3 _TriggerPosition;
    [SerializeField] private Vector3 _TriggerDimension;
    [SerializeField] private bool BringPlayerBackToSurface;

    private Rigidbody _Rb;


    private void Awake()
    {
        InitializeRB();
        InitializeTrigger();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!GameManager.Instance.LevelManager.CurrentLevel.IsInBossBattle)
            {
                _TriggerToMovePlayer.gameObject.SetActive(false);
                if (BringPlayerBackToSurface)
                {
                    other.GetComponent<PlayerMovement>().BackToSurface();
                }
                GameManager.Instance.LevelManager.CurrentLevel.IsInBossBattle = true;
                GameManager.Instance.LevelManager.CurrentLevel.IsLevelEnded = true;
                IsStopped = true;
            }
            else
            {
                GameManager.Instance.LevelManager.CurrentLevel.PlayerHasReachBeach = true;
                //MAU call VICTORY
                GameManager.Instance.LevelManager.CurrentLevel.EndLevel(Level.EndLevelType.Victory);
            }
        }
    }


    private void OnValidate()
    {
        InitializeTrigger();
    }

    private void InitializeRB()
    {
        //Initialize the Rigidbody component
        _Rb = GetComponent<Rigidbody>();
        _Rb.useGravity = false;
        _Rb.interpolation = RigidbodyInterpolation.Interpolate;
        _Rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void InitializeTrigger()
    {
        _TriggerToMovePlayer = gameObject.transform.Find("Trigger").GetComponent<BoxCollider>();
        _TriggerToMovePlayer.size = _TriggerDimension;
        _TriggerToMovePlayer.center = new Vector3(-_TriggerDimension.x / 2, _TriggerPosition.y, _TriggerPosition.z);
    }

}
