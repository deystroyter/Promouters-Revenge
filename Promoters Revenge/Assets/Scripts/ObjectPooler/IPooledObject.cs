using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    internal interface IPooledObject
    {
        ObjectPooler.ObjectInfo.ObjectType Type { get; }
    }
}