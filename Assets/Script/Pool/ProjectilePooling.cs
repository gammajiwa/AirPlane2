using UnityEngine;

public class ProjectilePooling : BaseObjectPooling<BaseProjectile>
{
	private ProjectileID cachedUnitID;
	public void Initialize(ProjectileID unitID) {
		cachedUnitID = unitID;
		Initialize();
	}

	public BaseProjectile Spawn(Vector3 position, Quaternion rotation, float damage, Color color) {
		BaseProjectile projectile = SpawnFromPool(position, rotation);
		projectile.Reinitialize(damage, color);
		return projectile;
	}
}
