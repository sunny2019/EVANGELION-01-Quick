﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabQuestDataTextOption : LabQuestDataOption
{
    public override Toggle InitOption(Choice choice)
    {
        TxtChoice txtChoice=choice as TxtChoice;
        optionText.text = txtChoice.option + "." + txtChoice.choiceContent;
        choiceIndexStr = txtChoice.option;
        return toggle;
    }
}