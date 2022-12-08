using UnityEngine;
using System;

public class Player : Unit
{
	public Action ShootEvent;
	public Action<float> DamagePlayerEvent;

	public Transform ProjectilePosition;

	protected override void UnitUpdate() {
		PlayerMovement();
	}

	private void PlayerMovement() {
		if (Input.GetKey(KeyCode.A)) {
			transform.Translate(Vector2.left * (Time.deltaTime * SpeedMovement));
		}
		if (Input.GetKey(KeyCode.D)) {
			transform.Translate(Vector2.right * (Time.deltaTime * SpeedMovement));
		}
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8f, 8f), transform.position.y, transform.position.z);
	}

	public void PlayerGetDamage(float damage) {
		HPUnit -= damage;
		Mathf.Clamp(HPUnit, 0, 10f);
		DamagePlayerEvent.Invoke(HPUnit);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		Enemy enemy = collision.GetComponent<Enemy>();
		if (!(enemy is null)) {
			PlayerGetDamage(enemy.AttackDamage * 3);
		}
	}

	public void ResetPlayerData() {
		transform.position = new Vector2(0, transform.position.y);
		HPUnit = 10;
		nextShoot = 0;
	}

	protected override void SetProjectile() {
		ShootEvent.Invoke();
	}
}
