using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace URPShadersGUI
{
    public class PropertiesSection
    {
        public bool Show = true;
        public string SectionName;
        public virtual bool UseKeyword { get; } = false;
        public List<MaterialProperty> PropertiesList = new List<MaterialProperty>();

    }

    public class PropertiesSectionKeyword : PropertiesSection
    {
        public string KeywordName;
        public override bool UseKeyword { get; } = false;
        public bool ShowKeyword = true;

        public bool UseKeywordFromEnum = false;
        public string PropertiesSectionEnumName;
        public int EnumKeywordIndex;

        public PropertiesSectionKeyword()
        {
            UseKeyword = true;
        }
    }

    public class PropertiesSectionEnum : PropertiesSection
    {
        public List<string> EnumValues = new List<string>();
        public string EnumName;

        public List<List<MaterialProperty>> PropertiesLists = new List<List<MaterialProperty>>();
    }


    public class URPShaderGuiBase : ShaderGUI
    {
        protected Material _material;
        protected MaterialProperty[] _props;
        protected MaterialEditor _materialEditor;

        protected List<string> _keywords;
        protected List<PropertiesSection> _propertiesSection = new List<PropertiesSection>();

        private Dictionary<string, int> keywordIndexesSave = new Dictionary<string, int>();
        private Dictionary<string, bool> propertiesFoldoutSave = new Dictionary<string, bool>();

        protected MaterialProperty GetProperty(string name)
        {
            return FindProperty(name, _props);
        }


        public virtual void AssignProperties()
        {
            foreach (var section in _propertiesSection)
            {
                if (!keywordIndexesSave.ContainsKey(section.SectionName))
                {
                    keywordIndexesSave.Add(section.SectionName, 0);
                }

                if (!propertiesFoldoutSave.ContainsKey(section.SectionName))
                {
                    propertiesFoldoutSave.Add(section.SectionName, true);
                }
            }

        }


        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            _material = materialEditor.target as Material;
            _props = props;
            _materialEditor = materialEditor;
            _keywords = new List<string>(_material.shaderKeywords);

            AssignProperties();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(-7);
            EditorGUILayout.BeginVertical();
            EditorGUI.BeginChangeCheck();
            DrawGUI();
            EditorGUILayout.EndVertical();
            GUILayout.Space(1);
            EditorGUILayout.EndHorizontal();

            Undo.RecordObject(_material, "Material Edition");
        }

        private void DrawGUI()
        {
            foreach (var section in _propertiesSection)
            {
                // propertiesFoldoutSave[section.SectionName] = EditorGUILayout.Foldout(propertiesFoldoutSave[section.SectionName], section.SectionName);
                propertiesFoldoutSave[section.SectionName] = EditorGUILayout.BeginFoldoutHeaderGroup(propertiesFoldoutSave[section.SectionName], section.SectionName);
                if (propertiesFoldoutSave[section.SectionName] && section.Show)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    DrawSection(section);
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }

        private void DrawSection(PropertiesSection section)
        {
            var midStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
            //EditorGUILayout.LabelField(section.SectionName, midStyle, GUILayout.ExpandWidth(true));

            if (!typeof(PropertiesSectionKeyword).IsSubclassOf(section.GetType()) && section.UseKeyword)
            {
                PropertiesSectionKeyword derivativeSection = (PropertiesSectionKeyword)section;
                if(derivativeSection.ShowKeyword) keywordIndexesSave[derivativeSection.SectionName] = GUILayout.Toggle((keywordIndexesSave[derivativeSection.SectionName] == 1) ? true : false, derivativeSection.KeywordName, EditorStyles.toggle) ? 1 : 0;

                if(derivativeSection.UseKeywordFromEnum)
                {
                    if (keywordIndexesSave[derivativeSection.PropertiesSectionEnumName] == derivativeSection.EnumKeywordIndex)
                    {
                        _material.EnableKeyword(derivativeSection.KeywordName);
                    }
                    else
                    {
                        EditorGUI.indentLevel++;
                        _materialEditor.SetDefaultGUIWidths();
                        foreach (var property in section.PropertiesList)
                        {
                            _materialEditor.ShaderProperty(property, property.name);
                        }
                        EditorGUI.indentLevel--;

                        _material.DisableKeyword(derivativeSection.KeywordName);
                    }
                }
                else
                {
                    if (keywordIndexesSave[derivativeSection.SectionName] == 1)
                    {
                        _material.EnableKeyword(derivativeSection.KeywordName);
                    }
                    else
                    {
                        EditorGUI.indentLevel++;
                        _materialEditor.SetDefaultGUIWidths();
                        foreach (var property in section.PropertiesList)
                        {
                            _materialEditor.ShaderProperty(property, property.name);
                        }
                        EditorGUI.indentLevel--;

                        _material.DisableKeyword(derivativeSection.KeywordName);
                    }
                }

              
            }
            else if (!typeof(PropertiesSectionEnum).IsSubclassOf(section.GetType()) && !section.UseKeyword)
            {
                PropertiesSectionEnum derivativeSection = (PropertiesSectionEnum)section;

                for(int i =0;i<_material.shaderKeywords.Length;i++)
                {
                    if(derivativeSection.EnumValues.Contains(_material.shaderKeywords[i]))
                    {
                        keywordIndexesSave[derivativeSection.SectionName] = derivativeSection.EnumValues.IndexOf(_material.shaderKeywords[i]);
                    }
                }

                keywordIndexesSave[derivativeSection.SectionName] = EditorGUILayout.Popup(keywordIndexesSave[derivativeSection.SectionName], derivativeSection.EnumValues.ToArray());

                for (int i = 0; i < derivativeSection.EnumValues.Count; i++)
                {
                    if (keywordIndexesSave[derivativeSection.SectionName] == i)
                    {
                        _material.EnableKeyword(derivativeSection.EnumValues[i]);

                        if (derivativeSection.PropertiesLists.Count > i)
                        {
                            foreach (var property in derivativeSection.PropertiesLists[i])
                            {
                                _materialEditor.ShaderProperty(property, property.name);
                            }
                        }
                    }
                    else _material.DisableKeyword(derivativeSection.EnumValues[i]);
                }
            }
            else
            {
                EditorGUI.indentLevel++;
                _materialEditor.SetDefaultGUIWidths();
                foreach (var property in section.PropertiesList)
                {
                    _materialEditor.ShaderProperty(property, property.name);
                }
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

        }
    }
}
