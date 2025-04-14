using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Menus
{
    public enum LayoutType { Vertical, Horizontal, Grid }

    public class ButtonLayoutEditor : MonoBehaviour
    {
        [SerializeField] RectTransform container;
        [SerializeField] GameObject objectPrefab;
        [SerializeField] int objectToAdd = 0;
        [SerializeField] float paddingOffset = 0f;
        [SerializeField] LayoutType layoutType = LayoutType.Vertical;

        [ContextMenu("Add Object")]
        public void AddObject()
        {
            if (objectPrefab is null)
            {
                Debug.LogError("Object prefab is not assigned");
                return;
            }
            #if UNITY_EDITOR
            for (int i = 0; i < objectToAdd; i++)
            {
                GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(objectPrefab, transform);
                newObject.transform.localScale = Vector3.one;
                newObject.name = objectPrefab.name + "_" + transform.childCount;
            }
            #endif
            LayoutObjects();
        }

        [ContextMenu("Refresh Layout")]
        public void LayoutObjects()
        {
            if (container is null)
            {
                Debug.LogError("Container is not assigned");
                return;
            }

            int childCount = transform.childCount;
            if (childCount == 0)
            {
                Debug.LogWarning("No child object found");
                return;
            }

            float containerWidth = container.rect.width;
            float containerHeight = container.rect.height;

            RectTransform refObject = transform.GetChild(0).GetComponent<RectTransform>();
            if (refObject is null && objectPrefab is not null)
            {
                refObject = objectPrefab.GetComponent<RectTransform>();
            }
            if (refObject is null)
            {
                Debug.LogError("Could not determine object size (RectTransform not found)");
                return;
            }

            float defaultObjectWidth = refObject.sizeDelta.x;
            float defaultObjectHeight = refObject.sizeDelta.y;

            // Set object layout based on layout type
            switch (layoutType)
            {
                case LayoutType.Vertical:
                {
                    float autoPaddingVertical = (containerHeight - childCount * defaultObjectHeight) / (childCount + 1);
                    float finalPaddingVertical = autoPaddingVertical + paddingOffset;
                    float autoPaddingHorizontal = (containerWidth - defaultObjectWidth) / 2f;
                    float finalPaddingHorizontal = autoPaddingHorizontal + paddingOffset;
                    float finalObjectWidth = containerWidth - 2 * finalPaddingHorizontal;
                    float spacingVertical = (childCount > 1) ? (containerHeight - childCount * defaultObjectHeight - 2 * finalPaddingVertical) / (childCount - 1) : 0f;
                    float startY = containerHeight / 2f - finalPaddingVertical - defaultObjectHeight / 2f;

                    for (int i = 0; i < childCount; i++)
                    {
                        Transform child = transform.GetChild(i);
                        RectTransform rt = child.GetComponent<RectTransform>();
                        if (rt is not null)
                        {
                            float posY = startY - i * (defaultObjectHeight + spacingVertical);
                            rt.sizeDelta = new Vector2(finalObjectWidth, defaultObjectHeight);
                            rt.anchoredPosition = new Vector2(0, posY);
                        }
                    }
                    break;
                }
                case LayoutType.Horizontal:
                {
                    float autoPaddingHorizontal = (containerWidth - childCount * defaultObjectWidth) / (childCount + 1);
                    float finalPaddingHorizontal = autoPaddingHorizontal + paddingOffset;
                    float spacingHorizontal = (childCount > 1) ? (containerWidth - childCount * defaultObjectWidth - 2 * finalPaddingHorizontal) / (childCount - 1) : 0f;
                    float finalObjectWidth = (containerWidth - 2 * finalPaddingHorizontal - (childCount - 1) * spacingHorizontal) / childCount;
                    float startX = -containerWidth / 2f + finalPaddingHorizontal + finalObjectWidth / 2f;

                    for (int i = 0; i < childCount; i++)
                    {
                        Transform child = transform.GetChild(i);
                        RectTransform rt = child.GetComponent<RectTransform>();
                        if (rt is not null)
                        {
                            float posX = startX + i * (finalObjectWidth + spacingHorizontal);
                            rt.sizeDelta = new Vector2(finalObjectWidth, defaultObjectHeight);
                            rt.anchoredPosition = new Vector2(posX, 0);
                        }
                    }
                    break;
                }
                case LayoutType.Grid:
                {
                    int columns = Mathf.CeilToInt(Mathf.Sqrt(childCount));
                    int rows = Mathf.CeilToInt(childCount / (float)columns);

                    float autoPaddingHorizontal = (containerWidth - columns * defaultObjectWidth) / (columns + 1);
                    float finalPaddingHorizontal = autoPaddingHorizontal + paddingOffset;
                    float autoPaddingVertical = (containerHeight - rows * defaultObjectHeight) / (rows + 1);
                    float finalPaddingVertical = autoPaddingVertical + paddingOffset;

                    float spacingHorizontal = (columns > 1) ? (containerWidth - columns * defaultObjectWidth - 2 * finalPaddingHorizontal) / (columns - 1) : 0f;
                    float spacingVertical = (rows > 1) ? (containerHeight - rows * defaultObjectHeight - 2 * finalPaddingVertical) / (rows - 1) : 0f;

                    float finalObjectWidth = (containerWidth - 2 * finalPaddingHorizontal - (columns - 1) * spacingHorizontal) / columns;
                    float finalObjectHeight = (containerHeight - 2 * finalPaddingVertical - (rows - 1) * spacingVertical) / rows;

                    for (int i = 0; i < childCount; i++)
                    {
                        int row = i / columns;
                        int col = i % columns;
                        Transform child = transform.GetChild(i);
                        RectTransform rt = child.GetComponent<RectTransform>();
                        if (rt is not null)
                        {
                            float posX = -containerWidth / 2f + finalPaddingHorizontal + finalObjectWidth / 2f + col * (finalObjectWidth + spacingHorizontal);
                            float posY = containerHeight / 2f - finalPaddingVertical - finalObjectHeight / 2f - row * (finalObjectHeight + spacingVertical);
                            rt.sizeDelta = new Vector2(finalObjectWidth, finalObjectHeight);
                            rt.anchoredPosition = new Vector2(posX, posY);
                        }
                    }
                    break;
                }
            }

            // Set explicit navigation for wrap-around
            switch (layoutType)
            {
                case LayoutType.Vertical:
                {
                    for (int i = 0; i < childCount; i++)
                    {
                        Button btn = transform.GetChild(i).GetComponent<Button>();
                        if (btn is not null)
                        {
                            Navigation nav = btn.navigation;
                            nav.mode = Navigation.Mode.Explicit;
                            Button upBtn = transform.GetChild((i == 0) ? childCount - 1 : i - 1).GetComponent<Button>();
                            Button downBtn = transform.GetChild((i == childCount - 1) ? 0 : i + 1).GetComponent<Button>();
                            nav.selectOnUp = upBtn;
                            nav.selectOnDown = downBtn;
                            btn.navigation = nav;
                        }
                    }
                    break;
                }
                case LayoutType.Horizontal:
                {
                    for (int i = 0; i < childCount; i++)
                    {
                        Button btn = transform.GetChild(i).GetComponent<Button>();
                        if (btn is not null)
                        {
                            Navigation nav = btn.navigation;
                            nav.mode = Navigation.Mode.Explicit;
                            Button leftBtn = transform.GetChild((i == 0) ? childCount - 1 : i - 1).GetComponent<Button>();
                            Button rightBtn = transform.GetChild((i == childCount - 1) ? 0 : i + 1).GetComponent<Button>();
                            nav.selectOnLeft = leftBtn;
                            nav.selectOnRight = rightBtn;
                            btn.navigation = nav;
                        }
                    }
                    break;
                }
                case LayoutType.Grid:
                {
                    int columns = Mathf.CeilToInt(Mathf.Sqrt(childCount));
                    int rows = Mathf.CeilToInt(childCount / (float)columns);
                    for (int i = 0; i < childCount; i++)
                    {
                        Button btn = transform.GetChild(i).GetComponent<Button>();
                        if (btn is not null)
                        {
                            Navigation nav = btn.navigation;
                            nav.mode = Navigation.Mode.Explicit;
                            int row = i / columns;
                            int col = i % columns;
                            int upIndex = (row == 0) ? ((rows - 1) * columns + col) : i - columns;
                            if (upIndex >= childCount) upIndex = i;
                            int downIndex = (row == rows - 1) ? col : i + columns;
                            if (downIndex >= childCount) downIndex = i;
                            int leftIndex = (col == 0) ? (i - col + (columns - 1)) : i - 1;
                            if (leftIndex >= childCount) leftIndex = i;
                            int rightIndex = (col == columns - 1) ? (i - col) : i + 1;
                            if (rightIndex >= childCount) rightIndex = i;
                            nav.selectOnUp = transform.GetChild(upIndex).GetComponent<Button>();
                            nav.selectOnDown = transform.GetChild(downIndex).GetComponent<Button>();
                            nav.selectOnLeft = transform.GetChild(leftIndex).GetComponent<Button>();
                            nav.selectOnRight = transform.GetChild(rightIndex).GetComponent<Button>();
                            btn.navigation = nav;
                        }
                    }
                    break;
                }
            }

            // Select first button by default
            if (EventSystem.current is not null && transform.GetChild(0) is not null)
            {
                Button firstBtn = transform.GetChild(0).GetComponent<Button>();
                if (firstBtn is not null)
                {
                    EventSystem.current.SetSelectedGameObject(firstBtn.gameObject);
                }
            }
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(ButtonLayoutEditor))]
    public class ButtonLayoutEditorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ButtonLayoutEditor layoutEditor = (ButtonLayoutEditor)target;
            GUILayout.Space(10);
            if (GUILayout.Button("Add Buttons"))
            {
                layoutEditor.AddObject();
            }
            if (GUILayout.Button("Refresh Layout"))
            {
                layoutEditor.LayoutObjects();
            }
        }
    }
    #endif
}
