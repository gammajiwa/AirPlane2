using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData    // Change ID enum, Status enum, and the File Path
{
	private static readonly Dictionary<PlayerID, JSONNode> cachedStatus = new Dictionary<PlayerID, JSONNode>();
	private static readonly Dictionary<PlayerID, Dictionary<PlayerParameter, JSONNode>> cachedNode = new Dictionary<PlayerID, Dictionary<PlayerParameter, JSONNode>>();
	private const string filePath = "Data/PlayerID";

	private static JSONNode LoadData(PlayerID identifier) {
		return JSON.Parse(Resources.Load<TextAsset>($"{filePath}/{identifier}").ToString());
	}

	private static JSONNode GetUnitData(PlayerID identifier) {
		return cachedStatus.TryGetValue(identifier, out JSONNode data) ? data : (cachedStatus[identifier] = LoadData(identifier));
	}

	private static JSONNode Get(PlayerID identifier, PlayerParameter status) {
		if (!cachedNode.TryGetValue(identifier, out Dictionary<PlayerParameter, JSONNode> data)) {
			data = cachedNode[identifier] = new Dictionary<PlayerParameter, JSONNode>();
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

	public static string GetString(PlayerID identifier, PlayerParameter status) {
		return Get(identifier, status).Value;
	}

	public static float GetFloat(PlayerID identifier, PlayerParameter status) {
		return Get(identifier, status).AsFloat;
	}

	public static int GetInt(PlayerID identifier, PlayerParameter status) {
		return Get(identifier, status).AsInt;
	}

	public static int[] GetIntArray(PlayerID identifier, PlayerParameter status) {
		JSONArray arr = GetUnitData(identifier)[status.ToString()].AsArray;
		int count = arr.Count;
		int[] ints = new int[count];
		for (int i = 0; i < count; i++) {
			ints[i] = arr[i].AsInt;
		}
		return ints;
	}

	public static float[] GetFloatArray(PlayerID identifier, PlayerParameter status) {
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

	public static bool GetBool(PlayerID identifier, PlayerParameter status) {
		return Get(identifier, status).AsBool;
	}
}