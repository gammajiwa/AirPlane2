using UnityEngine;
using System;
public class BossSpawner : MonoBehaviour
{
	public Action BossEvent;
	[SerializeField] private int countEnemyDead;

	public int CountingEnemy(int value, int maxEnemyCount) {
		if (countEnemyDead < maxEnemyCount)
			countEnemyDead += value;
		else {
			BossEvent.Invoke();
			countEnemyDead = 0;
		}
		return countEnemyDead;
	}

	public void ResetCounting() {
		countEnemyDead = 0;
	}

}
