using System;
using UnityEngine;

namespace YergoLabs.SceneUtilities.SceneReferences
{
    [Serializable]
    public struct SceneAssetReference
    {
        [SerializeField] private string _Path;

        public string Path => _Path;
    }
}