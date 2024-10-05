using System;
using UnityEngine;

namespace YergoLabs.SceneUtilities.SceneReferences
{
    [Serializable]
    public struct BuildSceneReference
    {
        [SerializeField] private int _BuildIndex;

        public int BuildIndex => _BuildIndex;
    }
}