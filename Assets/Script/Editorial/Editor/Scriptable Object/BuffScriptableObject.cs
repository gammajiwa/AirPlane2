#if UNITY_EDITOR
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class BuffScriptableObject : BaseScriptableObject    // Change ID enum, and Data class reference in the comment below
{
	[BoxGroup("General")]
	[HorizontalGroup("General/Split", 0.8f, LabelWidth = 135)]
	[VerticalGroup("General/Split/Left")] [ReadOnly] public BuffID ID;
	[VerticalGroup("General/Split/Left")] [OnValueChanged("ValueChanged")] public float Value;

	private string filePath = BuffsData.GetFilePath();  // Change this to corresponding Data class

	public BuffScriptableObject() { }

	public BuffScriptableObject(BuffID identifier) {
		ID = identifier;
		Load();
		Save();
	}

	public override void Load() {
		JSONNode jSONNode = JSON.Parse(Resources.Load<TextAsset>($"{filePath}/{ID}").ToString());
		JSONNode data = jSONNode;

		GetFloat(data, "Value", out Value);

		AssetDatabase.Refresh();
		ValueSet();
	}

	public override void Save() {
		JSONObject json = new JSONObject();

		SetFloat(Value, "Value", ref json);
		SaveFile($"Assets/Resources/{filePath}", ID.ToString(), json.ToString());
		AssetDatabase.Refresh();
		ValueSet();
	}
}
#endif