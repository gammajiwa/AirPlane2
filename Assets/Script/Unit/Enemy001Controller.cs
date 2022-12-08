using UnityEngine;

public class Enemy001Controller : Enemy
{
	private Transform player;
	protected override void Initialized() {
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	protected override void SetProjectile() {
		Vector3 dir = player.transform.position - transform.position;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		ShootEnemyEvent.Invoke(this, AttackDamage, Quaternion.Euler(0f, 0f, angle - 90));
	}
}
