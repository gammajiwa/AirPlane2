using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class GameManagers : MonoBehaviour
{
	[SerializeField] private Player player;
	[SerializeField] private ObjectPoolingManager objectpool;
	[SerializeField] private EnemySpawner enemySpawner;
	[SerializeField] private UIManager uiManager;
	private List<Enemy> enemies = new List<Enemy>();
	private List<BaseProjectile> projectiles = new List<BaseProjectile>();


	private void Start() {
		player.ShootEvent += OnPlayerAttack;
		player.DamagePlayerEvent += OnPlayerGetDamage;
		enemySpawner.SpawnEnemyEvent += OnSpawnEnemy;
		objectpool.Initialize();
	}

	private void Update() {
		enemySpawner.SpawnEnemy();
	}

	private void OnPlayerGetDamage(float hpPlayer) {
		uiManager.UpdateHpBar(hpPlayer);
		if (hpPlayer <= 0) {
			uiManager.GameOver();
			Time.timeScale = 0;
		}
	}

	private void OnEnemyAttack(Enemy enemy, float damage, Quaternion rotation, ProjectileID projectileID) {
		SetProjectile(projectileID, enemy.ProjectilePosition.position, rotation, enemy.AttackDamage);
	}

	private void OnPlayerAttack() {
		SetProjectile(ProjectileID.Projectile001, player.ProjectilePosition.position, Quaternion.identity, player.AttackDamage);
	}

	private void OnSpawnEnemy(float position, EnemyID enemyType) {
		Enemy enemy = objectpool.EnemySpawn(enemyType, new Vector3(position, 7));
		if (enemy.IsNotInitialized()) {
			enemy.Initialize(enemyType);
			enemy.EnemyDamageEvent += OnEnemyGetDamage;
			enemy.ShootEnemyEvent += OnEnemyAttack;
		}
		enemies.Add(enemy);
	}


	private void SetProjectile(ProjectileID enumTypePool, Vector3 position, Quaternion rotation, float damage) {
		BaseProjectile projectile = objectpool.ProjectileSpawn(enumTypePool, position, rotation);
		if (projectile.IsNotInitialized()) {
			projectile.Initialize(enumTypePool);
			projectile.DestructibleEvent += OnDestructibleProjectile;
			projectile.DebuffProjectileEvent += OnDebuffProjectile;
			projectile.SetDamage(damage);
		}
		projectiles.Add(projectile);
	}

	private void OnDebuffProjectile(BaseProjectile baseProjectile, BuffID buffID) {
		baseProjectile.DebuffEffect(buffID);
	}

	private void OnDestructibleProjectile(BaseProjectile baseProjectile) {
		baseProjectile.DeactivateProjectile();
	}

	private void OnEnemyGetDamage(Enemy enemy, float damage) {
		enemy.EnemyGetDamage(damage);
	}

	public void ResetGame() {

		for (int i = 0; i < projectiles.Count; i++) {
			projectiles[i].DeactivateProjectile();
		}
		for (int i = 0; i < enemies.Count; i++) {
			enemies[i].DeactivateEnemy();
		}
		uiManager.UpdateHpBar(10);
		player.ResetPlayerData();
		Time.timeScale = 1;
	}
}

