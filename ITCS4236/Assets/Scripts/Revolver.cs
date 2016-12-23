using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Revolver : MonoBehaviour, Gun
{
    public static readonly float DEFAULT_RELOAD_SPEED = 2f;

    private static readonly Vector3 OriginalLocation = new Vector3(0.16f, -0.19f, 0.32f);
    private static readonly Vector3 AimLocation      = new Vector3(0.0f, -0.15f, 0.25f);
    private static readonly Vector3 ReloadLocation   = new Vector3(0.0f, -1.0f, 0.0f);

    public string weaponName = "Revolver";
    public int maxClipSize = 6;
    public int currentClipSize = 6;
    public int currentAmmo = 30;   
    public int maxAmmo = 30;
    public int index = 0;
    public float reloadSpeed = 2f;
    public float damage = 30.0f;
    public float range = Room.LENGTH * Mathf.Sqrt(2);
    public String state = "DEFAULT";
    public AudioSource source;
    public AudioClip shootSound;
    public AudioClip reloadSound;

    public Revolver(Vector3 location, string resourcePath)
    {
        GameObject.Instantiate(Resources.Load(resourcePath), location, new Quaternion());
    }

    void Awake()
    {
        weaponName = "Revolver";
        currentClipSize = 6;
        maxClipSize = 6;
        currentClipSize = maxClipSize;
        currentAmmo = 30;
        maxAmmo = 30;
        reloadSpeed = 2f;
        index = 0;
        damage = 30.0f;
        range = Room.LENGTH * Mathf.Sqrt(2);
        state = "DEFAULT";
        source = GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        if (currentClipSize > 0)
        {
            source.PlayOneShot(shootSound);

            --currentClipSize;

            RaycastHit shotHit = new RaycastHit();
            if (Physics.Raycast(transform.position, transform.TransformDirection(Quaternion.Euler(0, -90, 0) * Vector3.forward), out shotHit))
            {
                if (shotHit.distance < range)
                {
                    Debug.DrawLine(transform.position, shotHit.point, Color.red, 100.0f, false);

                    shotHit.transform.gameObject.SendMessage("DeductHealth", damage, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }

    public void Reload()
    {
        if (currentClipSize != maxClipSize && currentAmmo != 0)
            StartCoroutine(ReloadTimer());
    }

    public void Aim(Boolean isAim)
    {
        UIManager.crosshair.enabled = !isAim;
        state = isAim ? "AIM" : "DEFAULT";
    }

    public string GetWeaponName()
    {
        return weaponName;
    }

    public int GetClipSize()
    {
        return currentClipSize;
    }

    public int GetMaxClipSize()
    {
        return maxClipSize;
    }

    public int GetAmmo()
    {
        return currentAmmo;
    }

    public int GetMaxAmmo()
    {
        return maxAmmo;
    }

    public float GetReloadSpeed()
    {
        return reloadSpeed;
    }

    public float GetDamage()
    {
        return damage;
    }

    public float GetRange()
    {
        return range;
    }

    public string GetState()
    {
        return state;
    }

    public Vector3 GetDefaultPos()
    {
        return OriginalLocation;
    }

    public Vector3 GetAimingPos()
    {
        return AimLocation;
    }

    public Vector3 GetReloadLocation()
    {
        return ReloadLocation;
    }

    public void SetClipSize(int clipSize, bool upgrade = false)
    {
        this.currentClipSize = upgrade ? (this.currentClipSize + clipSize) : clipSize;
    }

    public void SetMaxClipSize(int maxClipSize, bool upgrade = false)
    {
        this.maxClipSize = upgrade ? (this.maxClipSize + maxClipSize) : maxClipSize;
    }

    public void SetAmmo(int ammo, bool upgrade = false)
    {
        this.currentAmmo = upgrade ? (this.currentAmmo + ammo) : ammo;
    }

    public void SetMaxAmmo(int maxAmmo, bool upgrade = false)
    {
        this.maxAmmo = upgrade ? (this.maxAmmo + maxAmmo) : maxAmmo;
    }

    public void SetReloadSpeed(float reloadSpeed, bool upgrade = false)
    {
        this.reloadSpeed = upgrade ? (this.reloadSpeed + reloadSpeed) : reloadSpeed;
    }

    public void SetDamage(float damage, bool upgrade = false)
    {
        this.damage = upgrade ? (this.damage + damage) : damage;
    }

    public void SetRange(float range, bool upgrade = false)
    {
        this.range = upgrade ? (this.range + range) : range;
    }

    public void SetState(string state)
    {
        this.state = state;
    }

    public void SetDefaultState()
    {
        this.state = "DEFAULT";
        transform.localPosition = OriginalLocation;
    }

    public IEnumerator ReloadTimer()
    {
        state = "RELOAD";

        transform.localPosition = ReloadLocation;

        source.pitch = DEFAULT_RELOAD_SPEED / reloadSpeed;

        source.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(reloadSpeed);

        int amountToFull = maxClipSize - currentClipSize;

        if (currentAmmo >= amountToFull)
        {
            currentAmmo -= amountToFull;
            currentClipSize = maxClipSize;
        }
        else
        {
            currentClipSize += currentAmmo;
            currentAmmo = 0;
        }

        state = "DEFAULT";

        //transform.localPosition = OriginalLocation;
        GunController.ReloadReadyToLerpBack = true;

        source.pitch = 1;
    }

    public override string ToString()
    {
        return "weaponName=" + weaponName +
            "\nmaxClipSize=" + maxClipSize +
            "\ncurrentClipSize=" + currentClipSize +
            "\ncurrentAmmo=" + currentAmmo +
            "\nmaxAmmo=" + maxAmmo +
            "\nreloadSpeed=" + reloadSpeed +
            "\nindex=" + index +
            "\ndamage=" + damage +
            "\nrange=" + range +
            "\nstate=" + state;
    }
}
