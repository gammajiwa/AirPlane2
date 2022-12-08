#if UNITY_EDITOR
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class ProjectileScriptableObject : BaseScriptableObject    // Change ID enum, and Data class reference in the comment below
{
	[BoxGroup("General")]
	[HorizontalGroup("General/Split", 0.8f, LabelWidth = 135)]
	[VerticalGroup("General/Split/Left")] [ReadOnly] public ProjectileID ID;
	[VerticalGroup("General/Split/Left")] [OnValueChanged("ValueChanged")] public float Speed;

	private string filePath = ProjectileData.GetFilePath();  // Change this to corresponding Data class

	public ProjectileScriptableObject() { }

	public ProjectileScriptableObject(ProjectileID identifier) {
		ID = identifier;
		Load();
		Save();
	}

	public override void Load() {
		JSONNode data = JSON.Parse(Resources.Load<TextAsset>($"{filePath}/{ID}").ToString());
		GetFloat(data, "Speed", out Speed);
		AssetDatabase.Refresh();
		ValueSet();
	}

	public override void Save() {
		JSONObject json = new JSONObject();
		SetFloat(Speed, "Speed", ref json);
		SaveFile($"Assets/Resources/{filePath}", ID.ToString(), json.ToString());
		AssetDatabase.Refresh();
		ValueSet();
	}
}
#endif