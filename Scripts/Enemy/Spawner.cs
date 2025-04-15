using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class WaveContent
    {
        [SerializeField]
        GameObject[] monsterSpawner;

        public GameObject[] GetMonsterSpawnList()
        {
            return monsterSpawner;
        }
    }

    [SerializeField] WaveContent[] waves;
    [SerializeField] public TextMeshProUGUI roundNum;
    int currentWave = 0;
    float spawnRange = 10;
    public int enemiesKilled;
    private float spawnDuration = 8.0f;
    AudioSource roundSound;

    // Start is called before the first frame update
    void Start()
    {
        roundSound = GetComponent<AudioSource>();
        SpawnWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesKilled >= waves[currentWave].GetMonsterSpawnList().Length)
        {
            enemiesKilled = 0;
            Debug.Log("All Things Killed");
            SoundManager.PlaySound(SoundType.ROUNDEND);
            SpawnTimer();
        }
        roundNum.text = "" + (currentWave + 1);
    }

    void SpawnWave()
    {
        if (waves == null || waves.Length <= currentWave)
        {
            Debug.LogError("Waves array is null or current wave index is out of bounds!");
            return;
        }

        var spawnList = waves[currentWave].GetMonsterSpawnList();
        if (spawnList == null || spawnList.Length == 0)
        {
            Debug.LogError($"Wave {currentWave} has no monsters to spawn!");
            return;
        }

        for (int i = 0; i < spawnList.Length; i++)
        {
            Debug.Log($"Spawning {spawnList[i].name} at wave {currentWave}");
            Instantiate(spawnList[i], FindSpawnLoc(), Quaternion.identity);
        }

        SoundManager.PlaySound(SoundType.ROUNDSTART);
    }

    Vector3 FindSpawnLoc()
    {
        float xLoc = Random.Range(-spawnRange, spawnRange) + transform.position.x;
        float zLoc = Random.Range(-spawnRange, spawnRange) + transform.position.z;
        float yLoc = transform.position.y;

        Vector3 spawnPos = new Vector3(xLoc, yLoc, zLoc);
        Debug.Log($"Trying spawn location: {spawnPos}");

        if (Physics.Raycast(spawnPos, Vector3.down, 5))
        {
            Debug.Log($"Valid spawn location: {spawnPos}");
            return spawnPos;
        }
        else
        {
            Debug.LogWarning($"Invalid spawn location: {spawnPos}, retrying...");
            return FindSpawnLoc();
        }
    }

    void SpawnTimer()
    {
        StartCoroutine(DisableSpawnTime(spawnDuration));
    }

    private IEnumerator DisableSpawnTime(float duration)
    {
        yield return new WaitForSeconds(duration);

        currentWave++;
        SpawnWave();
    }


}
