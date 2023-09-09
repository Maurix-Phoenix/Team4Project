using UnityEngine;

public class PlayerAppearence : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private bool _IsInMenuScene = false;
    [SerializeField] private bool _GoFromNormalToCursed = true;
    [SerializeField] private float _MaxTimeBetWeenTransition = 5;
    [SerializeField] private float _MinTimeBetWeenTransition = 10;
    [SerializeField] private float _TransitionTimer;
    [SerializeField] private float _TransitionCD = 0f;
    private float _NormalTransparency;
    private float _CursedTransparency;

    [Header("NPC")]
    [SerializeField] private bool _IsNPC = false;

    [Header("Skin Reference")]
    [SerializeField] private GameObject _Position;
    [SerializeField] private GameObject _NormalSkin;
    [SerializeField] private GameObject _CursedSkin;
    [SerializeField] private Material _NormalMat;
    [SerializeField] private Material _CursedMat;

    [Header("VFX")]
    [SerializeField] private ParticleSystem _AboveWaterVFX;
    [SerializeField] private Color _NormalColor;
    [SerializeField] private Color _CursedColor;
    [SerializeField] private ParticleSystem _UnderWaterVFX;

    [Header("Treshold")]
    [SerializeField] private float _YUpperThreshold;
    [SerializeField] private float _YLowerThreshold;

    private float _StartingNormalTransparency;
    private float _StartingCursedTransparency;

    private void Awake()
    {
        if (_IsInMenuScene)
        {
            _IsNPC = false;
        }

        if (_IsNPC)
        {
            _NormalMat = _NormalSkin.GetComponent<SkinnedMeshRenderer>().material;
            _CursedMat = _CursedSkin.GetComponent<SkinnedMeshRenderer>().material;
        }
        else
        {
            _NormalMat = _NormalSkin.GetComponent<MeshRenderer>().material;
            _CursedMat = _CursedSkin.GetComponent<MeshRenderer>().material;
        }

        _StartingNormalTransparency = _NormalMat.GetFloat("_Transparency");
        _StartingCursedTransparency = _CursedMat.GetFloat("_Transparency");
    }
    private void Start()
    {
        if (_IsInMenuScene)
        {
            _IsNPC = false;
            _NormalTransparency = _StartingNormalTransparency;
            _CursedTransparency = 0;
            _NormalSkin.SetActive(true);
            _CursedSkin.SetActive(false);
            _GoFromNormalToCursed = true;
            _TransitionTimer = Random.Range(_MinTimeBetWeenTransition, _MaxTimeBetWeenTransition);
        }
        else
        {
            if (_IsNPC)
            {
                _NormalTransparency = _StartingNormalTransparency;
                _CursedTransparency = 0;
                _NormalSkin.SetActive(true);
                _CursedSkin.SetActive(false);
            }
        }
    }

    private void Update()
    {
        ChangeSkin();

        if (!_IsInMenuScene)
        {
            ActivateCorrectSkin();

            if (!_IsNPC)
            {
                ActivateCorrectPS();
            }
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

    private void ActivateCorrectPS()
    {
        if (_Position.transform.position.y > _YLowerThreshold)
        {
            _AboveWaterVFX.startColor = _NormalColor;
            
        }
        else
        {
            _AboveWaterVFX.startColor = _CursedColor;
        }


        if (_Position.transform.position.y < _YUpperThreshold)
        {
            if (!_UnderWaterVFX.isPlaying)
            {
                _UnderWaterVFX.Play();
            }
        }
        else
        {
            if (_UnderWaterVFX.isPlaying)
            {
                _UnderWaterVFX.Stop();
            }
        }
    }

    private void ChangeSkin()
    {
        if (_IsInMenuScene)
        {
            if(_TransitionCD <= _TransitionTimer)
            {
                _TransitionCD += Time.deltaTime;
            }
            else
            {
                if (_GoFromNormalToCursed)
                {
                    _NormalSkin.SetActive(true);
                    _CursedSkin.SetActive(true);

                    _NormalTransparency -= Time.deltaTime;
                    _NormalMat.SetFloat("_Transparency", _NormalTransparency);

                    _CursedTransparency += (Time.deltaTime * _StartingCursedTransparency);
                    _CursedMat.SetFloat("_Transparency", _CursedTransparency);

                    if (_NormalMat.GetFloat("_Transparency") < 0)
                    {
                        _TransitionCD = 0;
                        _TransitionTimer = Random.Range(_MinTimeBetWeenTransition, _MaxTimeBetWeenTransition);

                        _NormalMat.SetFloat("_Transparency", 0);
                        _NormalSkin.SetActive(false);

                        _CursedMat.SetFloat("_Transparency", _StartingCursedTransparency);

                        _GoFromNormalToCursed = false;
                    }
                }
                else
                {
                    _NormalSkin.SetActive(true);
                    _CursedSkin.SetActive(true);

                    _NormalTransparency += Time.deltaTime;
                    _NormalMat.SetFloat("_Transparency", _NormalTransparency);

                    _CursedTransparency -= (Time.deltaTime * _StartingCursedTransparency);
                    _CursedMat.SetFloat("_Transparency", _CursedTransparency);

                    if (_NormalMat.GetFloat("_Transparency") > 1)
                    {
                        _TransitionCD = 0;
                        _TransitionTimer = Random.Range(_MinTimeBetWeenTransition, _MaxTimeBetWeenTransition);

                        _NormalMat.SetFloat("_Transparency", _StartingNormalTransparency);

                        _CursedMat.SetFloat("_Transparency", 0);
                        _CursedSkin.SetActive(false);

                        _GoFromNormalToCursed = true;
                    }
                }
            }
        }
        else
        {
            if (_Position.transform.position.y <= _YUpperThreshold &&
                _Position.transform.position.y >= _YLowerThreshold)
            {
                float newNormalTransparency = (((_Position.transform.position.y - _YLowerThreshold) / (_YLowerThreshold - _YUpperThreshold)) * (0f - _StartingNormalTransparency) + 0);
                float newCursedTransparency = (((_Position.transform.position.y - _YLowerThreshold) / (_YLowerThreshold - _YUpperThreshold)) * (_StartingCursedTransparency - 0f) + _StartingCursedTransparency);
                _NormalMat.SetFloat("_Transparency", newNormalTransparency);
                _CursedMat.SetFloat("_Transparency", newCursedTransparency);
            }
            else
            {
                if (_Position.transform.position.y > _YUpperThreshold)
                {
                    _NormalMat.SetFloat("_Transparency", _StartingNormalTransparency);
                    _CursedMat.SetFloat("_Transparency", 0f);
                }

                if (_Position.transform.position.y < _YLowerThreshold)
                {
                    _NormalMat.SetFloat("_Transparency", 0);
                    _CursedMat.SetFloat("_Transparency", _StartingCursedTransparency);
                }
            }
        }
    }
}
