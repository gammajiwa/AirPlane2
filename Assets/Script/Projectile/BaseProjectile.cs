using System;
using UnityEngine;
using System.Collections.Generic;

public class BaseProjectile : PooledObject
{
	public delegate void DebuffProjectile(BaseProjectile projectile, BuffID buffID);
	public DebuffProjectile DebuffProjectileEvent;
	public Action<BaseProjectile> DestructibleEvent;

	public ProjectileType projectileType;
	public Renderer projectileColor;
	[HideInInspector] public float damage;
	protected private float speedProjectile;
	protected private List<Buff> buffEffects = new List<Buff>();
	private float BaseSpeed;

	private void Update() {
		ProjectileMove();
	}

	protected virtual void ProjectileMove() {
		transform.Translate(Vector2.up * (speedProjectile * Time.deltaTime));
		if (transform.position.y >= 7 || transform.position.y <= -7 || transform.position.x <= -11 || transform.position.x >= 11) {
			DeactivateProjectile();
		}
	}

	public void Initialize(ProjectileID cachedUnitID) {
		BaseSpeed = ProjectileData.GetFloat(cachedUnitID, ProjectileParameter.Speed);
		speedProjectile = BaseSpeed;
	}

	public void Reinitialize(float damage, Color color, ProjectileType type) {
		this.damage = damage;
		speedProjectile = BaseSpeed;
		projectileColor.GetComponent<SpriteRenderer>().color = color;
		projectileType = type;
		buffEffects.Clear();
	}

	public void DeactivateProjectile() {
		ReturnToPool();
	}

	public void DebuffEffect(Buff buff, float value) {
		switch (buff.buffID) {
			case BuffID.Buff001:
				if (!IsBuffed(buff.buffID)) {
					speedProjectile /= value;
					buffEffects.Add(buff);
				}
				break;
			case BuffID.Buff002:
				if (!IsBuffed(buff.buffID)) {
					damage /= value;
					buffEffects.Add(buff);
				}
				break;
		}
	}

	private bool IsBuffed(BuffID buffID) {
		for (int i = 0; i < buffEffects.Count; i++) {
			if (buffEffects[i].buffID == buffID)
				return true;
		}
		return false;
	}
}
