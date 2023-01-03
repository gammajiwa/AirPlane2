
using UnityEngine;

public class Enemy003Controller : Enemy
{
	protected override void SetProjectile() {
		float rotation = -215;
		for (int i = 0; i < 3; i++) {
			ShootEnemyEvent.Invoke(this, AttackDamage, Quaternion.Euler(0, 0, rotation), ProjectileType.Blocking);
			rotation += 35;
		}
	}
}
