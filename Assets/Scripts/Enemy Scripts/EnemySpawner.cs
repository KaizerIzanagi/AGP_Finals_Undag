using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject[] summonedEnemies;
    [SerializeField] private float _countdown, _spawnDelay;

    void Start()
    {
        _countdown = 10;
        SpawnDelay();
    }
    void Update()
    {
        _countdown -= Time.deltaTime;

        if (_countdown <= 0)
        {
            _spawnDelay -= Time.deltaTime;
            if (_spawnDelay <= 0)
            {
                if (summonedEnemies.Length <= 7)
                {
                    SpawnDelay();
                    Instantiate(_enemyPrefab);
                    summonedEnemies = GameObject.FindGameObjectsWithTag("Enemy");
                }
                else
                {
                    SpawnDelay();
                    Debug.Log("Cry About it");
                }
                
            }
        }

        
    }

    public void SpawnDelay()
    {
        _spawnDelay = Random.Range(0.25f, 1f);
    }
        
}
