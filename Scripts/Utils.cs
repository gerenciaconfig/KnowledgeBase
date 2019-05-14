using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using System.Text;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Mkey
{
    public class Utils
    {
        public static void Measure(Action measProc)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();//https://msdn.microsoft.com/ru-ru/library/system.diagnostics.stopwatch%28v=vs.110%29.aspx
            stopWatch.Start();
            if (measProc != null) { measProc(); }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            UnityEngine.Debug.Log(" elapsed time: " + elapsedTime);
        }

        public static List<T>[] SplitListToArrays<T>(List<T> list, int count)
        {
            int length = (list.Count > count) ? count : list.Count;
            List<T>[] Array = new List<T>[length];
            UnityEngine.Debug.Log("source length:" + length);

            for (int elemNumber = 0; elemNumber < Array.Length; elemNumber++)
            {
                UnityEngine.Debug.Log("el:" + elemNumber);
                if (Array[elemNumber] == null) { Array[elemNumber] = new List<T>(); }
                Array[elemNumber].Add(list[0]);
                list.Remove(list[0]);
                if (elemNumber == Array.Length - 1 && list.Count > 0) { elemNumber = -1; }
                if (list.Count == 0) break;
            }
            return Array;
        }

        public static PointInt GetMaxSizesTextures(Texture2D[] textures)
        {

            int maxWidth = 0;
            int maxHeight = 0;
            foreach (Texture2D t2d in textures)
            {
                maxWidth = (t2d.width > maxWidth) ? t2d.width : maxWidth;
                maxHeight = (t2d.height > maxHeight) ? t2d.height : maxHeight;
            }
            PointInt maxP = new PointInt(maxWidth, maxHeight);
            return maxP;
        }

        public static PointInt GetMaxSizesTextures(Sprite[] sprites)
        {
            Texture2D[] textures = new Texture2D[sprites.Length];
            for (int iS = 0; iS < sprites.Length; iS++) { textures[iS] = sprites[iS].texture; }
            return GetMaxSizesTextures(textures);
        }

        public static Texture2D ExtendTextureSize(Texture2D texture, PointInt newSize)
        {
            int oldWidth = texture.width;
            int oldHeight = texture.height;
            int newWidth = newSize.X;
            int newHeight = newSize.Y;
            int deltaW = (newWidth - oldWidth) / 2;
            int deltaH = (newHeight - oldHeight) / 2;
            Texture2D extendTexture = new Texture2D(newWidth, newHeight);
            for (int iH = 0; iH < newHeight; iH++)
            {
                for (int iW = 0; iW < newWidth; iW++)
                {
                    Color color = (iW < deltaW || iW > newWidth - deltaW) ? new Color(0f, 0f, 0f, 0f) : texture.GetPixel(iW - deltaW, iH - deltaH);
                    extendTexture.SetPixel(iW, iH, color);
                }

            }
            extendTexture.Apply();
            return extendTexture;
        }

        public static Texture2D MergeSprites(Sprite[] sprites)
        {
            PointInt Size = GetMaxSizesTextures(sprites);
            Texture2D mergedTexture = new Texture2D(Size.X, Size.Y);
            for (int y = 0; y < Size.Y; y++)
            {
                for (int x = 0; x < Size.X; x++)
                {
                    Color color = new Color(0, 0, 0, 0);
                    mergedTexture.SetPixel(x, y, color);
                }
            }
            mergedTexture.Apply();
            Texture2D[] extendedTextures = new Texture2D[sprites.Length];
            for (int it = 0; it < sprites.Length; it++)
            {
                extendedTextures[it] = ExtendTextureSize(sprites[it].texture, Size);
            }
            UnityEngine.Debug.Log("ext text: " + Size.X + ";" + Size.Y);

            for (int i = 0; i < sprites.Length; i++)
            {
                Texture2D mergedTextureT = CombineTextureAOverB(extendedTextures[i], mergedTexture);
                mergedTexture = mergedTextureT;
            }
            return mergedTexture;
        }

        private static Texture2D CombineTextureAOverB(Texture2D A, Texture2D B)
        {
            Texture2D mergedTexture = new Texture2D(A.width, A.height);

            mergedTexture.Apply();
            for (int y = 0; y < A.height; y++)
            {
                for (int x = 0; x < A.width; x++)
                {
                    Color colorA = A.GetPixel(x, y);
                    Color colorB = B.GetPixel(x, y);
                    Color colorO = (colorA * colorA.a + colorB * colorB.a * (1f - colorA.a)) / (colorA.a + colorB.a * (1 - colorA.a));
                    mergedTexture.SetPixel(x, y, colorO);
                }
            }
            mergedTexture.Apply();
            return mergedTexture;
        }

        public static int getDigit(int cyff, int digit)
        {
            int ti = 0;
            cyff = (int)(cyff / Mathf.Pow(10, digit - 1));
            ti = cyff - (int)(cyff / 10) * 10;
            return ti;
        }

        public static void SaveTextureToPNGFile(Texture2D tex, string fName)
        {
            // Encode texture into PNG
            byte[] bytes = tex.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/../" + fName, bytes);
        }

        public static void DebugInfo(string dText)
        {
            Text dT = GameObject.FindGameObjectWithTag("debugText").GetComponent<Text>();
            if (dT)
                dT.text = dText;
        }

        System.Text.StringBuilder sB = new StringBuilder();
        public static void DebugInfo(char[] dText)
        {
            Text dT = GameObject.FindGameObjectWithTag("debugText").GetComponent<Text>();
            if (dT)
                dT.text = new string(dText);
        }

        public static void ScreenPhysSize() //http://forum.unity3d.com/threads/finding-physical-screen-size.203017/
        {

        }

        /// <summary>
        /// Search in dictionary word or part words.
        /// </summary>
        /// <param name="searchString">Searched string</param>
        /// <param name="dictL">Dictionary list</param>
        /// <returns>void</returns>
        private int MonoSearch(string searchString, List<string> dictL)
        {
            int wordCounter = 0;
            string searscStringUpper = searchString.ToUpper();
            dictL.ForEach(s => { if (s.IndexOf(searscStringUpper) != -1) { wordCounter++; } });
            return wordCounter;
        }

        public static string ObjectAddress(System.Object o)
        {
            GCHandle handle = GCHandle.Alloc(o, GCHandleType.Normal);
            IntPtr pointer = GCHandle.ToIntPtr(handle);
            handle.Free();
            return " address: " + "0x" + pointer.ToString("X");
        }

        #region arrays
        public static T[] GetRow<T>(T[,] matrix, int row)
        {
            var columns = matrix.GetLength(1);
            var array = new T[columns];
            for (int i = 0; i < columns; ++i)
                array[i] = matrix[row, i];
            return array;
        }

        public static T[] GetColumn<T>(T[,] matrix, int column)
        {
            var rows = matrix.GetLength(0);
            var array = new T[rows];
            for (int i = 0; i < rows; ++i)
                array[i] = matrix[i, column];
            return array;
        }
        #endregion arrays

        public string CalculateMD5Hash(string input) //https://blogs.msdn.microsoft.com/csharpfaq/2006/10/09/how-do-i-calculate-a-md5-hash-from-a-string/
        {//https://msdn.microsoft.com/ru-ru/library/s02tk69a(v=vs.110).aspx
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }

    public class DisplayMetricsAndroid
    {

        // The logical density of the display
        public static float Density { get; protected set; }

        // The screen density expressed as dots-per-inch
        public static int DensityDPI { get; protected set; }

        // The absolute height of the display in pixels
        public static int HeightPixels { get; protected set; }

        // The absolute width of the display in pixels
        public static int WidthPixels { get; protected set; }

        // A scaling factor for fonts displayed on the display
        public static float ScaledDensity { get; protected set; }

        // The exact physical pixels per inch of the screen in the X dimension
        public static float XDPI { get; protected set; }

        // The exact physical pixels per inch of the screen in the Y dimension
        public static float YDPI { get; protected set; }

        static DisplayMetricsAndroid()
        {
            // Early out if we're not on an Android device
            if (Application.platform != RuntimePlatform.Android)
            {
                return;
            }

            // The following is equivalent to this Java code:
            //
            // metricsInstance = new DisplayMetrics();
            // UnityPlayer.currentActivity.getWindowManager().getDefaultDisplay().getMetrics(metricsInstance);
            //
            // ... which is pretty much equivalent to the code on this page:
            // http://developer.android.com/reference/android/util/DisplayMetrics.html

            using (
              AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"),
              metricsClass = new AndroidJavaClass("android.util.DisplayMetrics")
            )
            {
                using (
                 AndroidJavaObject metricsInstance = new AndroidJavaObject("android.util.DisplayMetrics"),
                 activityInstance = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"),
                 windowManagerInstance = activityInstance.Call<AndroidJavaObject>("getWindowManager"),
                 displayInstance = windowManagerInstance.Call<AndroidJavaObject>("getDefaultDisplay")
                )
                {
                    displayInstance.Call("getMetrics", metricsInstance);
                    Density = metricsInstance.Get<float>("density");
                    DensityDPI = metricsInstance.Get<int>("densityDpi");
                    HeightPixels = metricsInstance.Get<int>("heightPixels");
                    WidthPixels = metricsInstance.Get<int>("widthPixels");
                    ScaledDensity = metricsInstance.Get<float>("scaledDensity");
                    XDPI = metricsInstance.Get<float>("xdpi");
                    YDPI = metricsInstance.Get<float>("ydpi");
                }
            }
        }
    }

    public struct PointInt
    {
        public int X;
        public int Y;

        public PointInt(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}