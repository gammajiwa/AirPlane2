using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class GameManagers : MonoBehaviour
{
	[SerializeField] private Player player;
	[SerializeField] private ObjectPoolingManager objectpool;
	[SerializeField] private EnemySpawner enemySpawner;
	[SerializeField] private BossSpawner bossSpawner;
	[SerializeField] private Boss boss;
	[SerializeField] private UIManager uiManager;
	[SerializeField] private GameState gameState;
	[SerializeField] private int maxEnemyCount;
	private List<Enemy> enemies = new List<Enemy>();
	private List<BaseProjectile> projectiles = new List<BaseProjectile>();


	private void Start() {
		player.ShootEvent += OnPlayerAttack;
		player.DamagePlayerEvent += OnPlayerGetDamage;
		player.BuffPlayerEvent += OnBuffPlayer;
		player.BuffDurationEvent += OnBuffDuration;
		enemySpawner.SpawnEnemyEvent += OnSpawnEnemy;
		bossSpawner.BossEvent += OnBossSpawn;
		objectpool.Initialize();
		boss.ShootBossEvent += OnBossAttack;
		boss.DamagBossEvent += OnBossGetDamage;
	}

	private void OnBossGetDamage(float hpBoss) {
		uiManager.BossHpBar(hpBoss);
		if (hpBoss <= 0) {
			uiManager.GameOver("YOU WIN");
			Time.timeScale = 0;
		}
	}

	private void OnBuffDuration(float duration, float maxDuration) {
		uiManager.BuffDuration(duration, maxDuration);
	}

	private void OnBuffPlayer(float duration, float maxDuration) {
		uiManager.SpecialMovement(duration, maxDuration);
	}

	private void OnBossSpawn() {
		gameState = GameState.Boss;
		uiManager.BossBattles(true);
		boss.BossActivated();
		RemoveEnemy();

	}

	private void Update() {
		enemySpawner.SpawnEnemy();
	}

	private void OnPlayerGetDamage(float hpPlayer) {
		uiManager.UpdateHpBar(hpPlayer);
		if (hpPlayer <= 0) {
			uiManager.GameOver("GAME OVER");
			Time.timeScale = 0;
		}
	}

	private void OnEnemyAttack(Enemy enemy, float damage, Quaternion rotation, ProjectileID projectileID) {
		SetProjectile(projectileID, enemy.ProjectilePosition.position, rotation, enemy.AttackDamage);
	}
	private void OnBossAttack(float damage, Quaternion rotation, ProjectileID projectileID, float positionOffsite) {
		SetProjectile(projectileID, new Vector3(boss.transform.position.x + positionOffsite, boss.transform.position.y), rotation, damage);
	}

	private void OnPlayerAttack() {
		SetProjectile(ProjectileID.Projectile001, player.ProjectilePosition.position, Quaternion.identity, player.AttackDamage);
	}

	private void OnSpawnEnemy(float position, EnemyID enemyType) {
		if (gameState == GameState.Waves) {
			Enemy enemy = objectpool.EnemySpawn(enemyType, new Vector3(position, 7));
			if (enemy.IsNotInitialized()) {
				enemy.Initialize(enemyType);
				enemy.EnemyDamageEvent += OnEnemyGetDamage;
				enemy.ShootEnemyEvent += OnEnemyAttack;
				enemy.EnemyDestroyedEvent += OnEnemyDestroy;
			}
			enemies.Add(enemy);
		}
	}

	private void OnEnemyDestroy() {
		uiManager.EnemyCounting(bossSpawner.CountingEnemy(1, maxEnemyCount), maxEnemyCount);
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
		RemoveEnemy();
		uiManager.UpdateHpBar(10);
		player.ResetPlayerData();
		gameState = GameState.Waves;
		Time.timeScale = 1;
		boss.gameObject.SetActive(false);
		bossSpawner.ResetCounting(); 
		uiManager.EnemyCounting(0, maxEnemyCount);
		uiManager.BossBattles(false); ;
	}

	private void RemoveEnemy() {
		for (int i = 0; i < projectiles.Count; i++) {
			projectiles[i].DeactivateProjectile();
		}
		for (int i = 0; i < enemies.Count; i++) {
			enemies[i].DeactivateEnemy();
		}
	}

}

