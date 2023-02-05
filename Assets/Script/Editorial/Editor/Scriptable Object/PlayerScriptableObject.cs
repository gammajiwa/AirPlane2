#if UNITY_EDITOR
using SimpleJSON;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class PlayerScriptableObject : BaseScriptableObject    // Change ID enum, and Data class reference in the comment below
{
	[BoxGroup("General")]
	[HorizontalGroup("General/Split", 0.8f, LabelWidth = 135)]
	[VerticalGroup("General/Split/Left")] [ReadOnly] public PlayerID ID;
	[VerticalGroup("General/Split/Left")] [OnValueChanged("ValueChanged")] public float HP;
	[VerticalGroup("General/Split/Left")] [OnValueChanged("ValueChanged")] public float MoveSpeed;
	[VerticalGroup("General/Split/Left")] [OnValueChanged("ValueChanged")] public float Damage;
	[VerticalGroup("General/Split/Left")] [OnValueChanged("ValueChanged")] public float IntervalAttack;
	[VerticalGroup("General/Split/Left")] [OnValueChanged("ValueChanged")] public float SpecialMovementDuration ;

	private string filePath = PlayerData.GetFilePath();	// Change this to corresponding Data class

	public PlayerScriptableObject() { }

	public PlayerScriptableObject(PlayerID identifier) {
		ID = identifier;
		Load();
		Save();
	}

	public override void Load() {
		JSONNode data = JSON.Parse(Resources.Load<TextAsset>($"{filePath}/{ID}").ToString());

		GetFloat(data, "HP", out HP);
		GetFloat(data, "MoveSpeed", out MoveSpeed);
		GetFloat(data, "Damage", out Damage);
		GetFloat(data, "IntervalAttack", out IntervalAttack);
		GetFloat(data, "SpecialMovementDuration", out SpecialMovementDuration);

		AssetDatabase.Refresh();
		ValueSet();
	}

	public override void Save() {
		JSONObject json = new JSONObject();

		SetFloat(HP, "HP", ref json);
		SetFloat(MoveSpeed, "MoveSpeed", ref json);
		SetFloat(Damage, "Damage", ref json);
		SetFloat(IntervalAttack, "IntervalAttack", ref json);
		SetFloat(SpecialMovementDuration, "SpecialMovementDuration", ref json);
		SaveFile($"Assets/Resources/{filePath}", ID.ToString(), json.ToString());
		AssetDatabase.Refresh();
		ValueSet();
	}
}
#endif