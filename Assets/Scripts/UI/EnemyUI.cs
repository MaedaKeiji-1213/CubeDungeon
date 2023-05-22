using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    float hp = 0;           //Enemyの現在のHP
    float maxhp = 0;        //Enemyエネミーの最大HP
    int lv;                 //Enemyのレベル
    private Slider slider;  //HPバーを表示するためのSliderUIコンポーネント
    private Text text;      //れ弁るを表示するためのTextUIコンポーネント
    IStatus status;         //IStatusインターフェースを実装したオブジェクトの情報を格納するインターフェース型の変数
    
　　//Enemyの値を取得して代入
    void Start()
    {
        slider = GameObject.Find("HP").GetComponent<Slider>();
        text = GameObject.Find("Lv").GetComponent<Text>();
        status = GetComponent<IStatus>();
        lv = status.Level;
        maxhp = status.MaxHp;
        hp = status.Hp;
        hp /= maxhp;
        hp = (int)(slider.maxValue);
    }

    // Update is called once per frame
    void Update()
    {
        HpBar();
    }

    void HpBar()
    {
        hp = status.Hp;
        hp /= maxhp;
        slider.value = hp;
        text.text = "Lv." + lv;
    }
}
