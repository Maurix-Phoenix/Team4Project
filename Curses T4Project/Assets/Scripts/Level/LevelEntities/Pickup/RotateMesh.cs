using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMesh : MonoBehaviour
{
    [SerializeField] private GameObject _Mesh;
    [SerializeField] private Vector3 _RotationalSpeed;

    // Update is called once per frame
    void Update()
    {
        _Mesh.gameObject.transform.Rotate(_RotationalSpeed * 100 * Time.deltaTime);
    }
}
