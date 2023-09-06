using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerChangeColor : MonoBehaviour
{
    [SerializeField] private float _YPosition = -1f;
    [SerializeField] private float _TransitionDuration = 0.5f;

    [SerializeField] private bool _StandardMaterialAcquired;

    [SerializeField] private List<Material> _StandardMaterial;
    [SerializeField] private Material _UnderwaterMaterial;
    [SerializeField] private List<Material> _CurrentMaterial;

    [SerializeField] private List<GameObject> _ShipMesh;

    private bool _UnderWaterColorAcquired;
    private bool _AboveWaterColorAcquired;


    // Start is called before the first frame update
    void Start()
    {
        if(!_StandardMaterialAcquired)
        {
            ObtainStandardMaterial();
        }
    }

    private void Update()
    {
        if (gameObject.transform.position.y < _YPosition)
        {
            if (!_UnderWaterColorAcquired)
            {
                ChangeMaterial();
            }
        }
        else
        {
            if (!_AboveWaterColorAcquired)
            {
                ChangeMaterial();
            }

        }
    }

    private void ObtainStandardMaterial()
    {
        for (int mesh = 0;  mesh < _ShipMesh.Count; mesh++)
        {
            for (int materials = 0; materials < _ShipMesh[mesh].GetComponent<MeshRenderer>().materials.Length; materials++)
            {
                Material mat = _ShipMesh[mesh].GetComponent<MeshRenderer>().materials[materials];
                _StandardMaterial.Add(mat);
            }
        }
        _StandardMaterialAcquired = true;
    }

    private void ChangeMaterial()
    {
        if (gameObject.transform.position.y < _YPosition)
        {
            _AboveWaterColorAcquired = false;
            for (int mesh = 0; mesh < _ShipMesh.Count; mesh++)
            {
                for (int materials = 0; materials < _ShipMesh[mesh].GetComponent<MeshRenderer>().materials.Length; materials++)
                {
                    Debug.Log("Down");
                    Material standardMaterial = _StandardMaterial[materials];
                    _ShipMesh[mesh].GetComponent<MeshRenderer>().materials[materials].Lerp(standardMaterial, _UnderwaterMaterial, _TransitionDuration);
                }
            }
            _UnderWaterColorAcquired = true;
        }
        else
        {
            _UnderWaterColorAcquired = false;
            for (int mesh = 0; mesh < _ShipMesh.Count; mesh++)
            {
                for (int materials = 0; materials < _ShipMesh[mesh].GetComponent<MeshRenderer>().materials.Length; materials++)
                {
                    Debug.Log("Up");
                    Material standardMaterial = _StandardMaterial[materials];
                    _ShipMesh[mesh].GetComponent<MeshRenderer>().materials[materials].Lerp(_UnderwaterMaterial, standardMaterial, _TransitionDuration);
                }
            }
            _AboveWaterColorAcquired = true;
        }
    }
}
