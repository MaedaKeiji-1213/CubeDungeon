using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    [SerializeField]float cool_time;            //画像を表示する間隔
    [SerializeField]float lifetime;             //画像が消えるまでの時間
    [SerializeField,Range(0,255)]ushort max_number;         //最大描画距離
    Queue<GameObject> after_image_queue=new Queue<GameObject>();            //画像を格納するキュー
    float left_time;            //残り時間
    SpriteRenderer sprite_renderer;

    // Start is called before the first frame update
    void Start()
    {
        sprite_renderer=GetComponent<SpriteRenderer>();
        left_time=cool_time;            //クールタイムを初期化
    }

    // Update is called once per frame
    void Update()
    {
        left_time-=Time.deltaTime;
        if(left_time<=0)
        {
            GameObject after_image=new GameObject();                        //新しい画像オブジェクトを作成
            after_image.transform.position=transform.position;              //画像オブジェクトの位置を設定
            after_image.transform.localScale=transform.localScale;          //画像オブジェクトのスケールを設定
            after_image.transform.rotation=transform.rotation;              //画像オブジェクトの回転を設定


            after_image_queue.Enqueue(after_image);                         //画像オブジェクトをキューに格納
            SpriteRenderer image_sprite_renderer=after_image.AddComponent<SpriteRenderer>();
            image_sprite_renderer.sprite=sprite_renderer.sprite;            //gazouobujeスプライトを設定します。
            image_sprite_renderer.color=sprite_renderer.color;              //画像オブジェクトの色を設定
            StartCoroutine(DisappearGradually(after_image,image_sprite_renderer,lifetime));//消滅するまでのコルーチンを呼び出
            left_time=cool_time;//残り時間を初期化

        }
    }

    void OnDestroy()
    {
        while(after_image_queue.Count>0)
        {
            //画像オブジェクトののα値を取得
            Destroy(after_image_queue.Dequeue());
        }
    }
    //残像を消滅させるコルーチン
    IEnumerator DisappearGradually(GameObject _after_image,SpriteRenderer sprite,float _lifetime)
    {
        float sprite_alpha=sprite.color.a;          //画像オブジェクトののα値を取得
        float lifetime_left=_lifetime;             //画像の残り時間を初期化
        while(sprite.color.a>0&&_lifetime>0){       //画像オブジェクトののα値が0になるまで繰り返す
            if(sprite==null)
                yield break;
            yield return null;
            lifetime_left-=Mathf.Min(Time.deltaTime,lifetime_left);

            Color sprite_color=sprite.color;
            sprite_color.a=sprite_alpha*(float)(lifetime_left/_lifetime);
            sprite.color=sprite_color;

            if(after_image_queue.Count>max_number&&after_image_queue.Dequeue()==_after_image)
                break;
        }
        Destroy(_after_image);
    }
}
