//Fish.cs
//by MAURIZIO FISCHETTI

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
        if(_ConsiderLevelSpeed && GameManager.Instance.LevelManager.CurrentLevel != null)
        {
            _Speed += GameManager.Instance.LevelManager.CurrentLevel.LevelSpeed / 4;
        }
        _CurrentSpeed = _Speed + Random.Range(-_SpeedVariation, _SpeedVariation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * Time.deltaTime * _CurrentSpeed;

        //_ChangeDirectionT -= Time.deltaTime;
        //if(_ChangeDirectionT > 0)
        //{
        //    Vector3 direction = _CurrentTargetPos - transform.position;
        //    transform.Translate(direction.normalized * Time.deltaTime * _CurrentSpeed);
        //}
        //else
        //{
        //    _CurrentTargetPos =
        //    new Vector3(Random.Range(_XRange.x, _XRange.y),
        //    Random.Range(_YRange.x, _YRange.y),
        //    Random.Range(_ZRange.x, _ZRange.y));

        //    transform.LookAt(_CurrentTargetPos);

        //    _CurrentSpeed = _Speed + Random.Range(-_SpeedVariation, _SpeedVariation);
        //    _ChangeDirectionT = _ChangeDirectionTime;
        //}



        //if(transform.position.y < _YRange.x || transform.position.y > _YRange.y || transform.position.z < _ZRange.x || transform.position.z > _ZRange.y )
        //{
        //    _ChangeDirectionT = 0;
        //}
    }
}
