using System;
using UnityEngine;

namespace YergoLabs.SceneReferences
{
    [Serializable]
    public class SceneReference
    {
        [SerializeField] private string _Name;
        [SerializeField] private string _Path;

        public string Name => _Name;
        public string Path => _Path;
    }
}