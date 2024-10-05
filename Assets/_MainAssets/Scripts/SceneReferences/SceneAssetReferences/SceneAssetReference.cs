using System;
using UnityEngine;

namespace YergoLabs.SceneReferences
{
    [Serializable]
    public struct SceneAssetReference
    {
        [SerializeField] private string _Path;

        public string Path => _Path;
    }
}