using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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

	private void OnEnemyAttack(Enemy enemy, float damage, Quaternion rotation) {
		SetProjectile(ProjectileID.Projectile002, enemy.ProjectilePosition.position, rotation, enemy.AttackDamage, enemy.ProjectileColor);
	}

	private void OnPlayerAttack() {
		SetProjectile(ProjectileID.Projectile001, player.ProjectilePosition.position, Quaternion.identity, player.AttackDamage, Color.yellow);
	}

	private void OnSpawnEnemy(float position, EnemyID enemyType) {
		Enemy enemy = objectpool.EnemySpawn(enemyType, new Vector3(position, 7));
		if (enemy.IsNotInitialized()) {
			enemy.Initialize(enemyType);
			enemy.EnemyDamageEvent += OnEnemyGetDamage;
			enemy.ShootEnemyEvent += OnEnemyAttack;
			enemy.EnemyDeactivateEvent += OnEnemyDeactivate;
		}
		enemies.Add(enemy);
	}


	private void SetProjectile(ProjectileID enumTypePool, Vector3 position, Quaternion rotation, float damage, Color color) {
		BaseProjectile projectile = objectpool.ProjectileSpawn(enumTypePool, position, rotation, damage, color);
		if (projectile.IsNotInitialized()) {
			projectile.Initialize(enumTypePool);
			projectile.ProjectileDeactivateEvent += onProjectileDeactivate;
		}
		projectiles.Add(projectile);
	}


	private void OnEnemyGetDamage(Enemy enemy, float damage) {
		enemy.EnemyGetDamage(damage);
	}

	private void OnEnemyDeactivate(Enemy enemy) {
		StartCoroutine(WaitToRemoveEnemy(enemy));
	}

	private void onProjectileDeactivate(BaseProjectile projectile) {
		StartCoroutine(WaitToRemoveProjectile(projectile));
	}


	private IEnumerator WaitToRemoveEnemy(Enemy enemy) {
		yield return new WaitForSeconds(0.2f);
		enemies.Remove(enemy);
	}

	private IEnumerator WaitToRemoveProjectile(BaseProjectile projectile) {
		yield return new WaitForSeconds(0.2f);
		projectiles.Remove(projectile);
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

