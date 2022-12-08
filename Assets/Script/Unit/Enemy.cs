using System;
using UnityEngine;

public class Enemy : Unit 
{
	public delegate void ShootEnemy(Enemy enemy, float damage, Quaternion rotation);
	public delegate void EnemyDamage(Enemy enemy, float damage);
	public Action<Enemy> EnemyDeactivateEvent;
	public EnemyDamage EnemyDamageEvent;
	public ShootEnemy ShootEnemyEvent;

	public EnemyID enemyID;
	public Transform ProjectilePosition;
	public Color ProjectileColor;
	private float backToPool = -6f;
	private float hpEnemy;

	protected override void UnitUpdate() {
		EnemyMovement();
		BackToPool();
	}

	private void EnemyMovement() {
		transform.Translate(Vector2.down * (Time.deltaTime * SpeedMovement));
	}

	protected override void Initialized() {
		HPUnit = EnemyData.GetFloat(enemyID, EnemyParameter.HP);
		SpeedMovement = EnemyData.GetFloat(enemyID, EnemyParameter.MoveSpeed);
		AttackDamage = EnemyData.GetFloat(enemyID, EnemyParameter.Damage);
		intervalAttack = EnemyData.GetFloat(enemyID, EnemyParameter.IntervalAttack);
		hpEnemy = HPUnit;
	}

	private void BackToPool() {
		if (transform.position.y <= backToPool)
			DeactivateEnemy();
	}

	public void EnemyGetDamage(float damage) {
		hpEnemy -= damage;
		if (hpEnemy <= 0) {
			hpEnemy = HPUnit;
			DeactivateEnemy();
		}
	}

	public void DeactivateEnemy() {
		ReturnToPool();
		transform.position = new Vector2(0, -7);
		EnemyDeactivateEvent?.Invoke(this);
	}

}
