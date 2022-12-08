using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public delegate void EnemySpawn(float position, EnemyID typeEnemy);

    public EnemySpawn SpawnEnemyEvent;
    [SerializeField] private float randomPositionMin;
    [SerializeField] private float randomPositionMax;
    [SerializeField] private float randomIntervalMin;
    [SerializeField] private float randomIntervalMax;
    private float timeSpawn;

    public void SpawnEnemy() {
        timeSpawn -= 1 * Time.deltaTime;
        if (timeSpawn <= 0) {
            float randomPos = UnityEngine.Random.Range(randomPositionMin, randomPositionMax);
            timeSpawn = UnityEngine.Random.Range(randomIntervalMin, randomIntervalMax);
			SpawnEnemyEvent.Invoke(randomPos, (EnemyID)UnityEngine.Random.Range(0, 3));
		}
	}
}
