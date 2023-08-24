//ChangeCamera.cs
//by ANTHONY FEDELI

using UnityEngine;

/// <summary>
/// ChangeCamera.cs manages the switching between the cinemachine.
/// </summary>
public class ChangeCamera : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.LevelManager.Player.IsInStartAnimation)
        {
            _animator.SetBool("PlayPosition", true);
        }

        if (GameManager.Instance.LevelManager.CurrentLevel.ActualLayer == 0)
        {
            _animator.SetBool("IsUnderWater", false);
        }
        else
        {
            _animator.SetBool("IsUnderWater", true);
        }
    }
}
