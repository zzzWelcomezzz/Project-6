using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory
{
    public int ScrapCount { get; private set; }
    public List<Gun> Guns { get; private set; }

    public Inventory()
    {
        ScrapCount = 0;
        Guns = new List<Gun>();

        AddAllGuns();        
    }

    public Inventory(Inventory inventory)
    {
        ScrapCount = inventory.ScrapCount;
        Guns = new List<Gun>();

        AddAllGuns();
    }

    //bad, fix later
    public void ReduceGunReload(string name, float multiplier)
    {
        bool found = false;
        foreach (Gun gun in Guns)
        {
            if (gun.GetWeaponName() == name)
            {
                gun.SetReloadSpeed(multiplier * gun.GetReloadSpeed(), false);
                found = true;
            }
        }

        if (!found)
            Debug.LogWarning(string.Format("Warning: No gun found with name {0}, unable to add ammo", name));
    }

    // bad, fix later
    public void AddGunDamage(string name, float multiplier)
    {
        bool found = false;
        foreach (Gun gun in Guns)
        {
            if (gun.GetWeaponName() == name)
            {
                gun.SetDamage(multiplier * gun.GetDamage(), false);
                found = true;
            }
        }

        if (!found)
            Debug.LogWarning(string.Format("Warning: No gun found with name {0}, unable to add ammo", name));
    }

    // bad, fix later
    public void AddGunAmmo(string name, int amount)
    {
        bool found = false;
        foreach(Gun gun in Guns)
        {
            if (gun.GetWeaponName() == name)
            {
                gun.SetAmmo(amount, true);

                if (gun.GetAmmo() > gun.GetMaxAmmo())
                    gun.SetMaxAmmo(gun.GetAmmo(), false);                

                found = true;
            }
        }

        if (!found)
            Debug.LogWarning(string.Format("Warning: No gun found with name {0}, unable to add ammo", name));
    }

    private void AddAllGuns()
    {
        bool anyError = false;

        GameObject reGo = GameObject.Find("Revolver"), raGo = GameObject.Find("RayGun");        

        Gun revolver = null, raygun = null;

        if (reGo != null)
            revolver = reGo.GetComponent<Revolver>();

        if (raGo != null)
            raygun = raGo.GetComponent<RayGun>();

        if (revolver != null)
            PickUpGun(revolver);
        else
            anyError = true;

        if (raygun != null)
            PickUpGun(raygun);
        else
            anyError = true;

        if (anyError)
            Debug.LogError("ERROR: A GUN WAS NULL, UNABLE TO ADD. :'( " + Guns.Count);
    }

    public void PickUpGun(Gun gun)
    {
        Guns.Add(gun);
    }

    public void DropGun(Gun gun)
    {
        Guns.Remove(gun);
        // TODO actually put gun on ground?
    }

    public void AddScrap(int amount)
    {
        ScrapCount += amount;
    }

    public void SpendScrap(int amount)
    {
        ScrapCount -= amount;
    }
}
