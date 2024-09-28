using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;

using UnityEditorInternal;

namespace YergoLabs.Scenes.Groups
{
    [CustomEditor(typeof(SceneGroup))]
    public class SceneGroupEditor : Editor
    {
        private SerializedProperty _ActiveSceneIndexProperty;
        private SerializedProperty _SceneInfosProperty;
        private List<SceneInfoObject> _SceneInfoObjects;

        private ReorderableList _ReorderableList;

        private class SceneInfoObject
        {
            private SerializedProperty _SceneInfoProperty;
            private SceneAsset _SceneAsset;

            public SerializedProperty SerializedSceneInfoProperty => _SceneInfoProperty;
            public SceneAsset SceneAsset => _SceneAsset;

            public SceneInfoObject(SerializedObject serializedObject, string sceneInfoPropertyPath) : this(serializedObject.FindProperty(sceneInfoPropertyPath)) { }

            public SceneInfoObject(SerializedProperty sceneInfoProperty)
            {
                _SceneInfoProperty = sceneInfoProperty;

                _SceneAsset = GetSceneAsset(sceneInfoProperty);
            }

            public SceneInfoObject(SerializedProperty sceneInfoProperty, SceneAsset sceneAsset)
            {
                _SceneInfoProperty = sceneInfoProperty;

                _SceneAsset = sceneAsset;
            }

            private static IEnumerable<Scene> GetLoadedScenes()
            {
                for(int i = 0; i < EditorSceneManager.sceneCount; i++)
                    yield return EditorSceneManager.GetSceneAt(i);
            }

            [OnOpenAsset]
            public static bool OnOpenAsset(int instanceID, int _)
            {
                // UnityEngine.Object assetObject = EditorUtility.InstanceIDToObject(instanceID);

                // if(!(assetObject is SceneGroup sceneGroup))
                //     return false;

                // Scene[] loadedScenes = GetLoadedScenes()?.ToArray();
                // Scene[] dirtyScenes = loadedScenes?.Where(scene => scene.isDirty)?.ToArray();

                // bool userDidNotAborted = EditorSceneManager.SaveModifiedScenesIfUserWantsTo(dirtyScenes);

                // if(!userDidNotAborted || sceneGroup.SceneInfos?.Length <= 0)
                //     return true;

                // int index = 0;

                // do
                // {
                //     LoadScene(sceneGroup, index, index > 0);

                //     index++;
                // }
                // while(index < sceneGroup.SceneInfos.Length);
                
                return true;
            }

            private static void LoadScene(SceneGroup sceneGroup, int index, bool isAdditive)
            {
                // SceneInfo sceneInfo = sceneGroup.SceneInfos[index];

                // if(string.IsNullOrEmpty(sceneInfo.Name))
                //     return;

                // Scene loadedScene = EditorSceneManager.OpenScene(sceneInfo.Path, isAdditive ? OpenSceneMode.Additive : OpenSceneMode.Single);

                // if(index != sceneGroup.ActiveSceneIndex)
                //     return;

                // EditorSceneManager.SetActiveScene(loadedScene);
            }

            // private void SetSceneInfo(SceneInfo sceneInfo)
            // {
            //     SerializedProperty nameProperty = _SceneInfoProperty.FindPropertyRelative("name");
            //     SerializedProperty pathProperty = _SceneInfoProperty.FindPropertyRelative("path");

            //     nameProperty.stringValue = sceneInfo.Name;
            //     pathProperty.stringValue = sceneInfo.Path;
            // }

            private SceneAsset GetSceneAsset(SerializedProperty sceneInfoProperty)
            {
                SerializedProperty pathProperty = sceneInfoProperty.FindPropertyRelative("path");

                return AssetDatabase.LoadAssetAtPath<SceneAsset>(pathProperty.stringValue);
            }

            public SceneAsset DrawSceneAssetField(Rect rect)
            {
                return (SceneAsset)EditorGUI.ObjectField(rect, SceneAsset, typeof(SceneAsset), false);
            }

            public void SetSceneInfo(SceneAsset sceneAsset)
            {
                _SceneAsset = sceneAsset;

                // SceneInfo sceneInfo = new SceneInfo()
                // {
                //     Name = SceneAsset?.name,
                //     Path = AssetDatabase.GetAssetPath(SceneAsset)
                // };

                // SetSceneInfo(sceneInfo);
            }
        }

