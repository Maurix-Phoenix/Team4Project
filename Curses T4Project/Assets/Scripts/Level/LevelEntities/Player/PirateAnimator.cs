using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateAnimator : MonoBehaviour
{
    [SerializeField] private List<Animator> _PirateAnimator;

    [SerializeField] private float _MaxTimeBetweenAnimation = 3f;
    [SerializeField] private float _MinTimeBetweenAnimation = 5f;

    [SerializeField] private float _AnimationDuration;
    [SerializeField] private float _AnimationTime;

    [SerializeField] private bool _IsOnShip = false;


    // Start is called before the first frame update
    void Start()
    {
        _AnimationTime = 0f;
        _AnimationDuration = Random.Range(_MinTimeBetweenAnimation, _MaxTimeBetweenAnimation);
    }

    // Update is called once per frame
    void Update()
    {
        if (_IsOnShip)
        {
            if (_AnimationTime <= _AnimationDuration)
            {
                _AnimationTime += Time.deltaTime;
            }
            else
            {
                DoUrray();
                _AnimationDuration = Random.Range(_MinTimeBetweenAnimation, _MaxTimeBetweenAnimation);
                _AnimationTime = 0;
            }
        }
    }

    public void DoUrray()
    {
        for (int i = 0;  i < _PirateAnimator.Count; i++)
        {
            _IsOnShip = true;
            _PirateAnimator[i].SetTrigger("DoUrray");
        }
    }
}
