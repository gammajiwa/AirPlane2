using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyData    // Change ID enum, Status enum, and the File Path
{
	private static readonly Dictionary<EnemyID, JSONNode> cachedStatus = new Dictionary<EnemyID, JSONNode>();
	private static readonly Dictionary<EnemyID, Dictionary<EnemyParameter, JSONNode>> cachedNode = new Dictionary<EnemyID, Dictionary<EnemyParameter, JSONNode>>();
	private const string filePath = "Data/Enemy";

	private static JSONNode LoadData(EnemyID identifier) {
		return JSON.Parse(Resources.Load<TextAsset>($"{filePath}/{identifier}").ToString());
	}

	private static JSONNode GetUnitData(EnemyID identifier) {
		return cachedStatus.TryGetValue(identifier, out JSONNode data) ? data : (cachedStatus[identifier] = LoadData(identifier));
	}

	private static JSONNode Get(EnemyID identifier, EnemyParameter status) {
		if (!cachedNode.TryGetValue(identifier, out Dictionary<EnemyParameter, JSONNode> data)) {
			data = cachedNode[identifier] = new Dictionary<EnemyParameter, JSONNode>();
		}
		if (!data.TryGetValue(status, out JSONNode parameter)) {
			parameter = data[status] = GetUnitData(identifier)[status.ToString()];
		}
		if (parameter == null) {
			parameter = JSONNull.CreateOrGet();
		}

		return parameter;
	}

	public static string GetFilePath() { return filePath; }

	public static string GetString(EnemyID identifier, EnemyParameter status) {
		return Get(identifier, status).Value;
	}

	public static float GetFloat(EnemyID identifier, EnemyParameter status) {
		return Get(identifier, status).AsFloat;
	}

	public static int GetInt(EnemyID identifier, EnemyParameter status) {
		return Get(identifier, status).AsInt;
	}

	public static int[] GetIntArray(EnemyID identifier, EnemyParameter status) {
		JSONArray arr = GetUnitData(identifier)[status.ToString()].AsArray;
		int count = arr.Count;
		int[] ints = new int[count];
		for (int i = 0; i < count; i++) {
			ints[i] = arr[i].AsInt;
		}
		return ints;
	}

	public static float[] GetFloatArray(EnemyID identifier, EnemyParameter status) {
		if (GetUnitData(identifier)[status.ToString()].Count == 0) {
			float[] floats = new float[1];
			floats[0] = GetUnitData(identifier)[status.ToString()];
			return floats;
		}
		else {
			JSONArray arr = GetUnitData(identifier)[status.ToString()].AsArray;
			int count = arr.Count;
			float[] floats = new float[count];
			for (int i = 0; i < count; i++) {
				floats[i] = arr[i].AsFloat;
			}
			return floats;
		}
	}

	public static bool GetBool(EnemyID identifier, EnemyParameter status) {
		return Get(identifier, status).AsBool;
	}
}