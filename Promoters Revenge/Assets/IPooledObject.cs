using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal interface IPooledObject
{
    ObjectPooler.ObjectInfo.ObjectType Type { get; }
}