using UnityEngine;

public class Projectile004: BaseProjectile
{
	private void OnTriggerEnter2D(Collider2D collision) {
		Player player = collision.gameObject.GetComponent<Player>();
		if (!(player is null)) {
			player.PlayerGetDamage(damage);
			ReturnToPool();
		}
	}
}
