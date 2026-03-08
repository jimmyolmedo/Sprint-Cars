using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoin : MonoBehaviour
{
    bool startSpawned;

    [SerializeField] Coin prefab;

    [SerializeField] Transform[] positions;

    Vector3 lastPos;

    [SerializeField] int maxCoinCount;

    [SerializeField] List<GameObject> coinsSpawned = new List<GameObject>();

    [SerializeField] float timeToSpawn;

    private void OnEnable()
    {
        GameManager.OnStateChange += StartSpawn;
    }

    private void OnDisable()
    {
        GameManager.OnStateChange -= StartSpawn;
    }

    void spawnCoin()
    {
        bool ocupied = false;
        int indexPos = Random.Range(0, positions.Length);
        
        foreach (GameObject index in coinsSpawned)
        {
            if(index.transform.position == positions[indexPos].position)
            {
                ocupied = true;
            }
        }
        if(ocupied == false || positions[indexPos].position == lastPos)
        {
            GameObject obj = Instantiate(prefab.gameObject, positions[indexPos]);
            coinsSpawned.Add(obj);
            obj.GetComponent<Coin>().spawner = this;
        }
        else
        {
            spawnCoin();    
        }
        
    }

    IEnumerator SpawnCoroutine()
    {
        if(coinsSpawned.Count == maxCoinCount) { yield break; }

        yield return new WaitForSeconds(timeToSpawn);

        spawnCoin();
    }

    void StartSpawn(GameState state)
    {
        if(state != GameState.Gameplay && startSpawned == true) { return; }

        for(int i = 0; i < maxCoinCount; i++)
        {
            spawnCoin();
        }
    }

    public void DespawnCoin(GameObject coin)
    {
        coinsSpawned.Remove(coin);
        lastPos = coin.transform.position;
        StartCoroutine(SpawnCoroutine());
    }
}
