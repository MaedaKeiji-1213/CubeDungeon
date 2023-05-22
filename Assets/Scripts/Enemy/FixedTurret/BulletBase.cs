using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//抽象クラスのBulletBaseを宣言
abstract public class BulletBase:MonoBehaviour
{
    GameObject originalEnemy;           //弾を発射した敵の情報を保存する

    //弾を発射した敵の情報を取得・設定するプロパティ
    public GameObject OriginalEnemy{
        get{
            return originalEnemy;
        }
        set{
            //originalEnemyがnullの場合valueを設定する
            if(originalEnemy==null)
                originalEnemy=value;
        }
    }

    //弾の移動を実装するメソッド
    abstract protected void Move(); 

    void OnTriggerStay2D(Collider2D col)
    {
        //originalENemyが存在し、当たったオブジェクトがoriginalEnemy以外の場合
        if(originalEnemy!=null&&col.gameObject!=originalEnemy&&!col.isTrigger)
        {
            originalEnemy.GetComponent<IDamage>().AddDamage(col.gameObject);
            DestroyMe();
        }
    }

    //弾自身を破棄するメソッド
    void DestroyMe()
    {
        Destroy(gameObject);
    }

}
