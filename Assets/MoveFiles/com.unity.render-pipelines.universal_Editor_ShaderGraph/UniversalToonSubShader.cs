using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Graphing;
using UnityEditor.ShaderGraph;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UnityEditor.Rendering.Universal
{
    [Serializable]
    [FormerName("UnityEditor.Experimental.Rendering.LightweightPipeline.LightWeightToonSubShader")]
    [FormerName("UnityEditor.ShaderGraph.LightWeightToonSubShader")]
    [FormerName("UnityEditor.Rendering.LWRP.LightWeightToonSubShader")]
    class UniversalToonSubShader : IToonSubShader
    {
        Pass m_ForwardPassSpecular = new Pass()
        {
            Name = "UniversalForward",
            TemplatePath = "universalToonForwardPass.template",
            PixelShaderSlots = new List<int>()
            {
                ToonMasterNode.AlbedoSlotId,
                ToonMasterNode.NormalSlotId,
                ToonMasterNode.EmissionSlotId,
                ToonMasterNode.SpecularSlotId,
                ToonMasterNode.SmoothnessSlotId,
                ToonMasterNode.OcclusionSlotId,
                ToonMasterNode.AlphaSlotId,
                ToonMasterNode.AlphaThresholdSlotId,
                ToonMasterNode.ShadeSlotId,
                ToonMasterNode.ShadeShiftSlotId,
                ToonMasterNode.ShadeToonySlotId,
                ToonMasterNode.ToonyLightingSlotId,
                ToonMasterNode.SphereAddSlotId,
                ToonMasterNode.OutlineColorSlotId,
            },
            VertexShaderSlots = new List<int>()
            {
                ToonMasterNode.PositionSlotId,
                ToonMasterNode.OutlineWidthSlotId
            },
            Requirements = new ShaderGraphRequirements()
            {
                requiresNormal = UniversalSubShaderUtilities.k_PixelCoordinateSpace,
                requiresTangent = UniversalSubShaderUtilities.k_PixelCoordinateSpace,
                requiresBitangent = UniversalSubShaderUtilities.k_PixelCoordinateSpace,
                requiresPosition = UniversalSubShaderUtilities.k_PixelCoordinateSpace,
                requiresViewDir = UniversalSubShaderUtilities.k_PixelCoordinateSpace,
                requiresMeshUVs = new List<UVChannel>() { UVChannel.UV1 },
            },
            ExtraDefines = new List<string>(),
            OnGeneratePassImpl = (IMasterNode node, ref Pass pass, ref ShaderGraphRequirements requirements) =>
            {
                var masterNode = node as ToonMasterNode;

                pass.ExtraDefines.Add("#define _SPECULAR_SETUP 1");
                if (masterNode.IsSlotConnected(ToonMasterNode.NormalSlotId))
                    pass.ExtraDefines.Add("#define _NORMALMAP 1");
                if (masterNode.IsSlotConnected(ToonMasterNode.AlphaThresholdSlotId))
                    pass.ExtraDefines.Add("#define _AlphaClip 1");
                if (masterNode.surfaceType == SurfaceType.Transparent && masterNode.alphaMode == AlphaMode.Premultiply)
                    pass.ExtraDefines.Add("#define _ALPHAPREMULTIPLY_ON 1");
                if (requirements.requiresDepthTexture)
                    pass.ExtraDefines.Add("#define REQUIRE_DEPTH_TEXTURE");
                if (requirements.requiresCameraOpaqueTexture)
                    pass.ExtraDefines.Add("#define REQUIRE_OPAQUE_TEXTURE");
            }
        };

        Pass m_DepthShadowPass = new Pass()
        {
            Name = "",
            TemplatePath = "universalToonExtraPasses.template",
            PixelShaderSlots = new List<int>()
            {
                ToonMasterNode.AlbedoSlotId,
                ToonMasterNode.EmissionSlotId,
                ToonMasterNode.AlphaSlotId,
                ToonMasterNode.AlphaThresholdSlotId
            },
            VertexShaderSlots = new List<int>()
            {
                ToonMasterNode.PositionSlotId
            },
            Requirements = new ShaderGraphRequirements()
            {
                requiresNormal = UniversalSubShaderUtilities.k_PixelCoordinateSpace,
                requiresTangent = UniversalSubShaderUtilities.k_PixelCoordinateSpace,
                requiresBitangent = UniversalSubShaderUtilities.k_PixelCoordinateSpace,
                requiresPosition = UniversalSubShaderUtilities.k_PixelCoordinateSpace,
                requiresViewDir = UniversalSubShaderUtilities.k_PixelCoordinateSpace,
                requiresMeshUVs = new List<UVChannel>() { UVChannel.UV1 },
            },
            ExtraDefines = new List<string>(),
            OnGeneratePassImpl = (IMasterNode node, ref Pass pass, ref ShaderGraphRequirements requirements) =>
            {
                var masterNode = node as ToonMasterNode;

                if (masterNode.model == ToonMasterNode.Model.Specular)
                    pass.ExtraDefines.Add("#define _SPECULAR_SETUP 1");
                if (masterNode.IsSlotConnected(ToonMasterNode.AlphaThresholdSlotId))
                    pass.ExtraDefines.Add("#define _AlphaClip 1");
                if (masterNode.surfaceType == SurfaceType.Transparent && masterNode.alphaMode == AlphaMode.Premultiply)
                    pass.ExtraDefines.Add("#define _ALPHAPREMULTIPLY_ON 1");
                if (requirements.requiresDepthTexture)
                    pass.ExtraDefines.Add("#define REQUIRE_DEPTH_TEXTURE");
                if (requirements.requiresCameraOpaqueTexture)
                    pass.ExtraDefines.Add("#define REQUIRE_OPAQUE_TEXTURE");
            }
        };

        public int GetPreviewPassIndex() { return 0; }

        public string GetSubshader(IMasterNode masterNode, GenerationMode mode, List<string> sourceAssetDependencyPaths = null)
        {
            if (sourceAssetDependencyPaths != null)
            {
                // UniversalToonSubShader.cs
                sourceAssetDependencyPaths.Add(AssetDatabase.GUIDToAssetPath("ca91dbeb78daa054c9bbe15fef76361c"));
            }

            // Master Node data
            var ToonMasterNode = masterNode as ToonMasterNode;
            var tags = ShaderGenerator.BuildMaterialTags(ToonMasterNode.surfaceType);
            var options = ShaderGenerator.GetMaterialOptions(ToonMasterNode.surfaceType, ToonMasterNode.alphaMode, ToonMasterNode.twoSided.isOn);

            // Passes
            var passes = new Pass[] { m_ForwardPassSpecular, m_DepthShadowPass };

            return UniversalSubShaderUtilities.GetSubShader<ToonMasterNode>(ToonMasterNode, tags, options,
                passes, mode, "UnityEditor.ShaderGraph.ToonMasterGUI", sourceAssetDependencyPaths);
        }

        public bool IsPipelineCompatible(RenderPipelineAsset renderPipelineAsset)
        {
            return renderPipelineAsset is UniversalRenderPipelineAsset;
        }

        public UniversalToonSubShader() { }
    }
}
