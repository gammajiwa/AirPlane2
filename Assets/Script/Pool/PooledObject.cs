using System;
using System.Collections;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
	public Action<PooledObject> ReturnToPoolEvent;

	private bool isNotInitialized = true;

	public bool IsNotInitialized() {
		if (isNotInitialized) {
			isNotInitialized = false;
			return true;
		}
		return false;
	}

	public void Reinitialize() {
		if (isNotInitialized) {
			StartCoroutine(WaitForInitialization());
		}
		else {
			OnSpawnedFromPool();
		}
	}

	private IEnumerator WaitForInitialization() {
		yield return new WaitUntil(() => !isNotInitialized);
		OnSpawnedFromPool();
	}

	protected virtual void OnSpawnedFromPool() { }

	protected void ReturnToPool() {
		ReturnToPoolEvent?.Invoke(this);
	}
}
