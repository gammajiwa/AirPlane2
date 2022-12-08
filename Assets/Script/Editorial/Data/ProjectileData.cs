using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectileData    // Change ID enum, Status enum, and the File Path
{
	private static readonly Dictionary<ProjectileID, JSONNode> cachedStatus = new Dictionary<ProjectileID, JSONNode>();
	private static readonly Dictionary<ProjectileID, Dictionary<ProjectileParameter, JSONNode>> cachedNode = new Dictionary<ProjectileID, Dictionary<ProjectileParameter, JSONNode>>();
	private const string filePath = "Data/Projectile";

	private static JSONNode LoadData(ProjectileID identifier) {
		return JSON.Parse(Resources.Load<TextAsset>($"{filePath}/{identifier}").ToString());
	}

	private static JSONNode GetUnitData(ProjectileID identifier) {
		return cachedStatus.TryGetValue(identifier, out JSONNode data) ? data : (cachedStatus[identifier] = LoadData(identifier));
	}

	private static JSONNode Get(ProjectileID identifier, ProjectileParameter status) {
		if (!cachedNode.TryGetValue(identifier, out Dictionary<ProjectileParameter, JSONNode> data)) {
			data = cachedNode[identifier] = new Dictionary<ProjectileParameter, JSONNode>();
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

	public static string GetString(ProjectileID identifier, ProjectileParameter status) {
		return Get(identifier, status).Value;
	}

	public static float GetFloat(ProjectileID identifier, ProjectileParameter status) {
		return Get(identifier, status).AsFloat;
	}

	public static int GetInt(ProjectileID identifier, ProjectileParameter status) {
		return Get(identifier, status).AsInt;
	}

	public static int[] GetIntArray(ProjectileID identifier, ProjectileParameter status) {
		JSONArray arr = GetUnitData(identifier)[status.ToString()].AsArray;
		int count = arr.Count;
		int[] ints = new int[count];
		for (int i = 0; i < count; i++) {
			ints[i] = arr[i].AsInt;
		}
		return ints;
	}

	public static float[] GetFloatArray(ProjectileID identifier, ProjectileParameter status) {
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

	public static bool GetBool(ProjectileID identifier, ProjectileParameter status) {
		return Get(identifier, status).AsBool;
	}
}