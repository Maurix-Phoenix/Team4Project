using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterBackground : MonoBehaviour
{
    [SerializeField]
    private float _ScrollingSpeed = 0.5f;

    private Vector3 _RightEdge;
    private Vector3 _LeftEdge;
    private Vector3 _StartingPos;
    private Vector3 _EdgesDistance;

    SpriteRenderer _SR; 


    private void Awake()
    {
        _SR = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _StartingPos = transform.position;
        _LeftEdge = new(transform.position.x - (_SR.bounds.extents.x /3f), transform.position.y, transform.position.z);
        _RightEdge = new(transform.position.x + (_SR.bounds.extents.x /3f), transform.position.y, transform.position.z);
        _EdgesDistance = _RightEdge - _LeftEdge;
    }


    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance != null)
        {
            if (GameManager.Instance.GameState == GameManager.States.Playing && 
                !GameManager.Instance.LevelManager.CurrentLevel.IsInBossBattle && 
                !GameManager.Instance.LevelManager.CurrentLevel.IsFinalArrivalBeach)
            {
                transform.localPosition += (_ScrollingSpeed + (GameManager.Instance.LevelManager.CurrentLevel.LevelSpeed/3)) * Vector3.left * Time.deltaTime;
            }
        }
        else
        {
            transform.position += _ScrollingSpeed * Vector3.left * Time.deltaTime;
        }

        if (transform.position.x < _LeftEdge.x)
        {
            ResetSpritePosition();
        }        
    }

    private void ResetSpritePosition()
    {
        transform.position += _EdgesDistance;
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Gizmos.DrawSphere(_StartingPos, 0.2f);
        Gizmos.DrawSphere(_LeftEdge, 0.2f);
        Gizmos.DrawSphere(_RightEdge, 0.2f);
#endif
    }
}
