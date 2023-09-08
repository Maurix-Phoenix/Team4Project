using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortVendor : LevelEntity
{
    [Header("Port Variables")]
    [SerializeField] private bool _CanSell = true;
    [SerializeField] private float _SellAfterTime = 0.5f;

    [Header("SFX")]
    [SerializeField] private AudioClip _MerchantSFX;

    private void OnTriggerEnter(Collider other)
    {
        //do damage to the player and deactivate the collider
        if (other.gameObject.GetComponent<Player>() != null)
        {
            GameManager.Instance.AudioManager.PlaySFX(_MerchantSFX);
            Invoke("DropLoot", _SellAfterTime);
        }
    }
}
