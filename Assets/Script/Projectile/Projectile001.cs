using UnityEngine;

public class Projectile001 : BaseProjectile 
{
	private void OnTriggerEnter2D(Collider2D collision) {
		Enemy enemy = collision.gameObject.GetComponent<Enemy>();
		if (!(enemy is null)) {
			enemy.EnemyDamageEvent.Invoke(enemy, damage);
			ReturnToPool();

		}
	}
}
