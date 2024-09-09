using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace URPShadersGUI
{
    public class GalaxyShaderGUI : URPShaderGuiBase
    {
        public override void AssignProperties()
        {
            _propertiesSection.Clear();

            PropertiesSection MainTextures = new PropertiesSection();
            MainTextures.SectionName = "Main Textures";

            MainTextures.PropertiesList.Add(GetProperty("_MainTexture"));
            MainTextures.PropertiesList.Add(GetProperty("_MainTextureRotationSpeed"));
            MainTextures.PropertiesList.Add(GetProperty("_SecondTexture"));
            MainTextures.PropertiesList.Add(GetProperty("_SecondTextureRotationSpeed"));
            MainTextures.PropertiesList.Add(GetProperty("_SecondTextureIntesity"));
            _propertiesSection.Add(MainTextures);

            PropertiesSection Blending = new PropertiesSection();
            Blending.SectionName = "Blending";

            Blending.PropertiesList.Add(GetProperty("_BlendOpacity"));
            Blending.PropertiesList.Add(GetProperty("_BlendRange"));
            Blending.PropertiesList.Add(GetProperty("_BlendFuzziness"));
            Blending.PropertiesList.Add(GetProperty("_BlendClampValue"));
            _propertiesSection.Add(Blending);

            PropertiesSection General = new PropertiesSection();
            General.SectionName = "General";

            General.PropertiesList.Add(GetProperty("_NormalAffectStrength"));
            General.PropertiesList.Add(GetProperty("_RimColor"));
            General.PropertiesList.Add(GetProperty("_RimNoiseScale"));
            General.PropertiesList.Add(GetProperty("_RimPower"));
            General.PropertiesList.Add(GetProperty("_RimNoiseSpeed"));
            General.PropertiesList.Add(GetProperty("_Temperature"));
            General.PropertiesList.Add(GetProperty("_Tint"));
            _propertiesSection.Add(General);


            PropertiesSectionEnum NoiseUsage = new PropertiesSectionEnum();
            NoiseUsage.EnumValues.Add("_USENOISETO_HIGHLIGHT");
            NoiseUsage.EnumValues.Add("_USENOISETO_ADD_COLOR");
            NoiseUsage.EnumValues.Add("_USENOISETO_NONE");

            NoiseUsage.SectionName = "Noise Usage";
            NoiseUsage.EnumName = "Use Noise To";

            List<MaterialProperty> higlightProperties = new List<MaterialProperty>();
            higlightProperties.Add(GetProperty("_HighlightIntensity"));
            NoiseUsage.PropertiesLists.Add(higlightProperties);

            List<MaterialProperty> colorAddProperties = new List<MaterialProperty>();
            colorAddProperties.Add(GetProperty("_NoiseColor"));
            NoiseUsage.PropertiesLists.Add(colorAddProperties);

            _propertiesSection.Add(NoiseUsage);


            PropertiesSectionKeyword Noise = new PropertiesSectionKeyword();
            Noise.SectionName = "Noise";

            Noise.KeywordName = "_USENOISETO_NONE";
            Noise.ShowKeyword = false;
            Noise.UseKeywordFromEnum = true;
            Noise.PropertiesSectionEnumName = "Noise Usage";
            Noise.EnumKeywordIndex = 2;

            Noise.PropertiesList.Add(GetProperty("_NoiseSpeed"));
            Noise.PropertiesList.Add(GetProperty("_NoiseScale"));
            Noise.PropertiesList.Add(GetProperty("_NoisePower"));
            Noise.PropertiesList.Add(GetProperty("_NoiseSubtract"));
            Noise.PropertiesList.Add(GetProperty("_NoiseClampValue"));
            _propertiesSection.Add(Noise);

            base.AssignProperties();
        }

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            base.OnGUI(materialEditor, props);
        }
    }
}