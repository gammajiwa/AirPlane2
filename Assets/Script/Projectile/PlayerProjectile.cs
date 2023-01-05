using UnityEngine;
using System;
public class PlayerProjectile : BaseProjectile
{
	private void OnTriggerEnter2D(Collider2D collision) {
		Enemy enemy = collision.gameObject.GetComponent<Enemy>();
		if (!(enemy is null)) {
			enemy.EnemyDamageEvent.Invoke(enemy, damage);
			ReturnToPool();
		}
		BaseProjectile baseProjectile = collision.gameObject.GetComponent<BaseProjectile>();
		if (!(baseProjectile is null)) {
			switch (baseProjectile.projectileID) {
				case ProjectileID.Projectile001:
					break;
				case ProjectileID.Projectile002:
					DebuffProjectileEvent?.Invoke(baseProjectile, BuffID.Buff001);
					DebuffProjectileEvent?.Invoke(baseProjectile, BuffID.Buff002);
					break;
				case ProjectileID.Projectile003:
					ReturnToPool();
					DestructibleEvent?.Invoke(baseProjectile);
					break;
				case ProjectileID.Projectile004:
					ReturnToPool();
					break;
			}
		}
	}
}
