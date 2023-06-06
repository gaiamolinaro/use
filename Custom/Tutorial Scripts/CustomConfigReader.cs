using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using USE_States;
using Newtonsoft.Json;

public class CustomConfigReader : MonoBehaviour
{
    //public string configPath = "C:/Gaia/CCN Lab/Goal Selection Project/USE Tutorials/Assets/_Resources/TutorialConfig.json";
    public TextAsset configFile;
    public dynamic expConfigs;
    public void Start() {
        string serialized = configFile.text;
        expConfigs = JsonConvert.DeserializeObject(serialized);
    }
}
