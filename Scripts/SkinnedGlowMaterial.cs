﻿using System.Collections.Generic;
using UnityEngine;

namespace SpriteGlow
{
    public class SkinnedGlowMaterial : Material
    {
        public Texture SpriteTexture { get { return mainTexture; } }
        public bool DrawOutside { get { return IsKeywordEnabled(outsideMaterialKeyword); } }
        public bool InstancingEnabled { get { return enableInstancing; } }

        private const string outlineShaderName = "Sprites/Outline";
        private const string outsideMaterialKeyword = "SPRITE_OUTLINE_OUTSIDE";
        private static readonly Shader outlineShader = Shader.Find(outlineShaderName);

        private static List<SkinnedGlowMaterial> sharedMaterials = new List<SkinnedGlowMaterial>();

        public SkinnedGlowMaterial (//Texture spriteTexture, 
            bool drawOutside = false, bool instancingEnabled = false)
            : base(outlineShader)
        {
            if (!outlineShader) Debug.LogError(string.Format("{0} shader not found. Make sure the shader is included to the build.", outlineShaderName));

            //mainTexture = spriteTexture;
            if (drawOutside) EnableKeyword(outsideMaterialKeyword);
            if (instancingEnabled) enableInstancing = true;
        }

        public static Material GetSharedFor (SkinnedGlowEffect spriteGlow)
        {
            for (int i = 0; i < sharedMaterials.Count; i++)
            {
                if (//sharedMaterials[i].SpriteTexture == spriteGlow.Renderer.sprite.texture &&
                    sharedMaterials[i].DrawOutside == spriteGlow.DrawOutside &&
                    sharedMaterials[i].InstancingEnabled == spriteGlow.EnableInstancing)
                    return sharedMaterials[i];
            }

            var material = new SkinnedGlowMaterial(//spriteGlow.Renderer.sprite.texture, 
                spriteGlow.DrawOutside, spriteGlow.EnableInstancing);
            material.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor | HideFlags.NotEditable;
            sharedMaterials.Add(material);

            return material;
        }
    }
}
