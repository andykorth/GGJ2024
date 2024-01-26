using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Structure that represents a range of float values.  Has a custom editor for inspector input.
/// </summary>
[System.Serializable]
public struct Rangef
{
    [SerializeField]
    private float min;
    [SerializeField]
    private float max;
    
    /// <summary>
    /// Value constructor.
    /// </summary>
    /// <param name="min">The lowest value in the range.</param>
    /// <param name="max">The highest value in the range.</param>
    public Rangef(float min = 0, float max = 0)
    {
        this.min = Mathf.Min(min, max);
        this.max = Mathf.Max(min, max);
    }

    /// <summary>
    /// The minimum value in the range.  (Assigning a large number will auto-arrange min and max values.)
    /// </summary>
    public float Min
    {
        get
        {
            return min;
        }

        set
        {
            if (value > max)
            {
                min = max;
                max = value;
            }
            else
                min = value;
        }
    }

    /// <summary>
    /// The maximum value in the range.  (Assigning a small number will auto-arrange min and max values.)
    /// </summary>
    public float Max
    {
        get
        {
            return max;
        }

        set
        {
            if (value < min)
            {
                max = min;
                min = value;
            }
            else
                max = value;
        }
    }

    /// <summary>
    /// Linear interpolate between the min and max values.
    /// </summary>
    /// <param name="t">A percentage of interpolation between 0.0 and 1.0.</param>
    /// <returns>The interpolated value.</returns>
    public float Lerp(float t)
    {
        return Mathf.Lerp(min, max, Mathf.Clamp01(t));
    }

    /// <summary>
    /// Returns a random float between the min and max values (inclusive.)
    /// </summary>
    /// <returns>A random float between the min and max values (inclusive.)</returns>
    public float Random()
    {
        return (max - min) * UnityEngine.Random.value + min;
    }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(Rangef))]
public class RangefPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 3f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // layout rects
        Rect labelRect = new Rect()
        {
            x = position.x,
            y = position.y,
            width = position.width,
            height = EditorGUIUtility.singleLineHeight
        };

        Rect minRect = new Rect()
        {
            x = position.x + 10f,
            y = labelRect.y + labelRect.height,
            width = position.width - 10f,
            height = EditorGUIUtility.singleLineHeight
        };

        Rect maxRect = new Rect()
        {
            x = position.x + 10f,
            y = minRect.y + minRect.height,
            width = position.width - 10f,
            height = EditorGUIUtility.singleLineHeight
        };

        // draw parameters
        EditorGUI.LabelField(labelRect, label);
        EditorGUI.PropertyField(minRect, property.FindPropertyRelative("min"));
        EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("max"));
    }
}

#endif