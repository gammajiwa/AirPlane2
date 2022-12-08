using UnityEngine;
using System.Collections.Generic;

public class EnemyPoolingManager : MonoBehaviour
{
	[SerializeField] private EnemyPooling enemyPooling001;
	[SerializeField] private EnemyPooling enemyPooling002;
	[SerializeField] private EnemyPooling enemyPooling003;
	private Dictionary<EnemyID, EnemyPooling> enemies = new Dictionary<EnemyID, EnemyPooling>();

	public void Initialize() {
		
		enemies.Add(EnemyID.Enemy001, enemyPooling001);
		enemies.Add(EnemyID.Enemy002, enemyPooling002);
		enemies.Add(EnemyID.Enemy003, enemyPooling003);
		foreach (EnemyPooling enemyPooling in enemies.Values) {
			enemyPooling.Initialize();
		}
	}

	public Enemy Spawn(EnemyID enemyID, Vector3 position) {
	return	enemies[enemyID].Spawn(position);
	}

}
