using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(order = 1, menuName = "ELConfigs/OldPeopleDataConfig")]
public class OldPeopleDataConfig : SerializedScriptableObject
{
    public Dictionary<OldPeopleType, OldPeopleData> OldPeopleDatas = new Dictionary<OldPeopleType, OldPeopleData>();
}
