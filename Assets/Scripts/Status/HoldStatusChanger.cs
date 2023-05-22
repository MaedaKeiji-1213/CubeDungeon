using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HoldStatusChanger : MonoBehaviour
{
    IStatus status;
    PlayerController player;
    private void Awake ()
    {
        GameObject player_object = GameObject.Find("Player");
        HoldStatus.currentScene = SceneManager.GetActiveScene().name;
        status = player_object.GetComponent<IStatus>();
        player = player_object.GetComponent<PlayerController>();
    }
    public void StatusChange()
    {
        HoldStatus.current_level = status.Level;
        HoldStatus.current_experience = player.CurrentExperience;
    }

    public void StatusReset()
    {
        HoldStatus.current_level = 1;
        HoldStatus.current_experience = 0;
    }
}


static class HoldStatus
{
    static public string currentScene;
    static public int current_level = 1;
    static public ulong current_experience = 0;
}

