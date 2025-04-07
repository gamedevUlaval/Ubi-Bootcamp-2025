using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

namespace Menus
{
    public class SliderNotchEditor : MonoBehaviour
    {
        [SerializeField] RectTransform labelsContainer;
        [SerializeField] GameObject labelPrefab;
        [SerializeField] int notchesCount = 0;
        [SerializeField] float labelVerticalOffset = 20f;

        void OnValidate()
        {
            if (labelsContainer is null)
            {
                labelsContainer = GetComponent<RectTransform>();
            }
        }

        [ContextMenu("Add Notches")]
        public void AddNotches()
        {
            if (labelPrefab is null)
            {
                Debug.LogError("Label prefab is not assigned");
                return;
            }

            if (labelsContainer is null)
            {
                Debug.LogError("Labels container is not assigned");
                return;
            }
            #if UNITY_EDITOR
            // Clear only labels added by this tool.
            for (int i = labelsContainer.childCount - 1; i >= 0; i--)
            {
                Transform child = labelsContainer.GetChild(i);
                if (child is not null && child.GetComponent<SliderNotchLabel>() is not null)
                {
                    DestroyImmediate(child.gameObject);
                }
            }

            for (int i = 0; i < notchesCount; i++)
            {
                GameObject newLabel = (GameObject)PrefabUtility.InstantiatePrefab(labelPrefab, labelsContainer);
                newLabel.transform.localScale = Vector3.one;
                newLabel.name = labelPrefab.name + "_Notch_" + i;
                newLabel.AddComponent<SliderNotchLabel>();
            }
            #endif
            RefreshNotches();
        }

        [ContextMenu("Refresh Notches")]
        public void RefreshNotches()
        {
            if (labelsContainer is null)
            {
                Debug.LogError("Labels container is not assigned");
                return;
            }

            if (notchesCount <= 0)
            {
                Debug.LogWarning("Notches count must be greater than 0");
                return;
            }

            RectTransform sliderRect = GetComponent<RectTransform>();
            if (sliderRect is null)
            {
                Debug.LogError("Slider RectTransform not found");
                return;
            }

            float sliderWidth = sliderRect.rect.width;
            float sliderHeight = sliderRect.rect.height;
            float spacing = (notchesCount > 1) ? sliderWidth / (notchesCount - 1) : 0f;
            float yPos = -sliderHeight / 2f - labelVerticalOffset;

            SliderNotchLabel[] markers = labelsContainer.GetComponentsInChildren<SliderNotchLabel>();
            List<SliderNotchLabel> notchLabels = new List<SliderNotchLabel>(markers);
            notchLabels.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
            if (notchLabels.Count != notchesCount)
            {
                Debug.LogWarning("Number of added labels (" + notchLabels.Count + ") does not match notchesCount (" +
                                 notchesCount + ").");
            }

            for (int i = 0; i < notchLabels.Count; i++)
            {
                RectTransform rt = notchLabels[i].GetComponent<RectTransform>();
                if (rt is not null)
                {
                    float xPos = -sliderWidth / 2f + (notchesCount > 1 ? i * spacing : 0);
                    rt.anchoredPosition = new Vector2(xPos, yPos);
                }
            }
        }
    }

    // Marker component to identify labels added by SliderNotchEditor.
    public class SliderNotchLabel : MonoBehaviour
    {
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(SliderNotchEditor))]
    public class SliderNotchEditorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            SliderNotchEditor editor = (SliderNotchEditor)target;
            GUILayout.Space(10);
            if (GUILayout.Button("Add Notches"))
            {
                editor.AddNotches();
            }

            if (GUILayout.Button("Refresh Notches"))
            {
                editor.RefreshNotches();
            }
        }
    }
    #endif
}