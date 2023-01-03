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
	[SerializeField] private List<Buff> buffEffect = new List<Buff>();
	private List<Enemy> enemies = new List<Enemy>();
	private List<BaseProjectile> projectiles = new List<BaseProjectile>();


	private void Start() {
		for (int i = 0; i < buffEffect.Count; i++) {
			buffEffect[i].value = BuffsData.GetFloat(buffEffect[i].buffID, BuffParameter.Value);
		}
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

	private void OnEnemyAttack(Enemy enemy, float damage, Quaternion rotation, ProjectileType type) {
		SetProjectile(ProjectileID.Projectile002, enemy.ProjectilePosition.position, rotation, enemy.AttackDamage, enemy.ProjectileColor, type);
	}

	private void OnPlayerAttack() {
		SetProjectile(ProjectileID.Projectile001, player.ProjectilePosition.position, Quaternion.identity, player.AttackDamage, Color.yellow, ProjectileType.Normal);
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


	private void SetProjectile(ProjectileID enumTypePool, Vector3 position, Quaternion rotation, float damage, Color color, ProjectileType type) {
		BaseProjectile projectile = objectpool.ProjectileSpawn(enumTypePool, position, rotation, damage, color, type);
		if (projectile.IsNotInitialized()) {
			projectile.Initialize(enumTypePool);
			projectile.DestructibleEvent += OnDestructibleProjectile;
			projectile.DebuffProjectileEvent += OnDebuffProjectile;
		}
		projectiles.Add(projectile);
	}

	private void OnDebuffProjectile(BaseProjectile baseProjectile, BuffID buffID) {
		for (int i = 0; i < buffEffect.Count; i++) {
			if (buffEffect[i].buffID == buffID) {
				baseProjectile.DebuffEffect(buffEffect[i], buffEffect[i].value);
			}
		}
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

