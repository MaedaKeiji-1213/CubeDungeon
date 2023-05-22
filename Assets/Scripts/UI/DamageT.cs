using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageT : MonoBehaviour
{
    // Start is called before the first frame update
    //ダメージテキストのプレハブオブジェクト
    GameObject prefab ;
    void Start()
    {
        //プレハブを李楚洲からロードする
        prefab=Resources.Load<GameObject>("DamageText");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //ダメージテキストの生成
    public IEnumerator GenerateDamageText(int damage)
    {

        GameObject damage_object= Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(damage_object,0.3f);
        //ダメージ数値を設定する
        Text text = damage_object.transform.GetChild(0).gameObject.GetComponent<Text>();
        text.text = damage.ToString();
        //敵の場合はテキストの色を赤に設定する
        if (GetComponent<Enemy>() != null)
        {
            text.color = Color.red;

        }        
        
        
        
        float limit_time=0.3f;
        while(limit_time>0&&damage_object!=null)
        {
            damage_object.transform.Translate(0,10.0f *Time.deltaTime,0);
            yield return null;
            limit_time-=Time.deltaTime;
        }
    }
}
