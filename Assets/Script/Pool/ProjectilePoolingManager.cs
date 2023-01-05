using UnityEngine;
using System.Collections.Generic;
public class ProjectilePoolingManager : MonoBehaviour
{
	public Dictionary<ProjectileID, ProjectilePooling> projectiles = new Dictionary<ProjectileID, ProjectilePooling>();
	[SerializeField] private ProjectilePooling projectilePooling001;
	[SerializeField] private ProjectilePooling projectilePooling002;
	[SerializeField] private ProjectilePooling projectilePooling003;
	[SerializeField] private ProjectilePooling projectilePooling004;

	public void Initialize() {
		projectiles.Add(ProjectileID.Projectile001, projectilePooling001);
		projectiles.Add(ProjectileID.Projectile002, projectilePooling002);
		projectiles.Add(ProjectileID.Projectile003, projectilePooling003);
		projectiles.Add(ProjectileID.Projectile004, projectilePooling004);
		foreach (ProjectilePooling projectilePooling in projectiles.Values) {
			projectilePooling.Initialize();
		}
	}

	public BaseProjectile Spawn(ProjectileID projectileID, Vector3 position, Quaternion rotation) {
		return projectiles[projectileID].Spawn(position, rotation);
	}
}
