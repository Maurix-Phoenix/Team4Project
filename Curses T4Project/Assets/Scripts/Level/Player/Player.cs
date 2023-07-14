//Player.cs
//by ANTHONY FEDELI

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static T4P;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerShoot))]
[RequireComponent(typeof(PlayerMovement))]

/// <summary>
/// Player.cs manages the behaviour of the player and the variables and conditions.
/// </summary>

public class Player : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField] private InputActionReference _InputPauseReference;

    [Header("Player Variables")]
    public int InitialHealth = 3;
    public int NOfCannonball;
    public int NOfDoubloons = 0;

    [Header("Shoot Condition")]
    public bool IsShooting = false;
    public bool CanShoot = false;

    [Header("Movement Condition")]
    public bool IsChangingLayer = false;
    public bool CanMove = false;
    public bool IsInStartAnimation = true;
    public bool LastDistancePicked = false;

    private Rigidbody _Rb;

    private void Awake()
    {
        //Initialize the Rigidbody component
        _Rb = GetComponent<Rigidbody>();
        _Rb.useGravity = false;
        _Rb.isKinematic = true;
        _Rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        _Rb.interpolation = RigidbodyInterpolation.Interpolate;
        _Rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Start()
    {
        NOfCannonball = GameManager.Instance.Level.StartingCannonBalls;
    }

    private void OnPauseGameInput()
    {
        GameManager.Instance.PauseUnpauseGame();
    }

    /// <summary>
    ///add the reource of the given type
    /// </summary>
    /// <param name="resourceType">the type of the resource</param>
    /// <param name="value">the quantity of the resource to add</param>
    public void AddResource(T4Project.PickupsType resourceType, int value)
    {
        switch(resourceType)
        {
            case T4Project.PickupsType.Cannonball:
            {
                NOfCannonball += value;
                break;
            }
            case T4Project.PickupsType.Doubloon:
            {
                NOfDoubloons += value;
                break;
            }
        }
    }

    public void TakeDamage(int dmg, GameObject damager)
    {
        InitialHealth -= dmg;

        //Damage Effect here

        T4Debug.Log($"Player damaged by {damager.name}");

        if (InitialHealth <= 0)
        {

            //player death animation?
            //gameObject.SetActive(false);

            //GameOver here
            //MAU - call endlevel (gameover)
            GameManager.Instance.Level.EndLevel(Level.EndLevelType.GameOver);
        }
    }
}
