using System;
using UnityEngine;
using System.Collections.Generic;

public class BaseProjectile : PooledObject
{
	public delegate void DebuffProjectile(BaseProjectile projectile, BuffID buffID);
	public DebuffProjectile DebuffProjectileEvent;
	public Action<BaseProjectile> DestructibleEvent;

	[HideInInspector] public ProjectileID projectileID;
	public Renderer projectileColor;
	[HideInInspector] public float damage;
	protected private float speedProjectile;
	protected private List<Buff> buffEffects = new List<Buff>();
	private float baseDamage;

	private void Update() {
		ProjectileMove();
		TimerBuff();
	}

	protected virtual void ProjectileMove() {
		transform.Translate(Vector2.up * (speedProjectile * Time.deltaTime));
		if (transform.position.y >= 7 || transform.position.y <= -7 || transform.position.x <= -11 || transform.position.x >= 11) {
			DeactivateProjectile();
		}
	}

	public void Initialize(ProjectileID cachedUnitID) {
		speedProjectile = ProjectileData.GetFloat(cachedUnitID, ProjectileParameter.Speed);
		projectileID = cachedUnitID;
	}

	public void SetDamage(float damage) {
		this.damage = damage;
		baseDamage = damage;
	}

	public void DeactivateProjectile() {
		speedProjectile = ProjectileData.GetFloat(projectileID, ProjectileParameter.Speed);
		buffEffects.Clear();
		ReturnToPool();
	}

	public void DebuffEffect(BuffID buff) {
		if (!IsBuffed(buff)) {
			switch (buff) {
				case BuffID.Buff001:
					speedProjectile /= BuffsData.GetFloat(buff, BuffParameter.Value);
					break;
				case BuffID.Buff002:
					damage /= BuffsData.GetFloat(buff, BuffParameter.Value);
					break;
			}
			Buff addBuff = new Buff();
			addBuff.AddBuff(buff, BuffsData.GetFloat(buff, BuffParameter.Value), BuffsData.GetFloat(buff, BuffParameter.Duration));
			buffEffects.Add(addBuff);
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
				}
				else {
					switch (buffEffects[i].buffID) {
						case BuffID.Buff001:
							speedProjectile = ProjectileData.GetFloat(projectileID, ProjectileParameter.Speed);
							break;
						case BuffID.Buff002:
							damage = baseDamage;
							break;
					}
					buffEffects.RemoveAt(i);
				}
			}
		}

	}
}
