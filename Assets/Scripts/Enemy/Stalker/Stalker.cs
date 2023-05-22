using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : Enemy
{
    [SerializeField] private Transform childObject; //子オブジェクトのTransformコンポーネントを格納する変数
    Transform player; // プレイヤーオブジェクトのコンポーネントTransformを格納する変数
    [SerializeField] float moveSpeed = 5f; //移動速度を調整する変数

    Rigidbody2D rb;

     protected override void Start()
    {
        player=GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Move();
        Child();
    }

    //プレイヤーに向かって移動するメソッド
    private void Move()
    {
        if (knockback_left_time <= 0)
        {
            //プレイヤーと自身の位置との差分を取得して正規化したベクトルを格納
            Vector2 direction = (player.position - transform.position).normalized;
            //フレームレートに依存しない移動をする
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

        }
    }
    //子オブジェクトを回転させるメソッド
    private void Child()
    {
        childObject.Rotate(Vector3.forward, Time.deltaTime * 1000f);
    }
}
