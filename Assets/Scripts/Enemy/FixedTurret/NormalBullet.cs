using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : BulletBase
{
    [SerializeField]float move_speed;       //弾の移動速度
    Vector2 move_direction;                 //弾の移動方向
    Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        
        float original_angle=OriginalEnemy.transform.eulerAngles.z;
        move_direction.x=Mathf.Cos((original_angle+90)*Mathf.Deg2Rad);
        move_direction.y=Mathf.Sin((original_angle+90)*Mathf.Deg2Rad);

        rigidbody=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //弾の移動処理
    protected override void Move()
    {
        if(rigidbody!=null)
        {
           rigidbody.velocity=move_direction.normalized*move_speed;
        }
    }

}
