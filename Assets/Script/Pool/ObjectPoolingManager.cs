using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
	[SerializeField] private EnemyPoolingManager EnemyPoolingManager;
	[SerializeField] private ProjectilePoolingManager projectilesPoolingManager;

	public void Initialize() {
		EnemyPoolingManager.Initialize();
		projectilesPoolingManager.Initialize();
	}

	public Enemy EnemySpawn(EnemyID enemyID, Vector3 position) {
		return EnemyPoolingManager.Spawn(enemyID, position);
	}

	public BaseProjectile ProjectileSpawn(ProjectileID projectileID, Vector3 position, Quaternion rotation, float damage, Color color, ProjectileType type) {
		return projectilesPoolingManager.Spawn(projectileID, position, rotation, damage, color, type);
	}
}

