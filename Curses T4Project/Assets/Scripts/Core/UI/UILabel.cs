using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class UILabel : MonoBehaviour
{
    private TMP_Text _TextObj;

    private Transform _Parent = null;
    private string _Text = null;
    private float _LifeTime = 3.0f;
    private bool _IsPermanent = false;

    private void OnEnable()
    {
        _TextObj = transform.GetComponent<TMP_Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_IsPermanent)
        {
            if (_LifeTime > 0.0f)
            {
                _LifeTime -= Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Update the label
    /// </summary>
    /// <param name="text">the text of the label</param>
    /// <param name="position">the world coords (or parent local)</param>
    /// <param name="parent">the parent object if specified the position will be in Local coords</param>
    /// <param name="lifetime">the lifetime in seconds of the label 0: permanent</param>
    public void UpdateLabel(string text, Vector3 position, Transform parent = null, float lifetime = 0)
    {
        transform.position = position;

        _Text = text;
        _Parent = parent;
        _LifeTime = lifetime;

        if(_LifeTime == 0)
        {
            _IsPermanent = true;
        }

        if(_Parent != null)
        {
            transform.SetParent(_Parent);
            transform.position = _Parent.position + position;
        }
        _TextObj.text = _Text;
    }
}
