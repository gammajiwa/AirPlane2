using UnityEngine;


public class EnemyPooling : BaseObjectPooling<Enemy>
{
	private EnemyID cachedUnitID;

	public void Initialize(EnemyID unitID) {
		cachedUnitID = unitID;
		Initialize();
	}

	public Enemy Spawn(Vector3 position) {
		Enemy enemy = SpawnFromPool(position, Quaternion.identity);
		return enemy;
	}

}
