using UnityEngine;

public class Enemy002Controller : Enemy
{
	protected override void SetProjectile() {
		ShootEnemyEvent.Invoke(this, AttackDamage, Quaternion.Euler(0, 0, -180));
	}
}

