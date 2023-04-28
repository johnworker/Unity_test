using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieSounds", menuName = "Scriptables/ZombieSounds")]
public class ZombieSounds : ScriptableObject
{
    public AudioClip attack;
    public AudioClip[] basic;
    public AudioClip dying;
}
