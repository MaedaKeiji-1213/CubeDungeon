using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage 
{
    void AddDamage(GameObject target);
    void ReceiveDamage(GameObject target);
    public bool IsInvincible{ get;}
}
