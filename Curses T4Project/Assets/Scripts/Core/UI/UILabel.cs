//UILabel.cs
//by: MAURIZIO FISCHETTI

using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UILabel : MonoBehaviour
{
    public Image DefaultLabel;
    public Image HealthIcon;
    public Image CannonballsIcon;
    public Image DoubloonsIcon;
    public Image FlagIcon;

    public TMP_Text DefaultLabelText;
    public TMP_Text DefaultLabelIconText;
    public TMP_Text LabelText;

    private Transform _Parent = null;
    private Vector3 _Offset = Vector3.zero;

    private bool _IsTemporary = false;
    private float _LifeTime = 0;

    private bool _IsAnimated = false;
    private Vector3 _AnimationDirection = Vector3.zero;
    private float _Speed = 0;
   

    public enum LabelIconStyles
    {
        None,
        Default,
        Health,
        Cannonballs,
        Doubloons,
        Flags,
    }
    public LabelIconStyles LabelStyle;

    public void HideAll()
    {
        HealthIcon.gameObject.SetActive(false);
        CannonballsIcon.gameObject.SetActive(false);
        DoubloonsIcon.gameObject.SetActive(false);
        FlagIcon.gameObject.SetActive(false);
        LabelText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Show the label object
    /// </summary>
    /// <param name="ls">the icon style</param>
    /// <param name="Text">the text of the label</param>
    /// <param name="relPosition">the relative position (world if parent null, offset if parent not null)</param>
    /// <param name="parent">if parent not null position became the offset</param>
    /// <param name="lifetime">The lifetime of the label. 0: not temporary</param>
    /// <param name="animMovespeed">the speed of animation</param>
    /// <param name="animDir">the animation direction</param>
    public void ShowLabel(LabelIconStyles ls, string Text, Vector3 relPosition,Transform parent = null, float lifetime = 0, float animMovespeed = 0, Vector3 animDir = new Vector3())
    {
        HideAll();

        switch (ls)
        {
            case LabelIconStyles.None: { break; }
            case LabelIconStyles.Default: { DefaultLabel.gameObject.SetActive(true); LabelText.gameObject.SetActive(false); break; }
            case LabelIconStyles.Health: { HealthIcon.gameObject.SetActive(true); LabelText.gameObject.SetActive(true); break; }
            case LabelIconStyles.Cannonballs: { CannonballsIcon.gameObject.SetActive(true);LabelText.gameObject.SetActive(true); break; }
            case LabelIconStyles.Doubloons: { DoubloonsIcon.gameObject.SetActive(true); LabelText.gameObject.SetActive(true); break; }
            case LabelIconStyles.Flags: { FlagIcon.gameObject.SetActive(true); LabelText.gameObject.SetActive(true); break; }
        }
        LabelText.text = Text;
        DefaultLabelIconText.text = LabelText.text;

        transform.position = relPosition;

        if (parent != null)
        {

            if(parent.gameObject.TryGetComponent<LevelEntity>(out LevelEntity le))
            {
                DefaultLabelText.text = le.EntityName;
            }
            else
            {
                DefaultLabelIconText.text = "";
            }

            _Offset = relPosition;
            _Parent = parent;
            transform.SetParent(_Parent);
        }

        SetLifetime(lifetime);
        SetAnimation(animMovespeed, animDir);
    }

    public void SetText(string text)
    {
        LabelText.text = text;
        DefaultLabelIconText.text = text;
    }

    /// <summary>
    /// Set the position of the label
    /// </summary>
    /// <param name="pos"></param>
    public void SetPosition(Vector3 pos, Transform parent = null)
    {
        if (parent != null)
        {
            transform.SetParent(parent);
            _Parent = parent;
        }
        transform.position = pos;
    }

    /// <summary>
    /// Set Label LifeTime 0: permanent
    /// </summary>
    /// <param name="t"> 0: Not Temporary</param>
    public void SetLifetime(float t =0)
    {
        if(t <= 0)
        {
            _IsTemporary = false;
        }
        else
        {
            _IsTemporary = true;
            _LifeTime = t;
        }
    }

    /// <summary>
    /// Set the animation of the Label
    /// </summary>
    /// <param name="moveSpeed">the speed of the animation</param>
    /// <param name="direction">the direction of the animation</param>
    public void SetAnimation(float moveSpeed, Vector3 direction)
    {
        if(moveSpeed <= 0)
        {
            _IsAnimated = false;
        }
        else
        {
            _AnimationDirection = direction;
            _Speed = moveSpeed;
            _IsAnimated = true;
        }
    }

    private void Update()
    {
        if(_IsTemporary)
        {
            _LifeTime -= Time.deltaTime;
            if(_LifeTime < 0 )
            {
                Destroy(gameObject);
            }
        }

        if (_IsAnimated)
        {
            transform.position += _AnimationDirection.normalized * Time.deltaTime * _Speed;
        }
    }

    private void OnDestroy()
    {
        UIManager UIM = GameManager.Instance.UIManager;
        UIM.UILabelList.Remove(this);
    }



}
