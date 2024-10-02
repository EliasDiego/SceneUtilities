using System;
using UnityEngine;

namespace YergoLabs.SceneReferences
{
    [Serializable]
    public struct BuildSceneReference
    {
        [SerializeField] private int _BuildIndex;

        public int BuildIndex => _BuildIndex;
    }
}