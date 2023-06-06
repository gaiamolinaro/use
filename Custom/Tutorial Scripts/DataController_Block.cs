using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using USE_Data;

public class DataController_Block : DataController {
    public ControlLevel_Block blockLevel;
    public ControlLevel_Trial trialLevel;

    public override void DefineDataController()
    {
        blockLevel = GameObject.Find("ControlLevels").GetComponent<ControlLevel_Block>();
        trialLevel = GameObject.Find("ControlLevels").GetComponent<ControlLevel_Trial>();
        AddDatum("Block", () => blockLevel.currentBlock);
        AddDatum("FirstTrial", () => blockLevel.firstTrial);
        AddDatum("LastTrial", () => blockLevel.lastTrial);
        AddDatum("Proportion Correct", () => trialLevel.numCorrect / trialLevel.numTrials);
        AddDatum("Proportion Reward", () => trialLevel.numReward / trialLevel.numTrials);
        AddStateTimingData(blockLevel, new string[] { "Duration", "StartFrame", "EndFrame" });
    }
}
