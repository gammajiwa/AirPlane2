using UnityEngine;
using System;
using System.Collections.Generic;

public class Player : Unit
{
	public delegate void BuffPlayer(float duration, float maxDuration);
	public delegate void BuffDuration(float duration, float maxDuration);
	public BuffPlayer BuffPlayerEvent;
	public BuffDuration BuffDurationEvent;
	public Action<float> DamagePlayerEvent;
	public Action ShootEvent;

	public Transform ProjectilePosition;
	[SerializeField] private PlayerID playerID;
	private List<Buff> buffEffects = new List<Buff>();
	private float specialMovement;

	private void Start() {
		Initialize();
	}
	protected override void UnitUpdate() {
		PlayerMovement();
		TimerBuff();
		if (specialMovement <= 0) {
			if (Input.GetKey(KeyCode.Space)) {
				BuffEffect(BuffID.Buff002);
				BuffEffect(BuffID.Buff003);
				BuffEffect(BuffID.Buff004);
				specialMovement = PlayerData.GetFloat(playerID, PlayerParameter.SpecialMovementDuration);
			}
		}
		else {
			specialMovement -= 1 * Time.deltaTime;
			BuffPlayerEvent?.Invoke(specialMovement, PlayerData.GetFloat(playerID, PlayerParameter.SpecialMovementDuration));
		}
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
		Initialize();
		nextShoot = 0;
	}

	protected override void SetProjectile() {
		ShootEvent.Invoke();
	}

	public void Initialize() {
		HPUnit = PlayerData.GetFloat(playerID, PlayerParameter.HP);
		SpeedMovement = PlayerData.GetFloat(playerID, PlayerParameter.MoveSpeed);
		AttackDamage = PlayerData.GetFloat(playerID, PlayerParameter.Damage);
		intervalAttack = PlayerData.GetFloat(playerID, PlayerParameter.IntervalAttack);
		specialMovement = PlayerData.GetFloat(playerID, PlayerParameter.SpecialMovementDuration);
	}


	private void BuffEffect(BuffID buff) {
		if (!IsBuffed(buff)) {
			switch (buff) {
				case BuffID.Buff002:
					AttackDamage *= BuffsData.GetFloat(buff, BuffParameter.Value);
					break;
				case BuffID.Buff003:
					SpeedMovement = BuffsData.GetFloat(buff, BuffParameter.Value);
					break;
				case BuffID.Buff004:
					intervalAttack = BuffsData.GetFloat(buff, BuffParameter.Value);
					break;
			}
			buffEffects.Add(new Buff(buff, BuffsData.GetFloat(buff, BuffParameter.Value), BuffsData.GetFloat(buff, BuffParameter.Duration)));
		}
	}

	private bool IsBuffed(BuffID buffID) {
		for (int i = 0; i < buffEffects.Count; i++) {
			if (buffEffects[i].buffID == buffID) {
				buffEffects[i].duration = BuffsData.GetFloat(buffID, BuffParameter.Duration);
				return true;
			}
		}
		return false;
	}

	private void TimerBuff() {
		if (buffEffects.Count > 0) {
			for (int i = 0; i < buffEffects.Count; i++) {
				if (buffEffects[i].duration >= 0) {
					buffEffects[i].duration -= 1 * Time.deltaTime;
					BuffDurationEvent?.Invoke(buffEffects[0].duration, BuffsData.GetFloat(buffEffects[0].buffID, BuffParameter.Duration));
				}
				else {
					switch (buffEffects[i].buffID) {
						case BuffID.Buff002:
							AttackDamage = PlayerData.GetFloat(playerID, PlayerParameter.Damage);
							break;
						case BuffID.Buff003:
							SpeedMovement = PlayerData.GetFloat(playerID, PlayerParameter.MoveSpeed);
							break;
						case BuffID.Buff004:
							intervalAttack = PlayerData.GetFloat(playerID, PlayerParameter.IntervalAttack);
							break;
					}
					buffEffects.RemoveAt(i);
				}
			}
		}
	}
}
