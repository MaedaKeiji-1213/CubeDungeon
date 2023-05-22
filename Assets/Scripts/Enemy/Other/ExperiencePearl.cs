using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePearl : MonoBehaviour
{
    [ReadOnly]public ulong add_experience = 0;      //追加経験値
    [SerializeField] float chase_speed = 0;         //プレイヤーを追う速度
    public float leave_speed = 0f;                  //プレイヤーから離れる速度
    public float distance = 5f;                     //移動の中心点
    public Transform center_point;                  //中心点からの最大距離
    GameObject player;                              

    Character statas;
    [SerializeField] float chase_wait_time =0;      //プレイヤーを追いかけるまでの待ち時間
    float timer = 0f;                               //タイマー
    
    void Start()
    {
        player = GameObject.Find("Player");
        statas = player.GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        float lenX = GetPlObj().transform.position.x - this.transform.position.x;       //プレイヤーとの距離

        if (center_point != null)
        {
          Vector3 offset = transform.position - center_point.position;      //中心点との差文
          float currentDistance = offset.magnitude;                         //中心点との距離
            if (currentDistance < distance)
            {
                
                leave_speed = Time.deltaTime;

                transform.position += offset.normalized * leave_speed;      //中心点から離れる
            }
        }
        else
        {
            Debug.Log(timer);
            timer += Time.deltaTime;
            if (chase_wait_time <= timer)
            {
                if (Mathf.Abs(lenX) < 48)           //一定の範囲内であれば
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, chase_speed *Time.deltaTime);
                }
            }
        }

        
    }

    private GameObject GetPlObj() { return GameObject.Find("Player"); }
    private float GetPlDir()
    {
        GameObject pl = GetPlObj();
        if(pl != null)
        {
            float lexX = pl.transform.position.x - this.transform.position.x;
            //
            return lexX > 0 ? 1.0f : -1.0f;
        }
        return this.transform.localScale.x;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<IExperience>() != null)
        {
            IExperience experience;
            experience = collision.gameObject.GetComponent<IExperience>();
            experience.ExperienceCalculation(add_experience);
            Destroy(gameObject);
        }
    }
}
