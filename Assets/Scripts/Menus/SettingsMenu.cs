//Daniel Oldham 
//S1903729
//Allows settings inside the settings menu to be tweaked 
//in-game via LWPR & main audio mixer 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer; 

   public void SetVolume (float volume)
   {
        audioMixer.SetFloat("Volume", volume);
   } 

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
