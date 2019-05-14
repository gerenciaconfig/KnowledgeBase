using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcolabs.Brushes
{

    public class Bucket : Brush
    {
        /// <summary>
        /// Color that will be flooded
        /// </summary>
        private Color targetColor;

        /// <summary>
        /// stack of possible flood pixels
        /// </summary>
        private List<Vector2> stack;

        /// <summary>
        /// Used to stop flood
        /// </summary>
        private Coroutine flood;

        /// <summary>
        /// Apply brush on Raycast hit target
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="hit"></param>
        /// <param name="applyanceColor"></param>
        /// <param name="brushApplyier"></param>
        public override void ApplyOn(Sprite sprite, Vector2 offset, Color applyanceColor, BrushApplyier brushApplyier,bool firstTouch=false)
        {
            if (flood != null) StopCoroutine(flood);
            stack = new List<Vector2>();


            targetColor = sprite.texture.GetPixel((int)offset.x, (int)offset.y);
            stack.Add(offset);

            bool ignore = false;
            foreach (Color ignoreColor in brushApplyier.ignoreColors)
            {
                if (Vector4.Distance((Vector4)ignoreColor, (Vector4)targetColor) < brushApplyier.ColorSensitivity)
                {
                    ignore = true;
                    continue;
                }
            }
            if (ignore) return;

            flood = StartCoroutine(FloodFill(sprite, applyanceColor));
        }

        /// <summary>
        /// Flood Fil algorithm for more info check : https://en.wikipedia.org/wiki/Flood_fill
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="applyanceColor"></param>
        /// <returns></returns>
        private IEnumerator FloodFill(Sprite sprite, Color applyanceColor)
        {
            yield return null;
            while (stack.Count > 0)
            {
                Vector2 current = stack[0];
                stack.RemoveAt(0);

                if (sprite.texture.GetPixel((int)current.x, (int)current.y) == applyanceColor) continue;
                if (sprite.texture.GetPixel((int)current.x, (int)current.y) != targetColor) continue;

                sprite.texture.SetPixel((int)current.x, (int)current.y, applyanceColor);

                stack.Add(current + new Vector2(-1, 0));

                stack.Add(current + new Vector2(1, 0));

                stack.Add(current + new Vector2(0, 1));

                stack.Add(current + new Vector2(0, -1));

            }
            sprite.texture.Apply();
        }
    }
}