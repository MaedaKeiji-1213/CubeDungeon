using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    
    [SerializeField]Image hp_bar;               //HP�o�[
    [SerializeField]Image exp_gage;             //�o���l�o�[
    [SerializeField]TextMeshProUGUI lv_text;    //���x���e�L�X�g
    IStatus player_status;
    PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        player_status=GameObject.Find("Player").GetComponent<IStatus>();
        controller=GameObject.Find("Player").GetComponent<PlayerController>();
    }

    //UI�Ə��𓯊�
    void Update()
    {
        if(player_status!=null&&controller!=null)
        {
            hp_bar.fillAmount=(float)player_status.Hp/(float)player_status.MaxHp;
            exp_gage.fillAmount=(float)controller.CurrentExperience/(float)controller.NeedExperience;
            lv_text.text=player_status.Level.ToString();

        }
    }
}
