using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Boss : MonoBehaviour
{
	public delegate void ShootBoss(float damage, Quaternion rotation, ProjectileID projectileID, float position);
	public ShootBoss ShootBossEvent;
	public Action<float> DamagBossEvent;

	[SerializeField] private Transform playerPosition;
	[SerializeField] private float speed;
	[SerializeField] private float hpBoss;
	[SerializeField] private float attackDamage;
	[SerializeField] private float[] intervalAttacks;
	private int indexMove;
	private int indexAttack;
	private float timerRandomeMove;
	private float timerRandomeAttack;
	private float[] _intervalAttacks = new float[3];

	void Update() {

		movementBoss(RandomMovement());
		BossAttackPattern(RandomAttack());
	}

	private void movementBoss(int movement) {

		switch (movement) {
			case 0:
				transform.Translate(Vector3.right * speed * Time.deltaTime);
				break;
			case 1:
				transform.Translate(Vector3.left * speed * Time.deltaTime);
				break;
			case 2:
				transform.position = Vector3.MoveTowards(transform.position, new Vector2(0, transform.position.y), 2 * speed * Time.deltaTime);
				break;
			case 3:
				transform.position = Vector3.MoveTowards(transform.position, new Vector2(playerPosition.transform.position.x, transform.position.y), 2 * speed * Time.deltaTime);
				break;
		}
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8f, 8f), transform.position.y, transform.position.z);
	}

	private int RandomMovement() {
		timerRandomeMove -= 1 * Time.deltaTime;
		if (timerRandomeMove <= 0) {
			timerRandomeMove = UnityEngine.Random.Range(0.5f, 3f);
			if (hpBoss >= 50) return indexMove = UnityEngine.Random.Range(0, 3);
			else speed = 6; return indexMove = UnityEngine.Random.Range(0, 4);
		}
		if (transform.position.x >= 8) indexMove = 0;
		else if (transform.position.x <= -8) indexMove = 1;
		return indexMove;
	}

	public int RandomAttack() {
		timerRandomeAttack -= 1 * Time.deltaTime;
		if (timerRandomeAttack <= 0) {
			timerRandomeAttack = UnityEngine.Random.Range(1f, 4f);
			if (hpBoss >= 50) return indexAttack = UnityEngine.Random.Range(-10, 3);
			else return indexAttack = UnityEngine.Random.Range(-10, 5);
		}
		return indexAttack;
	}


	private void AttackType1() {
		ShootBossEvent.Invoke(attackDamage, Quaternion.Euler(0, 0, -180), ProjectileID.Projectile003, 0.5f);
		ShootBossEvent.Invoke(attackDamage, Quaternion.Euler(0, 0, -180), ProjectileID.Projectile003, -0.5f);
	}
	private void AttackType2() {
		Vector3 dir = playerPosition.transform.position - transform.position;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		ShootBossEvent.Invoke(attackDamage, Quaternion.Euler(0f, 0f, angle - 90), ProjectileID.Projectile002, 0.2f);
		ShootBossEvent.Invoke(attackDamage, Quaternion.Euler(0f, 0f, angle - 90), ProjectileID.Projectile002, -0.2f);
	}

	private void AttackType3() {
		float rotation = -215;
		for (int i = 0; i < 3; i++) {
			ShootBossEvent.Invoke(attackDamage, Quaternion.Euler(0, 0, rotation), ProjectileID.Projectile004, 0.7f);
			ShootBossEvent.Invoke(attackDamage, Quaternion.Euler(0, 0, rotation), ProjectileID.Projectile004, -0.7f);
			rotation += 35;
		}
	}
	private void BossAttackPattern(int index) {

		switch (index) {
			case 0:
				_intervalAttacks[1] = TurnAttack(AttackType2, _intervalAttacks[1], intervalAttacks[1]);
				break;
			case 1:
				_intervalAttacks[2] = TurnAttack(AttackType3, _intervalAttacks[2], intervalAttacks[2]);
				break;
			case 2:
				_intervalAttacks[0] = TurnAttack(AttackType1, _intervalAttacks[0], intervalAttacks[0]);
				_intervalAttacks[1] = TurnAttack(AttackType2, _intervalAttacks[1], intervalAttacks[1]);
				break;
			case 3:
				_intervalAttacks[0] = TurnAttack(AttackType1, _intervalAttacks[0], intervalAttacks[0]);
				_intervalAttacks[2] = TurnAttack(AttackType3, _intervalAttacks[2], intervalAttacks[2]);
				break;
			case 4:
				_intervalAttacks[2] = TurnAttack(AttackType3, _intervalAttacks[2], intervalAttacks[2]);
				_intervalAttacks[1] = TurnAttack(AttackType2, _intervalAttacks[1], intervalAttacks[1]);
				break;
			default:
				_intervalAttacks[0] = TurnAttack(AttackType1, _intervalAttacks[0], intervalAttacks[0]);
				break;
		}
	}

	public float TurnAttack(Action funtion, float value, float value2) {
		value -= 1 * Time.deltaTime;
		if (value <= 0) {
			funtion.Invoke();
			return value2;
		}
		return value;
	}

	public void BossActivated() {
		gameObject.SetActive(true);
		transform.position = new Vector2(0, 4);
		hpBoss = 100;
		speed = 3;
	}

	private void DamagBoos(float damage) {
		hpBoss -= damage;
		if (hpBoss <= 0) gameObject.SetActive(false);
		DamagBossEvent.Invoke(hpBoss);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		PlayerProjectile projectile = collision.gameObject.GetComponent<PlayerProjectile>();
		if (!(projectile is null)) {
			DamagBoos(projectile.damage);
		}
	}
}
