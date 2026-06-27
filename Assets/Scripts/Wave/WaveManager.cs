using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private LivingEntity playerEntity;


    public static float timeBetweenWaves = 3.0f;
    public  int waveNumber;
    public static event System.Action<int> OnNewWave;
    public Wave[] waves;
    private Wave _currentWave;
    private int _currentWaveIndex;
    private bool _isWaveValid = true;

    private int _enemyRemainingAliveCount;
    private float _upgrade;



    private void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("Can't find enemy spawn point, please chect it!");
            return;
        }

        if (playerEntity == null)
        {
            Debug.LogError("Can't find player enetity, please chect it!");
            return;
        }

        playerEntity.OnDeath += OnPlayerDeath;
        



        StartCoroutine(NextWaveCoroutine());
    }

    private IEnumerator NextWaveCoroutine()
    {
        _currentWaveIndex++;

        if (_isWaveValid)
        {
            
            OnNewWave?.Invoke(++waveNumber);
        }
        
        yield return new WaitForSeconds(timeBetweenWaves);

        if (_currentWaveIndex - 1 < waves.Length)
        {
            
            _currentWave = waves[_currentWaveIndex - 1];
            _enemyRemainingAliveCount = _currentWave.count;
            if (playerEntity == null)
            {
                yield break;
            }
            for(int i = 0; i < _currentWave.count; i++)
            {
                int spawnIndex = Random.Range(0, spawnPoints.Length);
                Beetle beetle = Instantiate(_currentWave.beetle, spawnPoints[spawnIndex].position, Quaternion.identity);
                beetle.Setup(playerEntity.transform, _upgrade);
                beetle.OnDeath += OnEnemyDeath;
                yield return new WaitForSeconds(_currentWave.timeBetweenSpawn);
            }

            _isWaveValid = true;

        }

        else
        {
            _currentWaveIndex = 0;
            _isWaveValid = false;

            //难度提升
            _upgrade += 0.1f;
            StartCoroutine(NextWaveCoroutine());

        }
    }

    private void OnEnemyDeath()
    {
        _enemyRemainingAliveCount--;
        if (_enemyRemainingAliveCount == 0)
        {
            StartCoroutine(NextWaveCoroutine());

        }

    }

    private void OnPlayerDeath()
    {
        StopCoroutine(NextWaveCoroutine());
        
    }
  
}
