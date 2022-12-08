using UnityEngine;
using System.Collections.Generic;
public class ProjectilePoolingManager : MonoBehaviour
{
	public Dictionary<ProjectileID, ProjectilePooling> projectiles = new Dictionary<ProjectileID, ProjectilePooling>();
	[SerializeField] private ProjectilePooling projectilePooling001;
	[SerializeField] private ProjectilePooling projectilePooling002;

	public void Initialize() {
		projectiles.Add(ProjectileID.Projectile001, projectilePooling001);
		projectiles.Add(ProjectileID.Projectile002, projectilePooling002);
		foreach (ProjectilePooling projectilePooling in projectiles.Values) {
			projectilePooling.Initialize();
		}
	}

	public BaseProjectile Spawn(ProjectileID projectileID, Vector3 position, Quaternion rotation, float damage, Color color) {
		return projectiles[projectileID].Spawn(position, rotation, damage, color);
	}
}
