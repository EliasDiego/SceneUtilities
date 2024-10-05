using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YergoLabs.SceneUtilities.SceneReferences
{
    public class SceneReferenceTest : MonoBehaviour
    {
        [SerializeField] private BuildSceneReference _BuildSceneReference;
        [SerializeField] private SceneAssetReference _SceneAssetReference;

        private void Awake()
        {
            Debug.Log($"{_BuildSceneReference.BuildIndex}");
            Debug.Log($"{_SceneAssetReference.Path}");
        }
    }
}