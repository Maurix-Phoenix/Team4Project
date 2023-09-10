using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : LevelEntity
{
    [Header("Boat Variables")]
    [SerializeField] private float _DropAfterTime = 0f;

    [Header("Pirate Variables")]
    [SerializeField] private GameObject _Pirates;
    [SerializeField] private bool _IsPirateMovedOnShip = false;

    [Header("SFX")]
    [SerializeField] private AudioClip _UrraySFX;

    private GameObject _player;

    protected override void Start()
    {
        base.Start();
        _Pirates.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        //do damage to the player and deactivate the collider
        if (other.gameObject.GetComponent<Player>() != null)
        {
            _player = other.gameObject;
            if (_Pirates != null)
            {
                gameObject.GetComponent<PirateAnimator>().DoUrray();
                GameManager.Instance.AudioManager.PlaySFX(_UrraySFX);
                Invoke("DropLoot", _DropAfterTime);
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        if (gameObject.transform.position.x < 0 && _player != null)
        {
            _Pirates.SetActive(false);
            if (!_IsPirateMovedOnShip)
            {
                _IsPirateMovedOnShip = true;
                _player.GetComponent<Player>().SpawnPirates();
            }
        }
    }
}
