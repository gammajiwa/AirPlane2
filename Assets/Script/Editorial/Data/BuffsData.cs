using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

public static class BuffsData    // Change ID enum, Status enum, and the File Path
{
	private static readonly Dictionary<BuffID, JSONNode> cachedStatus = new Dictionary<BuffID, JSONNode>();
	private static readonly Dictionary<BuffID, Dictionary<BuffParameter, JSONNode>> cachedNode = new Dictionary<BuffID, Dictionary<BuffParameter, JSONNode>>();
	private const string filePath = "Data/Buffs";

	private static JSONNode LoadData(BuffID identifier) {
		return JSON.Parse(Resources.Load<TextAsset>($"{filePath}/{identifier}").ToString());
	}

	private static JSONNode GetUnitData(BuffID identifier) {
		return cachedStatus.TryGetValue(identifier, out JSONNode data) ? data : (cachedStatus[identifier] = LoadData(identifier));
	}

	private static JSONNode Get(BuffID identifier, BuffParameter status) {
		if (!cachedNode.TryGetValue(identifier, out Dictionary<BuffParameter, JSONNode> data)) {
			data = cachedNode[identifier] = new Dictionary<BuffParameter, JSONNode>();
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

	public static string GetString(BuffID identifier, BuffParameter status) {
		return Get(identifier, status).Value;
	}

	public static float GetFloat(BuffID identifier, BuffParameter status) {
		return Get(identifier, status).AsFloat;
	}

	public static int GetInt(BuffID identifier, BuffParameter status) {
		return Get(identifier, status).AsInt;
	}

	public static int[] GetIntArray(BuffID identifier, BuffParameter status) {
		JSONArray arr = GetUnitData(identifier)[status.ToString()].AsArray;
		int count = arr.Count;
		int[] ints = new int[count];
		for (int i = 0; i < count; i++) {
			ints[i] = arr[i].AsInt;
		}
		return ints;
	}

	public static float[] GetFloatArray(BuffID identifier, BuffParameter status) {
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

	public static bool GetBool(BuffID identifier, BuffParameter status) {
		return Get(identifier, status).AsBool;
	}
}