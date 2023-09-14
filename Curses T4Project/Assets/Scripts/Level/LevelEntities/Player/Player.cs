//Player.cs
//by ANTHONY FEDELI & MAURIZIO FISCHETTI

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
    [Header("Player Variables")]
    public int Health = 3;
    public int NOfCannonball = 0;
    public int NOfDoubloons = 0;
    public int NOfFlags = 0;

    [Header("Shoot Condition")]
    public bool IsShooting = false;
    public bool CanShoot = false;

    [Header("Movement Condition")]
    public bool IsChangingLayer = false;
    public bool CanMove = false;
    public bool IsInStartAnimation = true;
    public bool LastDistancePicked = false;

    [Header("Pirates")]
    [SerializeField] GameObject Pirates;

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
        Pirates.SetActive(false);
    }

    void Start()
    {
        InitializePlayer();
        UpdatePlayerUI();
    }

    private void InitializePlayer()
    {
        Health = GameManager.Instance.LevelManager.CurrentLevel.StartingHealth;
        NOfCannonball = GameManager.Instance.LevelManager.CurrentLevel.StartingCannonBalls;
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
        LevelUI levelUI = GameManager.Instance.UIManager.LevelUI;
        Vector3 worldCoord = Vector3.zero;

        UILabel.LabelIconStyles iconStyle = new UILabel.LabelIconStyles();
        switch (resourceType)
        {
            case T4Project.PickupsType.Cannonball:
                {
                    iconStyle = UILabel.LabelIconStyles.Cannonballs;
                    worldCoord = levelUI.CannonballsText.rectTransform.position;
                    NOfCannonball += value;
                    if (NOfCannonball < 0)
                    {
                        NOfCannonball = 0;
                    }
                    break;
                }
            case T4Project.PickupsType.Doubloon:
                {
                    iconStyle = UILabel.LabelIconStyles.Doubloons;
                    worldCoord = levelUI.DoubloonsText.rectTransform.position;
                    NOfDoubloons += value;
                    if (NOfDoubloons < 0)
                    {
                        NOfDoubloons = 0;
                    }
                    break;
                }
            case T4Project.PickupsType.Flag:
                {
                    iconStyle = UILabel.LabelIconStyles.Flags;
                    worldCoord = levelUI.FlagCover.rectTransform.position;
                    NOfFlags += value;
                    break;
                }
        }
        worldCoord.z = Camera.main.farClipPlane;
        worldCoord = Camera.main.ScreenToWorldPoint(worldCoord);
        Vector3 animDir = value > 0 ? worldCoord : -worldCoord;

        GameManager.Instance.UIManager.CreateUILabel().ShowLabel(iconStyle, $"+{value}",new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), null, 3f, 100f, animDir);
        UpdatePlayerUI();
    }

    public void TakeDamage(int dmg, GameObject damager)
    {
        Health -= dmg;

        UpdatePlayerUI();
        if (gameObject.GetComponent<DamageFlash>() != null && gameObject.GetComponent<DamageFlash>().enabled)
        {
            GetComponent<DamageFlash>().DamageIndicatorStart();
        }

        T4Debug.Log($"Player damaged by {damager.name}");
        Camera.main.GetComponent<CameraShake>().StartShaking();

        if(Health <=0)
        {
            Health = 0;
            //player death animation?
            //gameObject.SetActive(false);

            //GameOver here
            //MAU - call endlevel (gameover)
            GameManager.Instance.LevelManager.CurrentLevel.EndLevel(Level.EndLevelType.GameOver);
        }
    }

    public void SpawnPirates()
    {
        Pirates.SetActive(true);
    }

    public void UpdatePlayerUI()
    {
        GameManager.Instance.UIManager.LevelUI.UpdateLevelUI();
    }
}
