using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    MASTER,
    BGM,
    SFX,
    LOOPSFX,
    GUN,
    MaxCount
}

[CreateAssetMenu(menuName = "SO/Audio")]
public class AudioSO : ScriptableObject
{
    public string audioName;
    public AudioType audioType;
    public AudioClip clip;
}