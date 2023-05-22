using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    
    [SerializeField]Image hp_bar;               //HPバー
    [SerializeField]Image exp_gage;             //経験値バー
    [SerializeField]TextMeshProUGUI lv_text;    //レベルテキスト
    IStatus player_status;
    PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        player_status=GameObject.Find("Player").GetComponent<IStatus>();
        controller=GameObject.Find("Player").GetComponent<PlayerController>();
    }

    //UIと情報を同期
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
