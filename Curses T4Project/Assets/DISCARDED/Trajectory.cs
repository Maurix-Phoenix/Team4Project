using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Shooting _playerShoot;
    [SerializeField] private Transform _cannonPosition;

    [Header("Trajectory Line Smoothness/Lenght")]
    [SerializeField] private int _segmentCount = 50;
    [SerializeField] private float _curveLenght = 3.5f;

    private Vector2[] _segments;
    private LineRenderer _lineRenderer;

    private PlayerCannonball _playerCannonball;

    private float _cannonballSpeed;
    private float _cannonballGravityFromRB;

    private const float _timeCurveAddition = 0.5f;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = _segmentCount;

        //Initialize segments to have the same count
        _segments = new Vector2[_segmentCount];

        //grab the cannonball stats
        _playerCannonball = _playerShoot._CannonballPrefab.GetComponent<PlayerCannonball>();
        _cannonballSpeed = _playerCannonball._physicsCannonballSpeed;
        _cannonballGravityFromRB = _playerCannonball._physicsGravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        //set the starting position of the line renderer
        Vector2 startPos = _cannonPosition.position;
        _segments[0] = startPos;
        _lineRenderer.SetPosition(0, startPos);

        //set the starting velocity based on the cannonball physics
        Vector2 startVelocity = - transform.up * _cannonballSpeed;

        for (int i = 1; i < _segmentCount; i++)
        {
            //compute the time offsett assuming we're using a RB for physics
            float timeOffset = (i * Time.fixedDeltaTime * _curveLenght);

            //compute the gravity offset assuming we're using a RB for physics
            Vector2 gravityOffset = _timeCurveAddition * new Vector3(0f, -9.81f, 0f) * _cannonballGravityFromRB * Mathf.Pow(timeOffset, 2);

            //set the position of the point in the line renderer
            _segments[i] = _segments[0] + startVelocity * timeOffset + gravityOffset;
            _lineRenderer.SetPosition(i, _segments[i]);
        }
    }

}
