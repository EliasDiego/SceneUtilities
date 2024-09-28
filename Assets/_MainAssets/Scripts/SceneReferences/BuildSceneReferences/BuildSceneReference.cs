using System;
using UnityEngine;

namespace YergoLabs.SceneReferences
{
    [Serializable]
    public class BuildSceneReference
    {
        [SerializeField] private string _Path;
        [SerializeField] private int _BuildIndex = -1;

        public string Path => _Path;
        public int BuildIndex => _BuildIndex;
    }
}