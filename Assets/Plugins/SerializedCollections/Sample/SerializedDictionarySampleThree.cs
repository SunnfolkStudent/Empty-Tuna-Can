using System.Collections;
using System.Collections.Generic;
using Plugins.SerializedCollections.Runtime.Scripts;
using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    public class SerializedDictionarySampleThree : MonoBehaviour
    {
        [SerializeField]
        private SerializedDictionary<ScriptableObject, string> _nameOverrides;
    }
}