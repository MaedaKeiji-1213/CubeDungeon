using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedTurret : Enemy
{
    GameObject player;
    [SerializeField] float turn_speed;
    [SerializeField] float attack_interval;
    float interval_timer;
    [SerializeField] GameObject bullet;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player=GameObject.Find("Player");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        TurnToPlayer();
        interval_timer-=Time.deltaTime;
        if(interval_timer<0)
        {    
            Shoot();
            interval_timer=attack_interval;
        }
    }
    //攻撃するメソッド
    void Shoot()
    {
        GameObject bullet_object=Instantiate(bullet,transform.position,transform.rotation);
        if(bullet_object!=null)
            bullet_object.GetComponent<BulletBase>().OriginalEnemy=gameObject;
    }

    //プレイヤーに向けて回転するメソッド
    void TurnToPlayer()
    {
        Vector2 my_front_direction;
        my_front_direction.x=Mathf.Cos((transform.eulerAngles.z+90)*Mathf.Deg2Rad);         //敵の正面方向のベクトルを計算
        my_front_direction.y=Mathf.Sin((transform.eulerAngles.z+90)*Mathf.Deg2Rad);
        Debug.Log(my_front_direction);
        Vector2 player_direction=player.transform.position-transform.position;              //プレイヤーの方向ベクトルを計算

        Vector3 next_direction=Vector3.zero;
        next_direction.z=Mathf.Sign(my_front_direction.x*player_direction.y-my_front_direction.y*player_direction.x);           //プレイヤーに向けて回転する方向を計算
        transform.Rotate(next_direction*turn_speed);
        
    }
    
}
