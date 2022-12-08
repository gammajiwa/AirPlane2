using System;
using UnityEngine;

public class BaseProjectile : PooledObject
{
	public Action<BaseProjectile> ProjectileDeactivateEvent;

	public Renderer projectileColor;
	public float damage;
	[SerializeField] protected private float speedProjectile;
	[SerializeField] private ProjectileID projectileID;

	private void Start() {
		speedProjectile = ProjectileData.GetFloat(projectileID, ProjectileParameter.Speed);
	}

	private void Update() {
		ProjectileMove();
	}

	protected virtual void ProjectileMove() {
		transform.Translate(Vector2.up * (speedProjectile * Time.deltaTime));
		if (transform.position.y >= 7 || transform.position.y <= -7 || transform.position.x <= -11 || transform.position.x >= 11) {
			DeactivateProjectile();
		}
	}

	public void Initialize(ProjectileID cachedUnitID) { }

	public void Reinitialize(float damage, Color color) {
		this.damage = damage;
		projectileColor.GetComponent<SpriteRenderer>().color = color;
	}

	public void DeactivateProjectile() {
		ReturnToPool();
		transform.position = new Vector2(0, -7);
		ProjectileDeactivateEvent?.Invoke(this);
	}
}
