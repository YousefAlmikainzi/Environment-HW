#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System;


public class GUIInterfaceShader : ShaderGUI
{
    MaterialEditor editor;
    MaterialProperty[] properties;
    public override void OnGUI(MaterialEditor editor, MaterialProperty[] properties)
    {
        this.editor = editor;
        this.properties = properties;
        DoMain();
    }
    void DoMain()
    {
        //Main and Color
        GUILayout.Label("Main Maps", EditorStyles.boldLabel);
        MaterialProperty mainTex = FindProperty("_MainTex", properties);
        MaterialProperty color = FindProperty("_Color", properties);
        GUIContent albedoLabel = new GUIContent(mainTex.displayName);
        editor.TexturePropertySingleLine(albedoLabel, mainTex, color);
        editor.TextureScaleOffsetProperty(mainTex);

        MaterialProperty HeightModifier = FindProperty("_HeightModifier", properties);
        editor.ShaderProperty(HeightModifier, "Height Modifier");

        MaterialProperty windSpeedProp = FindProperty("_WindSpeed", properties);
        editor.ShaderProperty(windSpeedProp, "Wind Speed");

        MaterialProperty windStrengthProp = FindProperty("_WindStrength", properties);
        editor.ShaderProperty(windStrengthProp, "Wind Strength");

        MaterialProperty UVScale = FindProperty("_UVScale", properties);
        editor.ShaderProperty(UVScale, "UVScale");

        MaterialProperty UVScale2 = FindProperty("_UVScale2", properties);
        editor.ShaderProperty(UVScale2, "UVScale2");

        AdvancedOptions();
    }
    void AdvancedOptions()
    {
        GUILayout.Label("Advanced Options", EditorStyles.boldLabel);
        editor.EnableInstancingField();
    }
}
#endif