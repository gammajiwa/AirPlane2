using UnityEngine;
using System;
public class Projectile001 : BaseProjectile
{
	private void OnTriggerEnter2D(Collider2D collision) {
		Enemy enemy = collision.gameObject.GetComponent<Enemy>();
		if (!(enemy is null)) {
			enemy.EnemyDamageEvent.Invoke(enemy, damage);
			ReturnToPool();
		}
		BaseProjectile baseProjectile = collision.gameObject.GetComponent<BaseProjectile>();
		if (!(baseProjectile is null)) {
			switch (baseProjectile.projectileType) {
				case ProjectileType.Normal:
					break;
				case ProjectileType.Destructible:
					ReturnToPool();
					DestructibleEvent?.Invoke(baseProjectile);
					break;
				case ProjectileType.Blocking:
					ReturnToPool();
					break;
				case ProjectileType.Debuff:
					DebuffProjectileEvent?.Invoke(baseProjectile, BuffID.Buff001);
					DebuffProjectileEvent?.Invoke(baseProjectile, BuffID.Buff002);
					break;
			}
		}
	}
}
