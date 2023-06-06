using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USE_States;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using USE_States;

public class ControlLevel_Trial : ControlLevel
{
    //scene elements
    public GameObject stim1;
    public GameObject stim2;
    public GameObject goCue;
    public GameObject fb;

    //trial variables
    [System.NonSerialized]
    //trial variables
    public int trialInBlock, trialInExperiment = 1, response, reward;

    [System.NonSerialized]
    public float stimOnDur, responseMaxDur, fbDur, itiDur, posRange, minDistance, rewardProb;
    [System.NonSerialized]
    public int numTrials, numCorrect, numReward;
    [HideInInspector]
    public DataController_Trial trialData;

    public override void DefineControlLevel()
    {
        //define States within this Control Level
        State stimOn = new State("StimPres");
        State collectResponse = new State("Response");
        State feedback = new State("Feedback");
        State iti = new State("ITI");
        AddActiveStates(new List<State> { stimOn, collectResponse, feedback, iti });

        //Define stimOn State
        stimOn.AddInitializationMethod(() =>
        {
            //choose x/y position of first stim randomly, move second stim until it is far enough away that it doesn't overlap
            Vector3 stim1pos = AssignRandomPos();
            Vector3 stim2pos = AssignRandomPos();
            while (Vector3.Distance(stim1pos,stim2pos) < minDistance){
                stim2pos = AssignRandomPos();
            }
            stim1.transform.position = stim1pos;
            stim2.transform.position = stim2pos;
            stim1.SetActive(true);
            stim2.SetActive(true);

            response = -1;
        });
        // this won't work with a configuration file
        //stimOn.AddTimer(stimOnDur, collectResponse);
        // use this instead 
        stimOn.SpecifyTermination(() => Time.time - stimOn.TimingInfo.StartTimeAbsolute >= stimOnDur, collectResponse);

        //Define collectResponse State
        collectResponse.AddInitializationMethod(() => goCue.SetActive(true));
        collectResponse.AddUpdateMethod(() =>
        {
            if (InputBroker.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(InputBroker.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Target")
                    {
                        response = 1;
                        numCorrect++;
                    }
                    else
                    {
                        response = 0;
                    }
                }
                else
                {
                    response = 2;
                }
            }
        });
        // this will not work with a configuration file, as responseMaxDur will be kept at the original value at time of initialization
        //collectResponse.AddTimer(responseMaxDur, feedback);
        // note that the original wiki reports collectResponse.StartTimeAbsolute, but that won't work
        collectResponse.SpecifyTermination(() => Time.time - collectResponse.TimingInfo.StartTimeAbsolute >= responseMaxDur, feedback);
        collectResponse.SpecifyTermination(() => response > -1, feedback);
        collectResponse.AddDefaultTerminationMethod(() => goCue.SetActive(false));

        //Define feedback State
        feedback.AddInitializationMethod(() =>
        {
            fb.SetActive(true);
            Color col = Color.white;
            switch (response)
            {
                case -1:
                    col = Color.grey;
                    break;
                case 0:
                    if (Random.Range(0f, 1f) > rewardProb)
                    {
                        col = Color.green;
                    }else
                    {
                        col = Color.red;
                    }
                    break;
                case 1:
                    if (Random.Range(0f, 1f) <= rewardProb)
                    {
                        col = Color.green;
                    }
                    else
                    {
                        col = Color.red;
                    }
                    break;
                case 2:
                    col = Color.black;
                    break;
            }
            fb.GetComponent<RawImage>().color = col;
        });
        // this won't work with a configuration file
        //feedback.AddTimer(fbDur, iti, () => fb.SetActive(false));
        feedback.SpecifyTermination(() => Time.time - feedback.TimingInfo.StartTimeAbsolute >= fbDur, iti, () => fb.SetActive(false));
        
        //Define iti state
        iti.AddInitializationMethod(() =>
        {
            stim1.SetActive(false);
            stim2.SetActive(false);
        });
        iti.AddTimer(itiDur, stimOn, () => { trialInBlock++; trialInExperiment++; trialData.AppendData(); trialData.WriteData(); });

        this.AddTerminationSpecification(() => trialInBlock > numTrials);
    }

    Vector3 AssignRandomPos()
    {
        return new Vector3(Random.Range(-posRange, posRange), Random.Range(-posRange, posRange), 0);
    }
}
