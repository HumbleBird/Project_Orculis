using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Voice;

public class VoiceScript : MonoBehaviour
{
    public AppVoiceExperience voiceExperience;

    // Update is called once per frame
    void Update()
    {
        //if (OVRInput.GetUp(OVRInput.Button.One))
        if (Input.GetKey(KeyCode.A))
        {
            voiceExperience.Activate();
        }
    }
}