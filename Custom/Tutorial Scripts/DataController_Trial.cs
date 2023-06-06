using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using USE_Data;

public class DataController_Trial : DataController {
    public ControlLevel_Block blockLevel;
    public ControlLevel_Trial trialLevel;

    public override void DefineDataController()
    {
        blockLevel = GameObject.Find("ControlLevels").GetComponent<ControlLevel_Block>();
        trialLevel = GameObject.Find("ControlLevels").GetComponent<ControlLevel_Trial>();
        AddDatum("Block", () => blockLevel.currentBlock);
        AddDatum("TrialInBlock", () => trialLevel.trialInBlock);
        AddDatum("TrialInExperiment", () => trialLevel.trialInExperiment);
        AddDatum("Response", ()=> trialLevel.response);
        AddDatum("Reward", () => trialLevel.reward);
        AddDatum("Stim1_name", () => trialLevel.stim1.name);
        AddDatum("Stim1_targetStatus", () => trialLevel.stim1.tag == "Target" ? 1 : 0);
        AddDatum("Stim1_worldX", () => trialLevel.stim1.transform.position.x);
        AddDatum("Stim1_worldY", () => trialLevel.stim1.transform.position.y);
        AddDatum("Stim1_worldZ", () => trialLevel.stim1.transform.position.z);
        AddDatum("Stim1_screenX", () => Camera.main.WorldToScreenPoint(trialLevel.stim1.transform.position).x);
        AddDatum("Stim1_screenY", () => Camera.main.WorldToScreenPoint(trialLevel.stim1.transform.position).y);
        AddDatum("Stim2_name", () => trialLevel.stim2.name);
        AddDatum("Stim2_targetStatus", () => trialLevel.stim2.tag == "Target" ? 1 : 0);
        AddDatum("Stim2_worldX", () => trialLevel.stim2.transform.position.x);
        AddDatum("Stim2_worldY", () => trialLevel.stim2.transform.position.y);
        AddDatum("Stim2_worldZ", () => trialLevel.stim2.transform.position.z);
        AddDatum("Stim2_screenX", () => Camera.main.WorldToScreenPoint(trialLevel.stim2.transform.position).x);
        AddDatum("Stim2_screenY", () => Camera.main.WorldToScreenPoint(trialLevel.stim2.transform.position).y);
        AddStateTimingData(trialLevel, new string[] { "Duration", "StartFrame", "EndFrame" });
    }
}
