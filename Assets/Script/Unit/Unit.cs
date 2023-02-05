using UnityEngine;

public class Unit : PooledObject
{
	[HideInInspector] public float AttackDamage;
	protected float HPUnit;
	protected float SpeedMovement;
	protected float nextShoot = 0;
	protected float intervalAttack;

	private void Update() {
		AttackFire();
		UnitUpdate();
	}

	private void AttackFire() {
		nextShoot -= 1 * Time.deltaTime;
		if (nextShoot <= 0) {
			nextShoot = intervalAttack;
			SetProjectile();
		}
	}

	protected virtual void UnitUpdate() { }

	protected virtual void SetProjectile() { }

}