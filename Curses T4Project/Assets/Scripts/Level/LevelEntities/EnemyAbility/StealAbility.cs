//StealAbility.cs
//by ANTHONY FEDELI

using UnityEngine;

/// <summary>
/// StealAbility.cs give to the gameobject the ability of steal resources from the player when enter in collision with it.
/// </summary>

public class StealAbility : MonoBehaviour
{
    public enum ResourceToSteal
    {
        None,
        Doubloons,
        Cannonballs,
        Both
    }

    [SerializeField] private ResourceToSteal ResourcesStealed;
    [SerializeField] private bool _HasStolen = false;

    [Header("Quantity")]
    [SerializeField] private int MaxCannonballStealable = 1;
    [SerializeField] private int MinCannonballStealable = 1;
    [SerializeField] private int MaxDoubloonsStealable = 1;
    [SerializeField] private int MinDoubloonsStealable = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Player>() != null && !_HasStolen)
        {
            _HasStolen = true;
            int CannonballStealed = Random.Range(MinCannonballStealable, MaxCannonballStealable);
            int DoubloonsStealed = Random.Range(MinDoubloonsStealable, MaxDoubloonsStealable);

            switch (ResourcesStealed)
            {
                case ResourceToSteal.Cannonballs:
                    other.gameObject.GetComponentInParent<Player>().AddResource(T4P.T4Project.PickupsType.Cannonball, -CannonballStealed);
                    break;

                case ResourceToSteal.Doubloons:
                    other.gameObject.GetComponentInParent<Player>().AddResource(T4P.T4Project.PickupsType.Doubloon, -DoubloonsStealed);
                    break;

                case ResourceToSteal.Both:
                    other.gameObject.GetComponentInParent<Player>().AddResource(T4P.T4Project.PickupsType.Cannonball, -CannonballStealed);
                    other.gameObject.GetComponentInParent<Player>().AddResource(T4P.T4Project.PickupsType.Doubloon, -DoubloonsStealed);
                    break;
            }
        }
    }
}