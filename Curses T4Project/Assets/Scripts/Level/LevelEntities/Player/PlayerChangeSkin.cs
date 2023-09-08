using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeSkin : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject _NormalSkin;
    [SerializeField] private GameObject _CursedSkin;
    [SerializeField] private GameObject _Position;
    [SerializeField] private Material _CursedMat;
    [SerializeField] private Material _NormalMat;

    [Header("Treshold")]
    [SerializeField] private float _YUpperThreshold;
    [SerializeField] private float _YLowerThreshold;

    [Header("Transparency")]
    [SerializeField] private float _StartingNormalTransparency;
    [SerializeField] private float _StartingCursedTransparency;

    private void Awake()
    {
        _NormalMat = _NormalSkin.GetComponent<MeshRenderer>().material;
        _CursedMat = _CursedSkin.GetComponent<MeshRenderer>().material;

        _StartingNormalTransparency = _NormalMat.GetFloat("Transparency");
        _StartingCursedTransparency = _CursedMat.GetFloat("Transparency");
    }
    private void Start()
    {
        PickStartingSkin();
    }

    private void Update()
    {
        ActivateCorrectSkin();
        ChangeSkin();
    }

    private void PickStartingSkin()
    {
        if (GameManager.Instance.LevelManager.CurrentLevel.ActualLayer >= 0)
        {
            _NormalSkin.SetActive(true);
            _CursedSkin.SetActive(false);
        }
        else
        {
            _NormalSkin.SetActive(false);
            _CursedSkin.SetActive(true);
        }
    }

    private void ActivateCorrectSkin()
    {
        if (_Position.transform.position.y <= _YLowerThreshold)
        {
            _NormalSkin.SetActive(false);
        }
        else
        {
            _NormalSkin.SetActive(true);
        }

        if (_Position.transform.position.y >= _YUpperThreshold)
        {
            _CursedSkin.SetActive(false);
        }
        else
        {
            _CursedSkin.SetActive(true);
        }

    }

    private void ChangeSkin()
    {
        if (_Position.transform.position.y <= _YUpperThreshold)
        {
            float start = Mathf.Clamp(_Position.transform.position.y, _YUpperThreshold, _YLowerThreshold);
            Debug.Log(start);

            if (_NormalSkin.activeSelf)
            {
            }
        }
    }

}
