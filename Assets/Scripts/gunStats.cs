using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]


public class gunStats : ScriptableObject
{
    public string gunName;
    public int shootDamage;
    public float shootRate;
    public float shootSpread;
    public int shootDist;
    public int gunPellets;
    public int ammoCur;
    public int ammoMax;

    public GameObject model;
    public ParticleSystem hitEffect;
    public AudioClip shootSound;
    // public float shootSpeed;
    [Range(0, 1)] public float shootSoundVolume;
}
