using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YergoLabs.SceneReferences
{
    public class SceneReferenceTest : MonoBehaviour
    {
        [SerializeField] private BuildSceneReference _SceneReference;

        private void Awake()
        {
            Debug.Log($"{_SceneReference.BuildIndex} {_SceneReference.Path}");
        }
    }
}