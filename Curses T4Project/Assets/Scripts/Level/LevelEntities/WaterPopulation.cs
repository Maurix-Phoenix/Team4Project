//WaterPopulation.cs
//by MAURIZIO FISCHETTI

using System.Collections.Generic;
using UnityEngine;

public class WaterPopulation : MonoBehaviour
{
    [SerializeField] private int _MaxFishes = 15;

    public List<FishTemplate> FishesTemplates = new List<FishTemplate>();
    private List<Fish> FishesPool = new List<Fish>();
    private List<Fish> Fishes = new List<Fish>();

    //LocalCoords
    private Vector2 _XRange = new Vector2(-26f, 40f);
    private Vector2 _YRange = new Vector2(-10f, -1f);
    private Vector2 _ZRange = new Vector2(6, 23f);

    // Start is called before the first frame update
    void Start()
    {
        foreach (FishTemplate fishT in FishesTemplates)
        {
            for (int i = 0; i < fishT.Population * 2; i++)
            {
                Vector3 pos = Vector3.zero;
                pos.x = Random.Range(_XRange.x, _XRange.y);
                pos.y = Random.Range(_YRange.x, _YRange.y);
                pos.z = Random.Range(_ZRange.x, _ZRange.y);
                Fish newFish = Instantiate(fishT.fishPrefab, transform).GetComponent<Fish>();
                newFish.transform.position = pos;
                newFish.gameObject.SetActive(false);
                FishesPool.Add(newFish);
            }
        }

        for (int i = 0; i < _MaxFishes; i++)
        {
            SpawnFish(FishesPool[Random.Range(0, FishesPool.Count)], FishesPool[i].transform.position);
        }
    }

    private void SpawnFish(Fish fish, Vector3 position)
    {
        if(FishesPool.Count > 0 && Fishes.Count < _MaxFishes)
        {
            if (fish != null)
            {
                FishesPool.Remove(fish);
                Fishes.Add(fish);
                fish.transform.position = position;
                fish.gameObject.SetActive(true);
            }
        }

    }
    private void DespawnFish(Fish fish)
    {
        FishesPool.Add(fish);
        Fishes.Remove(fish);
        fish.gameObject.SetActive(false);
        fish.transform.position = RandomStartingPosition();
    }

    private Vector3 RandomStartingPosition()
    {
        Vector3 pos = Vector3.zero;

        pos.x = _XRange.y-1;
        pos.y = Random.Range(_YRange.x, _YRange.y);
        pos.z = Random.Range(_ZRange.x, _ZRange.y);

        return pos;
    }

    private void Update()
    {
        for (int i = Fishes.Count - 1; i > 0; i--)
        {
            if (Fishes[i] != null && (Fishes[i].transform.position.x <= _XRange.x || Fishes[i].transform.position.x >= _XRange.y))
            {
                DespawnFish(Fishes[i]);
                SpawnFish(FishesPool[Random.Range(0, FishesPool.Count)], RandomStartingPosition());
            }
        }
    }

}
