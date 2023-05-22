using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boar : Enemy
{
    Rigidbody2D rb;
    GameObject player;
    Camera cam;

    float timer = 0;                //ダッシュ継続時間
    float outer_product;            //プレイヤーとの外積
    float distance;             
    public float shake_duration = 0.1f;       // シェイクの継続時間
    public float shake_magnitude = 0.1f;      // シェイクの強さ

    private Vector3 originalPosition;       // シェイク前の敵の位置
    private Vector2 my_forward;             //向いている方向
    private Vector2 my_right;               //右方向
    private Vector2 player_vec;             //位置ベクトル
    Vector3 camera_view_area;               //カメラの範囲

    [SerializeField] private float move_speed;          //敵の移動速度
    [SerializeField] private float dash_continue_time;  //ダッシュの継続時間
    enum my_state
    {
        turn ,
        shake,
        dush
    }
    my_state state = my_state.turn;
    override protected void Start()
    {
        base.Start();
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    override protected void Update()
    {
        base.Update();
        timer -= Time.deltaTime;
        my_forward.y = Mathf.Sin((transform.eulerAngles.z + 90) * Mathf.Deg2Rad);       //向いている方向を計算
        my_forward.x = Mathf.Cos((transform.eulerAngles.z + 90) * Mathf.Deg2Rad);

        my_right.y = Mathf.Sin((transform.eulerAngles.z ) * Mathf.Deg2Rad);         //右方向を計算
        my_right.x = Mathf.Cos((transform.eulerAngles.z ) * Mathf.Deg2Rad);

        player_vec = (player.transform.position - transform.position).normalized;       //プレイヤーと敵の位置ベクトルを計算
        outer_product = my_forward.x * player_vec .y- my_forward.y * player_vec.x;      //プレイヤーとの外積を計算
        distance =(player_vec - my_forward).magnitude;                                  //プレイヤーとの距離を計算
        if(0 < timer)
        {
            Dash();
        }
        else if(timer <= 0)
        {
            
            state = my_state.turn;
            Turn();
        }
    }

    void Turn()
    {
        rb.velocity = Vector2.zero;
        camera_view_area = cam.WorldToViewportPoint(transform.position);
        //カメラから見て画面内にいる場合
        if (camera_view_area.x <= 1 && camera_view_area.y <= 1)
        {
            //シェイク中でなければターゲットの方向を向く
            if (state != my_state.shake)
            {
                if (outer_product < 0)
                {
                    transform.Rotate(0, 0, outer_product);
                }
                if (0 < outer_product)
                {
                    transform.Rotate(0, 0, outer_product);
                }
                if (distance < 0.1f)
                {
                    state = my_state.shake;
                    StartShake();
                }
            }
        }
    }

    void Dash()
    {
        //敵を正面に進める
        rb.velocity = transform.up * move_speed;
    }
    void StartShake()
    {
        //シェイク前の位置を保存する
        originalPosition = transform.position;
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < shake_duration)
        {
            // シェイク用のランダムな座標を生成
            float x = Mathf.Sign(Random.Range(-1f, 1f));

            rb.velocity = my_right*x;
            
            elapsed += Time.deltaTime;

            yield return null;
        }

        // シェイクが終了したら、カメラを元の位置に戻す
        transform.position = originalPosition;
        state = my_state.dush;
        timer = dash_continue_time;
    }

}
