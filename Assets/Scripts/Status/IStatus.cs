using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatus
{
    public int Level { get; set; }
    public int MaxHp    { get;}
    public int Hp    { get;set;}
    public int Def   { get;}

    public void Setlevel(int level);//name:calculation->setLevel

}
