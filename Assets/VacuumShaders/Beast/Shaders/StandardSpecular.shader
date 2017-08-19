Shader "VacuumShaders/Beast/Standard (Specular setup)"
{    
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}
		 
		_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
			     
		_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
		_GlossMapScale("Smoothness Factor", Range(0.0, 1.0)) = 1.0
		[Enum(Specular Alpha,0,Albedo Alpha,1)] _SmoothnessTextureChannel ("Smoothness texture channel", Float) = 0

		_SpecColor("Specular", Color) = (0.2,0.2,0.2)
		_SpecGlossMap("Specular", 2D) = "white" {}
		[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
		[ToggleOff] _GlossyReflections("Glossy Reflections", Float) = 1.0

		_BumpScale("Scale", Float) = 1.0 
		_BumpMap("Normal Map", 2D) = "bump" {}

		_Parallax ("Height Scale", Range (0.005, 0.08)) = 0.02
		_ParallaxMap ("Height Map", 2D) = "black" {} 

		_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		_OcclusionMap("Occlusion", 2D) = "white" {}

		_EmissionColor("Color", Color) = (0,0,0)
		_EmissionMap("Emission", 2D) = "white" {}
		
		_DetailMask("Detail Mask", 2D) = "white" {}

		_DetailAlbedoMap("Detail Albedo x2", 2D) = "grey" {}
		_DetailNormalMapScale("Scale", Float) = 1.0
		_DetailNormalMap("Normal Map", 2D) = "bump" {}

		[Enum(UV0,0,UV1,1)] _UVSec ("UV Set for secondary textures", Float) = 0


		// Blending state
		[HideInInspector] _Mode ("__mode", Float) = 0.0
		[HideInInspector] _SrcBlend ("__src", Float) = 1.0
		[HideInInspector] _DstBlend ("__dst", Float) = 0.0
		[HideInInspector] _ZWrite ("__zw", Float) = 1.0


		//Tessellation
		_V_WIRE_Tessellation_Type("", Float) = 0 
		_V_WIRE_Tessellation_Factor("", Range(1, 64)) = 4
		_V_WIRE_Tessellation_MinDistance("", float) = 10
		_V_WIRE_Tessellation_MaxDistance("", float) = 35
		_V_WIRE_Tessellation_EdgeLength("", Range(2, 64)) = 16
		_V_WIRE_TEssellation_Phong("", Range(0, 1)) = 0.5
		_V_WIRE_Tessellation_DispTex("", 2D) = "black" {}
		[Enum(UV0,0,UV1,1)] _V_WIRE_Tessellation_DispTex_UVSet("", Float) = 0
		[Enum(Red Channel,0,Alpha,1)] _V_WIRE_Tessellation_DispTex_Source("", Float) = 0
	    _V_WIRE_Tessellation_DispStrength("", float) = 0
		_V_WIRE_Tessellation_NormalCoef("", Float) = 1
		_V_WIRE_Tessellation_TangentCoef("", Float) = 1
		_V_WIRE_Tessellation_ShadowCasterLOD("", Range(0, 1)) = 1
		_V_WIRE_Tessellation_ImageEffectLOD("", Range(0, 1)) = 1
	}

	CGINCLUDE
		#define UNITY_SETUP_BRDF_INPUT SpecularSetup
	ENDCG

	SubShader
	{
		Tags { "RenderType"="Beast_Opaque" "PerformanceChecks"="False" }
		LOD 300
	

		// ------------------------------------------------------------------
		//  Base forward pass (directional light, emission, lightmaps, ...)
		Pass
		{
			Name "FORWARD" 
			Tags { "LightMode" = "ForwardBase" }

			Blend [_SrcBlend] [_DstBlend]
			ZWrite [_ZWrite]

			CGPROGRAM
			#include "CompilationTargetLevel.cginc" 

			// -------------------------------------

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature ___ _DETAIL_MULX2
			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
			#pragma shader_feature _ _SPECULARHIGHLIGHTS_OFF
			#pragma shader_feature _ _GLOSSYREFLECTIONS_OFF
			#pragma shader_feature _PARALLAXMAP

			#pragma multi_compile_fwdbase
			#pragma multi_compile_fog
			#pragma multi_compile_instancing

			#pragma vertex tessvert_surf       
			#pragma hull hs_surf
			#pragma domain ds_surf
			#pragma fragment fragBase
			#include "UnityStandardCoreForward.cginc"

			#define _TESSELLATION_FORWARDBASE
			#pragma shader_feature _ GENERATE_NORMALS GENERATE_TANGENTS
			#pragma shader_feature DEBUG_NONE DEBUG_NORMAL DEBUG_TANGENT
			#pragma shader_feature _TESSELLATION_FIXED _TESSELLATION_DISTANCE_BASED _TESSELLATION_EDGE_LENGTH _TESSELLATION_PHONG
			#pragma shader_feature _ AVERAGE_NORMALS
			#include "Assets/VacuumShaders/Beast/Shaders/Beast.cginc"

			ENDCG
		}
		// ------------------------------------------------------------------
		//  Additive forward pass (one light per pass)
		Pass
		{
			Name "FORWARD_DELTA"
			Tags { "LightMode" = "ForwardAdd" }
			Blend [_SrcBlend] One
			Fog { Color (0,0,0,0) } // in additive pass fog should be black
			ZWrite Off
			ZTest LEqual

			CGPROGRAM
			#include "CompilationTargetLevel.cginc"  

			// -------------------------------------

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
			#pragma shader_feature _ _SPECULARHIGHLIGHTS_OFF
			#pragma shader_feature ___ _DETAIL_MULX2
			#pragma shader_feature _PARALLAXMAP

			#pragma multi_compile_fwdadd_fullshadows
			#pragma multi_compile_fog

			#pragma vertex tessvert_surf       
			#pragma hull hs_surf
			#pragma domain ds_surf
			#pragma fragment fragAdd
			#include "UnityStandardCoreForward.cginc"

			#define _TESSELLATION_FORWARDADD
			#pragma shader_feature _ GENERATE_NORMALS GENERATE_TANGENTS
			#pragma shader_feature DEBUG_NONE DEBUG_NORMAL DEBUG_TANGENT
			#pragma shader_feature _TESSELLATION_FIXED _TESSELLATION_DISTANCE_BASED _TESSELLATION_EDGE_LENGTH _TESSELLATION_PHONG
			#pragma shader_feature _ AVERAGE_NORMALS
			#include "Assets/VacuumShaders/Beast/Shaders/Beast.cginc"

			ENDCG
		}
		// ------------------------------------------------------------------
		//  Shadow rendering pass
		Pass {
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }

			ZWrite On ZTest LEqual

			CGPROGRAM
			#include "CompilationTargetLevel.cginc" 

			// -------------------------------------


			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature _PARALLAXMAP
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_instancing

			#pragma vertex tessvert_surf       
			#pragma hull hs_surf
			#pragma domain ds_surf
			#pragma fragment fragShadowCaster
			#include "UnityStandardShadow.cginc"

			#define _TESSELLATION_SHADOWCASTER
			#pragma shader_feature _TESSELLATION_FIXED _TESSELLATION_DISTANCE_BASED _TESSELLATION_EDGE_LENGTH _TESSELLATION_PHONG
			#pragma shader_feature _ AVERAGE_NORMALS
			#include "Assets/VacuumShaders/Beast/Shaders/Beast.cginc"

			ENDCG
		}

		// ------------------------------------------------------------------
		//  Deferred pass
		Pass
		{
			Name "DEFERRED"
			Tags { "LightMode" = "Deferred" }

			CGPROGRAM
			#include "CompilationTargetLevel.cginc" 
			#pragma exclude_renderers nomrt


			// -------------------------------------

			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
			#pragma shader_feature _ _SPECULARHIGHLIGHTS_OFF
			#pragma shader_feature ___ _DETAIL_MULX2
			#pragma shader_feature _PARALLAXMAP

			#pragma multi_compile_prepassfinal
			#pragma multi_compile_instancing

			#pragma vertex tessvert_surf       
			#pragma hull hs_surf
			#pragma domain ds_surf
			#pragma fragment fragDeferred
			#include "UnityStandardCore.cginc"

			#define _TESSELLATION_DEFERRED
			#pragma shader_feature _ GENERATE_NORMALS GENERATE_TANGENTS
			#pragma shader_feature _TESSELLATION_FIXED _TESSELLATION_DISTANCE_BASED _TESSELLATION_EDGE_LENGTH _TESSELLATION_PHONG
			#pragma shader_feature _ AVERAGE_NORMALS
			#include "Assets/VacuumShaders/Beast/Shaders/Beast.cginc"

			ENDCG
		}

		// ------------------------------------------------------------------
		// Extracts information for lightmapping, GI (emission, albedo, ...)
		// This pass it not used during regular rendering.
		Pass
		{
			Name "META" 
			Tags { "LightMode"="Meta" }

			Cull Off

			CGPROGRAM
			#pragma vertex tessvert_surf       
			#pragma hull hs_surf
			#pragma domain ds_surf
			#pragma fragment frag_meta

			#pragma shader_feature _EMISSION
			#pragma shader_feature _SPECGLOSSMAP
			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
			#pragma shader_feature ___ _DETAIL_MULX2
			#pragma shader_feature EDITOR_VISUALIZATION

			#include "UnityStandardMeta.cginc"

			#define _TESSELLATION_META
			#pragma shader_feature _TESSELLATION_FIXED _TESSELLATION_DISTANCE_BASED _TESSELLATION_EDGE_LENGTH _TESSELLATION_PHONG
			#pragma shader_feature _ AVERAGE_NORMALS
			#include "Assets/VacuumShaders/Beast/Shaders/Beast.cginc"
			ENDCG
		}
	}


	FallBack "Standard (Specular setup)"
	CustomEditor "VacuumShaders.BeastTessellationUltraShaderGUI"
}
