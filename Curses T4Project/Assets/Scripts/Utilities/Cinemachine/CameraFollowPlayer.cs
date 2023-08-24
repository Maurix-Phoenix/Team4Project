//CameraFollowPlayer.cs
//by ANTHONY FEDELI

using UnityEngine;
using Cinemachine;

/// <summary>
/// CameraFollowPlayer.cs manages the behaviour of the CinemachineCamera.
/// </summary>

public class CameraFollowPlayer: MonoBehaviour
{
    [Header("Camera settings")]
    [SerializeField] private float _CameraXDampAtTheEnd = 20;
    private CinemachineVirtualCamera _CVC;
    private float _PlayPositionCamYOffset;

    private void Awake()
    {
        _CVC = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        if (GameManager.Instance.LevelManager.CurrentLevel.StartingLane == T4P.T4Project.LaneType.AboveWater)
        {
            _PlayPositionCamYOffset = 1.690599f;
        }
        else
        {
            _PlayPositionCamYOffset = 1.154701f;
        }
    }

    private void Update()
    {
        if (gameObject.name == "PlayPosition")
        {
            gameObject.transform.position = new Vector3 (gameObject.transform.position.x,
                                                         GameManager.Instance.LevelManager.CurrentLevel.ActualLayer * GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer + _PlayPositionCamYOffset,
                                                         gameObject.transform.position.z);
            return;
        }
        else
        {
            if(GameManager.Instance.LevelManager.CurrentLevel.IsInBossBattle)
            {
                _CVC.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = _CameraXDampAtTheEnd;
            }
        }
        if (FindObjectOfType<Player>() != null)
        {
            _CVC.Follow = FindObjectOfType<Player>().transform;
        }
    }
}
