using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //posizione
    [SerializeField] private Vector3 _Position => transform.position;

   // private int _Health = 3;

    private Vector3 _StartPosition;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = _Position;
        _StartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
