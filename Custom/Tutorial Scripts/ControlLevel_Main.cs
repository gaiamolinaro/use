using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using USE_States;

public class ControlLevel_Main : ControlLevel {
    public GameObject textObj, panelObj;

    // set participant ID in the backend
    //public string participantID = "subj999";
    // allow user to set participant ID
    public SessionDetails sessionDetails;
    private string participantID;
    public string dataPath = "C:/Gaia/CCN Lab/Goal Selection Project/USE Tutorials/Data/";
    public string configPath = "C:/Gaia/CCN Lab/Goal Selection Project/USE Tutorials/Assets/_Resources/TutorialConfig.json";
    // configuration file
    // public TextAsset configFile;
    // [System.Serializable]
    // public class ExpConfigs
    //     {
    //     public float stimOnDur;	
    //     public float resposeMaxDur;	
    //     public float fbDur;	
    //     public float itiDur;	
    //     public float posRange;
    //     public float rewardProb;
    //     public float minDistance;
    //     public int numBlocks;
    //     public int numTrials;
    //     public bool storeData;
    //     }
    public override void DefineControlLevel(){
        State intro = new State("Intro");
        State mainTask = new State("MainTask");
        State goodbye = new State("Goodbye");
        AddActiveStates(new List<State> { intro, mainTask, goodbye });

        ControlLevel_Block blockLevel = transform.GetComponent<ControlLevel_Block>();
        ControlLevel_Trial trialLevel = transform.GetComponent<ControlLevel_Trial>();
        CustomConfigReader configReader = transform.GetComponent<CustomConfigReader>();
        ControlLevel_TMPSlides slideLevel = transform.GetComponent<ControlLevel_TMPSlides>();
        DataController_Block blockData = GameObject.Find("DataControllers").GetComponent<DataController_Block>();
        DataController_Trial trialData = GameObject.Find("DataControllers").GetComponent<DataController_Trial>();


        // SLIDES
        slideLevel.slideText = new string[] {"Welcome to our study!\nThank you very much for participating.",
            "In this task you will be shown two objects on each trial. You will have to choose one of them by clicking on it with the mouse.",
            "Wait for the \"Go!\" signal before clicking.", "After clicking, you will get feedback. A green square means your choice was rewarded. A red square means it was not.",
            "Try to learn which object gives the most reward.", "Ask the experimenter if you have any questions, otherwise we will begin the experiment."};
        
        // INTRO
        // alternative method (not using ConfigReader)
        // works for basic things
        intro.AddInitializationMethod(() =>
        {
            // ExpConfigs expConfigs  = JsonUtility.FromJson<ExpConfigs>(configFile.text);
            // Debug.Log(JsonUtility.ToJson(expConfigs));
            // bool storeData = expConfigs.storeData;
            Debug.Log(JsonUtility.ToJson(configReader.expConfigs));
            bool storeData = configReader.expConfigs.storeData;
            Debug.Log($"storeData {storeData}");
            // specify which data controllers will be accessible by the Block and Trial Levels
            blockLevel.blockData = blockData;
            trialLevel.trialData = trialData;
            blockData.storeData = storeData;
            trialData.storeData = storeData;
            blockData.folderPath = dataPath;
            trialData.folderPath = dataPath;
            // comment if not using an initialization screen
            string participantID = sessionDetails.GetItemValue("Participant ID");
            blockData.fileName = participantID + "_BlockData.txt";
            trialData.fileName = participantID + "_TrialData.txt";
            blockData.CreateFile();
            trialData.CreateFile();
            // experiment configuration
            // blockLevel.numBlocks = expConfigs.numBlocks;
            // blockLevel.numTrials = expConfigs.numTrials;
            // trialLevel.stimOnDur = expConfigs.stimOnDur;
            // trialLevel.responseMaxDur = expConfigs.resposeMaxDur;
            // trialLevel.fbDur = expConfigs.fbDur;
            // trialLevel.itiDur = expConfigs.itiDur;
            // trialLevel.posRange = expConfigs.posRange;
            // trialLevel.minDistance = expConfigs.minDistance;
            // trialLevel.rewardProb = expConfigs.rewardProb;
            // comment if not using an initialization screen
            // experiment configuration
            blockLevel.numBlocks = configReader.expConfigs.numBlocks;
            blockLevel.numTrials = configReader.expConfigs.numTrials;
            trialLevel.stimOnDur = configReader.expConfigs.stimOnDur;
            trialLevel.responseMaxDur = configReader.expConfigs.resposeMaxDur;
            trialLevel.fbDur = configReader.expConfigs.fbDur;
            trialLevel.itiDur = configReader.expConfigs.itiDur;
            trialLevel.posRange = configReader.expConfigs.posRange;
            trialLevel.minDistance = configReader.expConfigs.minDistance;
            trialLevel.rewardProb = configReader.expConfigs.rewardProb;
            
        });
        intro.AddChildLevel(slideLevel);
        intro.SpecifyTermination(() => slideLevel.Terminated, mainTask);
        intro.AddChildLevel(slideLevel);
        intro.SpecifyTermination(() => slideLevel.Terminated, mainTask);

        // MAIN
        mainTask.AddChildLevel(blockLevel);
        mainTask.SpecifyTermination(() => blockLevel.Terminated, goodbye);

        // GOODBYE
        goodbye.AddInitializationMethod(() =>
        {
            textObj.SetActive(true);
            panelObj.SetActive(true);
            textObj.GetComponent<TMP_Text>().text = "Thank you very much for your time!";
        });
        goodbye.AddTimer(2f, null);

    }
}