using UnityEditor;
using UnityEngine;
using System;

namespace FACS01.Utilities
{
    public class RemoveMissingScripts : EditorWindow 
    {
        public GameObject source;
        private static FACSGUIStyles FacsGUIStyles;
        private static int GameObjectsScanned;
        private static int missingScriptsCount;
        private static string results;

        [MenuItem("工具箱/空脚本清除", false, 1004)]
        public static void ShowWindow2()
        {
            EditorWindow editorWindow = GetWindow(typeof(RemoveMissingScripts), false, "空脚本清除", true);
            editorWindow.autoRepaintOnSceneChange = true;
        }
        public void OnGUI()
        {
            if (FacsGUIStyles == null) { FacsGUIStyles = new FACSGUIStyles(); }
            FacsGUIStyles.helpbox.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.LabelField($"<color=cyan><b>空脚本清除</b></color>\n\n扫描被选择模型" +
                $" 并删除模型中所有无效的脚本\n" +
                $"如果模型是预制体将被展开.\n\n" +
                $".\n", FacsGUIStyles.helpbox);

            source = (GameObject)EditorGUILayout.ObjectField(source, typeof(GameObject), true, GUILayout.Height(40));

            if (GUILayout.Button("一键清除!", FacsGUIStyles.button, GUILayout.Height(40)))
            {
                if (source != null)
                {
                    FindInSelected(source);
                }
                else
                {
                    ShowNotification(new GUIContent("未选择模型"));
                    NullVars();
                }
            }
            if (results != null && results != "")
            {
                FacsGUIStyles.helpbox.alignment = TextAnchor.MiddleLeft;
                EditorGUILayout.LabelField(results, FacsGUIStyles.helpbox);
            }
        }
        
        private static void FindInSelected(GameObject src)
        {
            GameObjectsScanned = 0;
            missingScriptsCount = 0;
            results = "";

            FindInGo(src);

            if (missingScriptsCount > 0)
            {
                try
                {
                    PrefabUtility.UnpackPrefabInstance(src, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                }
                catch (ArgumentException) { }
            }

            GenerateResults();
        }
        private static void FindInGo(GameObject g)
        {
            GameObjectsScanned++;
         
            var tempCount = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(g);
			
			if (tempCount > 0) {
                Undo.RegisterCompleteObjectUndo(g, "Remove Empty Scripts");
                missingScriptsCount += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(g);
			}
            
            foreach (Transform childT in g.transform)
            {
                FindInGo(childT.gameObject);
            }
        }
        private static void GenerateResults()
        {
            results = $"Results:\n";
            results += $"   • <color=green>已扫描的子集:</color> {GameObjectsScanned}\n";
            results += $"   • <color=green>已清除的空脚本:</color> {missingScriptsCount}\n";
        }
        private void OnDestroy()
        {
            source = null;
            FacsGUIStyles = null;
            NullVars();
        }
        void NullVars()
        {
            results = null;
        }
    }
}
