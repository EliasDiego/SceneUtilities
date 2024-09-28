using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace YergoLabs.Scenes.Groups
{
    [CreateAssetMenu(menuName = "YergoLabs/Scene Group")]
    public class SceneGroup : ScriptableObject
    {
        [SerializeField] private int _ActiveSceneIndex;
        // [SerializeField] private SceneInfo[] _SceneInfos;

        // public int ActiveSceneIndex => _ActiveSceneIndex;
        // public SceneInfo[] SceneInfos => _SceneInfos;

        #if UNITY_EDITOR

        private class SceneGroupProcessor : AssetModificationProcessor
        {
            private static List<SceneGroup> _SceneGroupList = new List<SceneGroup>();

            private static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions removeOptions)
            {
                if(path.Contains(".asset"))
                {
                    SceneGroup sceneGroup = AssetDatabase.LoadAssetAtPath<SceneGroup>(path);

                    if(sceneGroup && _SceneGroupList.Contains(sceneGroup))
                    {
                        sceneGroup.OnDestroy();

                        return AssetDeleteResult.DidNotDelete;
                    }
                }

                // if(path.Contains(".unity"))
                // {
                //     List<SceneGroup> sceneGroups = _SceneGroupList?.
                //         Where(sceneGroup => sceneGroup._SceneInfos?.
                //             Any(sceneInfo => sceneInfo.Path == path) ?? false)?.
                //         ToList();

                //     sceneGroups?.
                //         ForEach(sceneGroup => 
                //         {
                //             sceneGroup._ActiveSceneIndex = -1;
                            
                //             sceneGroup._SceneInfos = sceneGroup._SceneInfos?.
                //                 Where(sceneInfo => sceneInfo.Path != path)?.
                //                 ToArray();
                //         });
                // }

                return AssetDeleteResult.DidNotDelete;
            }

            private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
            {
                string extension = ".unity";

                if(!sourcePath.Contains(extension))
                    return AssetMoveResult.DidNotMove;

                string assetName = destinationPath?.Split('/')?.Last()?.Trim(extension.ToCharArray());

                // List<SceneGroup> sceneGroups = _SceneGroupList?.
                //     Where(sceneGroup => sceneGroup._SceneInfos?.
                //         Any(sceneInfo => sceneInfo.Path == sourcePath) ?? false)?.
                //     ToList();

                // sceneGroups?.
                //     ForEach(sceneGroup => 
                //     {
                //         sceneGroup._ActiveSceneIndex = -1;

                        // int index = sceneGroup._SceneInfos?.
                        //     Select((sceneInfo, index) => Tuple.Create(index, sceneInfo))?.
                        //     FirstOrDefault(tuple => tuple.Item2.Path == sourcePath)?.Item1 ?? -1;

                        // if(index == -1)
                        //     return;

                        // sceneGroup._SceneInfos[index] = new SceneInfo()
                        // {
                        //     Name = destinationPath,
                        //     Path = assetName
                        // };
                    // });

                return AssetMoveResult.DidNotMove;
            }

            public static void AddSceneGroup(SceneGroup sceneGroup)
            {
                if(_SceneGroupList?.Contains(sceneGroup) ?? true)
                    return;

                _SceneGroupList.Add(sceneGroup);
            }

            public static void RemoveSceneGroup(SceneGroup sceneGroup)
            {
                if(!_SceneGroupList?.Contains(sceneGroup) ?? true)
                    return;

                _SceneGroupList.Remove(sceneGroup);
            }
        }

        private void Awake() 
        {
            SceneGroupProcessor.AddSceneGroup(this);
        }

        private void OnDestroy() 
        {
            Debug.Log("OnDestroy");
            SceneGroupProcessor.RemoveSceneGroup(this);
        }

        #endif
    }
}