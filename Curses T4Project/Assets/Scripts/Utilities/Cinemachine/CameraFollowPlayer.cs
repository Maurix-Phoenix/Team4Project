using UnityEngine;
using Cinemachine;

public class CameraFollowPlayer: MonoBehaviour
{
    [Header("PlayPosition camera settings")]
    [SerializeField] private float _YOffset;
    private CinemachineVirtualCamera _CVC;

    private void Awake()
    {
        _CVC = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (gameObject.name == "PlayPosition")
        {
            gameObject.transform.position = new Vector3 (gameObject.transform.position.x,
                                                         GameManager.Instance.LevelManager.CurrentLevel.ActualLayer * GameManager.Instance.LevelManager.CurrentLevel.UnitSpaceBetweenLayer + _YOffset,
                                                         gameObject.transform.position.z);
            return;
        }
        if (FindObjectOfType<Player>() != null)
        {
            _CVC.Follow = FindObjectOfType<Player>().transform;
        }
    }
}
