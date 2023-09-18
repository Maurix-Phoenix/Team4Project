//Fish.cs
//by MAURIZIO FISCHETTI

using Unity.VisualScripting;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public FishTemplate FishT;
    [SerializeField] private Vector3 _CurrentTargetPos = Vector3.zero;
    [SerializeField] private float _ChangeDirectionTime = 1;
   // [SerializeField] private float _ChangeDirectionT = 0;
    [SerializeField] private float _Speed = 1f;
    [SerializeField] private float _SpeedVariation = 0.5f;
    [SerializeField] private bool _ConsiderLevelSpeed = true;
    [SerializeField] private float _CurrentSpeed;
    public float XDir = -1;

    private Vector2 _XRange = new Vector2(-26, 36);
    private Vector2 _YRange = new Vector2(-10, -1);
    private Vector2 _ZRange = new Vector2(6, 23);

    // Start is called before the first frame update
    void Start()
    {
        if(FishT != null )
        {
            _ChangeDirectionTime = FishT.ChangeDirectionIntervail;
            _Speed = FishT.Speed;
            _SpeedVariation = FishT.SpeedVariation; 
            _ConsiderLevelSpeed = FishT.ConsiderLevelSpeed;
        }

        if (XDir == 1)//to right
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else //to left
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (_ConsiderLevelSpeed && GameManager.Instance.LevelManager.CurrentLevel != null)
        {

            if (XDir == 1)
            {
                _Speed -= transform.position.z / GameManager.Instance.LevelManager.CurrentLevel.LevelSpeed;
            }
            else
            {
                _Speed += transform.position.z / GameManager.Instance.LevelManager.CurrentLevel.LevelSpeed;
            }
        }
        _CurrentSpeed = _Speed + Random.Range(-_SpeedVariation, _SpeedVariation);
    }

    private void OnEnable()
    {
        if (FishT != null)
        {
            _ChangeDirectionTime = FishT.ChangeDirectionIntervail;
            _Speed = FishT.Speed;
            _SpeedVariation = FishT.SpeedVariation;
            _ConsiderLevelSpeed = FishT.ConsiderLevelSpeed;
        }


        if(XDir == 1)//to right
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else //to left
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (_ConsiderLevelSpeed && GameManager.Instance.LevelManager.CurrentLevel != null)
        {
            
            if (XDir == 1)
            {
                _Speed -= GameManager.Instance.LevelManager.CurrentLevel.LevelSpeed/2;
            }
            else
            {
                _Speed += GameManager.Instance.LevelManager.CurrentLevel.LevelSpeed;
            }
        }
        _CurrentSpeed = _Speed + Random.Range(-_SpeedVariation, _SpeedVariation);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.LevelManager.CurrentLevel.IsStopped)
        {
            _CurrentSpeed = _CurrentSpeed = _Speed + Random.Range(-_SpeedVariation, _SpeedVariation);
            transform.position += new Vector3(XDir, 0, 0) * Time.deltaTime * Mathf.Abs(_CurrentSpeed);
        }
        else
        {
            transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * Mathf.Abs(_CurrentSpeed);
        }


    }
}
