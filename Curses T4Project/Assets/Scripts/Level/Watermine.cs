using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Watermine : MonoBehaviour
{

    //Transform
    [SerializeField] private Vector3 _Position => transform.position;

    //Custom object vars
    [SerializeField] private float _FloatingSpeed = 1f;
    [SerializeField] private float _Amplitude = 0.5f;
    [SerializeField] private float _ExplosionRange = 2.0f;
    [SerializeField] private float _ExplosionTriggerRange = 1f;

    private Vector3 _StartingPos;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = _Position;

        _StartingPos = _Position;
    }

    // Update is called once per frame
    void Update()
    {
        //floating movement
        float newY = _StartingPos.y + _Amplitude * Mathf.Sin(_FloatingSpeed * Time.time);
        transform.position = new Vector3(_StartingPos.x, newY, _StartingPos.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_Position, _ExplosionRange);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_Position, _ExplosionTriggerRange);

    }
}
