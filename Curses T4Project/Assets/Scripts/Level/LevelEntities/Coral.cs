using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coral : LevelEntity
{
    [Header("Change Mesh Color")]
    [SerializeField] private MeshRenderer _CoralMesh;
    [SerializeField] private Color _newCoralColor;

    private void Awake()
    {
        _CoralMesh.material.color = _newCoralColor;
    }
}
