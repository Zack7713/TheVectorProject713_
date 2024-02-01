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
    public float recoilAngle;
    public float recoilRecoverySpeed;
    public float maxRecoilAngle;
    public bool isUpgraded = false;
    public int upgradeCost = 500; 
    public GameObject model;
    public ParticleSystem hitEffect;
    public AudioClip shootSound;
    // public float shootSpeed;
    [Range(0, 1)] public float shootSoundVolume;
    // Initialize original stats
    private int originalDamage;
    private float originalFireRate;
    public void Initialize()
    {
        originalDamage = shootDamage;
        originalFireRate = shootRate;
    }

    // Reset stats to original values
    public void ResetStats()
    {
        if(gunName == ("Pistol"))
        {
            shootDamage = 1;
            shootRate = 1;
        }

        shootDamage = originalDamage;
        shootRate = originalFireRate;
    }
    public void ResetPistol()
    {
        shootDamage = 1;
        shootRate = 1;
        upgradeCost = 500;
        isUpgraded = false;
    }
    public void ResetRifle()
    {
        shootDamage = 1;
        shootRate = 0.3f;
        upgradeCost = 500;
        isUpgraded = false;
    }
    public void ResetShotgun()
    {
        shootDamage = 2;
        shootRate = 2;
        upgradeCost = 500;
        isUpgraded = false;
    }
}
