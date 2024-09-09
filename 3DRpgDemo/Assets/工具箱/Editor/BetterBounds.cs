#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FACS01.Utilities
{
    public class BetterBounds : EditorWindow
    {
		[MenuItem("工具箱/陌颜汉化", false, 2001)]
        public static void hanhua()
        {
            EditorUtility.DisplayDialog("提示", "改模交流群：859928057\n禁止售卖此脚本", "Okay");
        }
        public GameObject source;

        private static FACSGUIStyles FacsGUIStyles;

        [MenuItem("工具箱/模型消失修复", false, 1000)]
        public static void ShowWindow()
        {
            GetWindow(typeof(BetterBounds), false, "界限优化器", true);
        }
        public void OnGUI()
        {
            if (FacsGUIStyles == null) { FacsGUIStyles = new FACSGUIStyles(); }
            FacsGUIStyles.helpbox.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.LabelField($"<color=red><b>界限优化器</b></color>\n\n将扫描该模型下所有的蒙皮网格, 重新计算并优化界限数值大小.\n\n优化之后理论上模型将不会再出现某些角度看过去会消失的问题.\n", FacsGUIStyles.helpbox);

            source = (GameObject)EditorGUILayout.ObjectField(source, typeof(GameObject), true, GUILayout.Height(40));

            if (GUILayout.Button("一键优化!", FacsGUIStyles.button, GUILayout.Height(40)))
            {
                if (source != null)
                {
                    Debug.Log("开始优化");
                    RunFix();
                    Debug.Log("优化完成");
                }
                else
                {
                    ShowNotification(new GUIContent("未选择任何模型"));
                }
            }
        }
        private void RunFix()
        {
            Transform rootT = source.transform;
            Bounds finalBounds = new Bounds();
            List<SkinnedMeshRenderer> SMRList = new List<SkinnedMeshRenderer>();
            foreach (Transform childT in rootT)
            {
                SkinnedMeshRenderer childSMR = childT.GetComponent<SkinnedMeshRenderer>();
                if (childSMR != null) SMRList.Add(childSMR);
            }
            int childSMRCount = SMRList.Count;
            if (childSMRCount == 0)
            {
                Debug.LogError($"未找到任何蒙皮网格 {rootT.name}");
                return;
            }
            finalBounds = SMRList[0].localBounds;
            bool allBoundsEqual = false;
            if (childSMRCount > 1)
            {
                allBoundsEqual = true;
                for (int i = 1; i < childSMRCount; i++)
                {
                    if (allBoundsEqual && finalBounds != SMRList[i].localBounds)
                    {
                        allBoundsEqual = false;
                    }
                    finalBounds.Encapsulate(SMRList[i].localBounds);
                }
            }
            if (allBoundsEqual)
            {
                EditorUtility.DisplayDialog("Warning", "先手动增加或减少模型中任何网格的bound大小然后再点击一键优化.", "Okay");
                Debug.LogWarning("All bounding boxes are the same size. Did you use the fix already? If not, just edit any of the bounds a bit and try again.");
            }
            else
            {
                Vector3 Center = finalBounds.center;
                Vector3 Extents = finalBounds.extents;
                float dx = Extents.x; float dy = Extents.y; float dz = Extents.z;

                Vector3 tmpMax = Center + new Vector3 { x = dy, y = dx, z = dz };
                Vector3 tmpMin = Center - new Vector3 { x = dy, y = dx, z = dz };
                finalBounds.Encapsulate(tmpMax); finalBounds.Encapsulate(tmpMin);
                tmpMax = Center + new Vector3 { x = dz, y = dx, z = dy };
                tmpMin = Center - new Vector3 { x = dz, y = dx, z = dy };
                finalBounds.Encapsulate(tmpMax); finalBounds.Encapsulate(tmpMin);

                Center = finalBounds.center;
                Extents = finalBounds.extents;
                dx = Extents.x; dy = Extents.y; dz = Extents.z;
                tmpMax = Center + new Vector3 { x = dx, y = 1.5f * dy, z = dz };
                finalBounds.Encapsulate(tmpMax);
                foreach (SkinnedMeshRenderer childSMR in SMRList)
                {
                    Undo.RegisterCompleteObjectUndo(childSMR, "Better Bounds");
                    childSMR.localBounds = finalBounds;
                }
                Debug.Log($"Better Bounds applied to {childSMRCount} Skinned Mesh Renderer{(childSMRCount>1?"s":"")}!");
            }

        }
        void OnDestroy()
        {
            source = null;
        }
    }
}
#endif