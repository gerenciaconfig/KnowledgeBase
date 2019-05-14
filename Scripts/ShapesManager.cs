using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///Developed by Indie Studio
///https://www.assetstore.unity3d.com/en/#!/publisher/9268
///www.indiestd.com
///info@indiestd.com

namespace Letra
{
    public class ShapesManager : MonoBehaviour
    {
            /// <summary>
            /// The shapes list.
            /// </summary>
            public List<Shape> shapes = new List<Shape> ();

            /// <summary>
            /// The last selected group.
            /// </summary>
            [HideInInspector]
            public int lastSelectedGroup;

            /// <summary>
            /// Shapes Manager Prefix
            /// </summary>
            public static string shapePrefix = "Uppercase";

            /// <summary>
            /// General single instance.
            /// </summary>
            public static ShapesManager instance;

            void Awake ()
            {
                    if (instance == null) {
                            instance = this;
                            lastSelectedGroup = 0;
                            DontDestroyOnLoad (gameObject);
                    } else {
                            Destroy (gameObject);
                    }
            }

            /// <summary>
            /// Get the current shape.
            /// </summary>
            /// <returns>The current shape.</returns>
            public Shape GetCurrentShape()
            {
                return shapes[Shape.selectedShapeID];
            }

            [System.Serializable]
            public class Shape
            {
                    public bool showContents = true;

                    /// <summary>
                    /// gamePrefab / shape prefab
                    /// </summary>
                    public GameObject gamePrefab;

                    /// <summary>
                    /// The audio clip of the shape , used for spelling.
                    /// </summary>
                    public AudioClip clip;

                    /// <summary>
                    /// The stars time period.
                    /// 0 - 14 seconds : 3 stars , 15 - 29 : 2 stars , otherwisee 1 star
                    /// </summary>
                    public int starsTimePeriod = 15;

                    /// <summary>
                    /// The number of the collected stars.
                    /// </summary>
                    [HideInInspector]
                    public StarsNumber starsNumber = StarsNumber.ZERO;

                    /// <summary>
                    /// Whether the shape is locked or not.
                    /// </summary>
                    [HideInInspector]
                    public bool isLocked = true;

                    /// <summary>
                    /// The ID selected/current shape.
                    /// </summary>
                    public static int selectedShapeID;

                    public enum StarsNumber
                    {
                        ZERO,
                        ONE,
                        TWO,
                        THREE
                    }
            }
    }
}