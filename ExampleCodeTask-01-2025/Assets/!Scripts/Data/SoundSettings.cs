using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundSettings", menuName = "Settings/SoundSettings")]
public class SoundSettings : ScriptableObject
{
    [SerializeField] private List<SoundData> _data;

    public List<SoundData> Data => _data;
}
