using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimationRandomFrame : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        var state = _animator.GetCurrentAnimatorStateInfo(0);
        _animator.Play(state.fullPathHash, 0, Random.Range(0.0f, 1.0f));
    }
}
