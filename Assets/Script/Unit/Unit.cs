using UnityEngine;

public class Unit : PooledObject
{
	public float AttackDamage;
	[SerializeField] protected private float HPUnit;
	[SerializeField] protected private float SpeedMovement;
	[SerializeField] protected private float intervalAttack;
	protected private float nextShoot = 0;

	private void Start() {
		Initialized();
	}
	private void Update() {
		AttackFire();
		UnitUpdate();
	}

	protected virtual void Initialized() { }

	private void AttackFire() {
		nextShoot -= 1 * Time.deltaTime;
		if (nextShoot <= 0) {
			nextShoot = intervalAttack;
			SetProjectile();
		}
	}

	protected virtual void UnitUpdate() { }

	protected virtual void SetProjectile() { }

	public void Initialize(EnemyID cachedUnitID) { }

}