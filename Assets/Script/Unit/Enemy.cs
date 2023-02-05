using System;
using UnityEngine;

public class Enemy : Unit
{
	public delegate void ShootEnemy(Enemy enemy, float damage, Quaternion rotation, ProjectileID projectileID);
	public delegate void EnemyDamage(Enemy enemy, float damage);
	public EnemyDamage EnemyDamageEvent;
	public ShootEnemy ShootEnemyEvent;
	public Action EnemyDestroyedEvent;

	public EnemyID enemyID;
	public Transform ProjectilePosition;
	public Color ProjectileColor;
	private float backToPool = -6f;

	protected override void UnitUpdate() {
		EnemyMovement();
		BackToPool();
	}

	private void EnemyMovement() {
		transform.Translate(Vector2.down * (Time.deltaTime * SpeedMovement));
	}

	private void BackToPool() {
		if (transform.position.y <= backToPool)
			DeactivateEnemy();
	}

	public void EnemyGetDamage(float damage) {
		HPUnit -= damage;
		if (HPUnit <= 0) {
			HPUnit = EnemyData.GetFloat(enemyID, EnemyParameter.HP);
			DeactivateEnemy();
			EnemyDestroyedEvent.Invoke();
		}
	}

	public void DeactivateEnemy() {
		ReturnToPool();
	}

	public void Initialize(EnemyID cachedUnitID) {
		HPUnit = EnemyData.GetFloat(enemyID, EnemyParameter.HP);
		SpeedMovement = EnemyData.GetFloat(enemyID, EnemyParameter.MoveSpeed);
		AttackDamage = EnemyData.GetFloat(enemyID, EnemyParameter.Damage);
		intervalAttack = EnemyData.GetFloat(enemyID, EnemyParameter.IntervalAttack);
	}

}
