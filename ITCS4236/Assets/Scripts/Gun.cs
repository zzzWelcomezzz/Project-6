using UnityEngine;
using System.Collections;

public interface Gun
{
    void Shoot();
    void Reload();
    void Aim(bool isAim);
    string GetWeaponName();
    int GetClipSize();
    int GetMaxClipSize();
    int GetAmmo();
    int GetMaxAmmo();
    float GetReloadSpeed();
    float GetDamage();
    float GetRange();
    string GetState();
    Vector3 GetDefaultPos();
    Vector3 GetAimingPos();
    Vector3 GetReloadLocation();
    void SetClipSize(int clipSize, bool upgrade);
    void SetMaxClipSize(int maxClipSize, bool upgrade);
    void SetAmmo(int ammo, bool upgrade);
    void SetMaxAmmo(int maxAmmo, bool upgrade);
    void SetReloadSpeed(float reloadSpeed, bool upgrade);
    void SetDamage(float damage, bool upgrade);
    void SetRange(float range, bool upgrade);
    void SetState(string state);
    void SetDefaultState();
    IEnumerator ReloadTimer();
    string ToString();
}
