using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LabQuestDataOption : SerializedMonoBehaviour
{
    public Toggle toggle;
    public TMP_Text optionText;
    [ReadOnly]
    public ChoiceIndexStr choiceIndexStr;

    public virtual Toggle InitOption(Choice choice)
    {
        return null;
    }
}
