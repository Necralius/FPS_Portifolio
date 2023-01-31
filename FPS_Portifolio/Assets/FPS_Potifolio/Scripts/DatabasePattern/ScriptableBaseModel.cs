using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableBaseModel : ScriptableObject
{
    public string BaseName;

    public ScriptableBaseModel GetBase() => this;
}