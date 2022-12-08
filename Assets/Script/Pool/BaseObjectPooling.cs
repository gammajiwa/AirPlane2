using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObjectPooling<T> : MonoBehaviour where T : PooledObject
{
	[SerializeField] private PooledObject prefab;
	[SerializeField] private int initialAmount = 50;
	private readonly Queue<T> typePool = new Queue<T>();

	public void Initialize() {
		for (int i = 0; i < initialAmount; i++) {
			CreateNewObject(Vector3.one * 100f, Quaternion.identity);
		}
	}

	protected T SpawnFromPool(Vector3 position, Quaternion rotation) {
		T selectedType;
		if (typePool.Count == 0) {
			CreateNewObject(position, rotation);
		}
		selectedType = typePool.Dequeue();
		selectedType.transform.position = position;
		selectedType.transform.rotation = rotation;
		selectedType.gameObject.SetActive(true);
		selectedType.Reinitialize();
		return selectedType;
	}

	protected void CreateNewObject(Vector3 position, Quaternion rotation) {
		PooledObject newObject = Instantiate(prefab, position, rotation, transform);
		newObject.ReturnToPoolEvent += ReturnToPool;
		ReturnToPool(newObject);
	}

	private void ReturnToPool(PooledObject returnedObject) {
		returnedObject.gameObject.SetActive(false);
		if (returnedObject is T type) {
			typePool.Enqueue(type);
			OnReturnToPool(type);
		}
	}

	protected virtual void OnReturnToPool(T type) { }
}

