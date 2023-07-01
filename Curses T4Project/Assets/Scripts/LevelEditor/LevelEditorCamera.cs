using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorCamera : MonoBehaviour
{
    public Vector3 InitialPosition;
    public float MoveSpeed = 0.02f;
    private Vector3 _Direction = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        InitialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        MoveCamera();
    }
    
    private void GetInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //editor menu
        }

        if(Input.GetKey(KeyCode.W))
        {
            //y up
            _Direction.y = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            //x left
            _Direction.x = -1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            // y down
            _Direction.y = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            //x rigth
            _Direction.x = 1;
        }
    }

    private void MoveCamera()
    {
        transform.position += _Direction * MoveSpeed;
        _Direction = Vector3.zero;
    }
}
