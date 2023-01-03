#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Linq;
using UnityEditor;

public class PlayerEditorWindow : OdinMenuEditorWindow    // Change ID enum, Scriptable Object enum, remove GetName function if you don't need Headers
{
    [MenuItem("Game Data Editor/Player", false, 20)]

    private static void OpenWindow() {
        var window = GetWindow<PlayerEditorWindow>(); // Change this to match the name of this class
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1250, 720);
	}

    protected override OdinMenuTree BuildMenuTree() {
        OdinMenuTree tree = new OdinMenuTree();
        tree.DefaultMenuStyle.IconSize = 28.00f;
        tree.Config.DrawSearchToolbar = true;

        foreach (PlayerID id in Enum.GetValues(typeof(PlayerID))) {
           PlayerScriptableObject scriptableObject = new PlayerScriptableObject();
            tree.Add(id.ToString(), scriptableObject);
        }

        tree.Selection.SelectionChanged += SelectionChanged;

        return tree;
    }

    private void SelectionChanged(SelectionChangedType obj) {
        OdinMenuItem selected = MenuTree.Selection.FirstOrDefault();
        if (selected == null) return;
        if (selected.Value is PlayerScriptableObject item) {
            PlayerID id;
            if (Enum.TryParse(selected.Name, out id)) {
				if (item.ID != id || item.ID == 0) {
					selected.Value = new PlayerScriptableObject(id);
				}
			}
        }
    }
}
#endif