        private void OnEnable() 
        {
            _SceneInfosProperty = serializedObject.FindProperty("_SceneInfos");
            _ActiveSceneIndexProperty = serializedObject.FindProperty("_ActiveSceneIndex");

            _SceneInfoObjects = GetEnumerable(_SceneInfosProperty)?.Select(property => new SceneInfoObject(property))?.ToList();

            _ReorderableList = new ReorderableList(_SceneInfoObjects, typeof(SceneAsset))
            {
                displayAdd = true,
                draggable = false,
                drawHeaderCallback = OnDrawHeader,
                drawElementCallback = OnDrawElement,
                onAddCallback = OnAddCallback,
                onRemoveCallback = OnRemoveCallback,
                onReorderCallbackWithDetails = OnReorderCallback
            };
        }

        private void OnDrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Scenes");
        }

        private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SceneInfoObject sceneInfoObject = (SceneInfoObject)_ReorderableList.list[index];

            Rect rectBoxRect = new Rect(rect.x, rect.y, 20, rect.height);
            Rect sceneAssetFieldRect = new Rect(rect.x + 20, rect.y, rect.width - 20, rect.height);

            bool contains = ContainsSceneInEditorBuildSettings(sceneInfoObject.SceneAsset);

            Color rectColor = contains ? Color.green : Color.yellow;

            EditorGUI.DrawRect(rectBoxRect, rectColor);

            SceneAsset sceneAsset = sceneInfoObject.DrawSceneAssetField(sceneAssetFieldRect);

            if(sceneInfoObject.SceneAsset && (sceneInfoObject.SceneAsset == sceneAsset || _SceneInfoObjects == null))
                return;

            if(_SceneInfoObjects.Any(sceneInfoObject => sceneInfoObject.SceneAsset == sceneAsset))
                return;

            sceneInfoObject.SetSceneInfo(sceneAsset);
        }

        private void OnAddCallback(ReorderableList list)
        {
            int listCount = list.list.Count;

            _SceneInfosProperty.InsertArrayElementAtIndex(listCount);

            SerializedProperty sceneInfoProperty = _SceneInfosProperty.GetArrayElementAtIndex(listCount);

            SceneInfoObject sceneInfoObject = new SceneInfoObject(sceneInfoProperty, null);

            list.list.Add(sceneInfoObject);
        }

        private void OnRemoveCallback(ReorderableList list)
        {
            for(int i = 0; i < list.selectedIndices?.Count; i++)
            {
                int index = list.selectedIndices[i];

                list.list.RemoveAt(index);

                _SceneInfosProperty.DeleteArrayElementAtIndex(index);
            }
        }

        private void OnReorderCallback(ReorderableList list, int oldIndex, int newIndex)
        {
            object item = list.list[oldIndex];

            int sign = (int)Mathf.Sign(oldIndex - newIndex);

            list.list.RemoveAt(oldIndex);
            list.list.Insert(newIndex + sign, item);

            // Apparently don't need to rearrange Scene Name and Path because of the Draw Element Callback
        }

        private bool ContainsSceneInEditorBuildSettings(SceneAsset sceneAsset)
        {
            if(!sceneAsset)
                return false;

            for(int i = 0; i < EditorSceneManager.sceneCountInBuildSettings; i++)
            {
                Scene sceneInBuildSettings = EditorSceneManager.GetSceneByBuildIndex(i);

                if(sceneInBuildSettings.name == sceneAsset.name)
                    return true;
            }

            return false;
        }

        private void DrawActiveSceneIndex()
        {
            _ActiveSceneIndexProperty.intValue = EditorGUILayout.IntPopup(
                "Active Scene", 
                _ActiveSceneIndexProperty.intValue, 
                _SceneInfoObjects?.
                    Select(sceneInfoObject => sceneInfoObject?.SceneAsset?.name ?? "-----")?.ToArray() ?? new string[] { "None"},
                _SceneInfoObjects?.
                    Select((sceneInfoObject, index) => index)?.ToArray() ?? new int[] { -1 });
        }

        private static IEnumerable<SerializedProperty> GetEnumerable(SerializedProperty source)
        {
            if(source == null)
                throw new ArgumentException($"{source} is null!");

            for(int i = 0; i < source.arraySize; i++)
                yield return source.GetArrayElementAtIndex(i);
        }

        public override void OnInspectorGUI()
        {
            DrawActiveSceneIndex();

            _ReorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}