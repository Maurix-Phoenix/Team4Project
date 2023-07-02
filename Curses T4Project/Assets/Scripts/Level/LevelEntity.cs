using System.Collections;
//LevelEntity.cs
//by MAURIZIO FISCHETTI
using UnityEngine;

public class LevelEntity : MonoBehaviour
{
    [SerializeField] public Vector3 Position;
    [SerializeField] public Quaternion Rotation;
    [SerializeField] public Vector3 Scale;

    protected virtual void Start()
    {
        Position = transform.localPosition;
        Rotation = transform.localRotation;
        Scale = transform.localScale;
    }
}
