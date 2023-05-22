using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Enemy : Character,IKnockBack
{
    //敵がノックバックを受けた時に小さい力と大きい力を設定
    [SerializeField] float knock_back_force_mini;
    [SerializeField] float knock_back_force_big;
    [HideInInspector] public float knockback_left_time=0;

    [SerializeField,Tooltip("ノックバックの回数")] float knock_back_times;
    float knock_back_hp;

    //経験値に関する変数
    [Header("経験値")]
    [SerializeField,ReadOnly] uint holding_experience;          //所持している経験値
    [SerializeField] int base_holding_experience;               //経験値の基本値
    // Start is called before the first frame update

    GameObject player_object;
    private Rigidbody2D rigidbody;
    private Collider2D target_collider; // 対象となるCollider2Dをインスペクタからアサイン

    //定数の定義
    const string PLAYER_OBJECT_NAME="Player";                   //プレイヤーの名前
    const string EXPERIENCE_PEARL_NAME="Exp";                   //経験値オーブの名前

    const float EXPERIENCE_UP_RATE=1.2f;                        //経験値の成長率



    
    override protected void Start()
    {
        base.Start();
        
        if(player_object==null)player_object=GameObject.Find(PLAYER_OBJECT_NAME);
        if(target_collider==null)target_collider=GetComponent<Collider2D>();
        if(rigidbody==null)rigidbody=GetComponent<Rigidbody2D>();

        //ノックバックを受けるために必要なHPを設定
        SetKnockBackHp();
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();

        knockback_left_time -=Mathf.Min(Time.deltaTime);
        if(player_object==null)player_object=GameObject.Find(PLAYER_OBJECT_NAME);
        if(target_collider==null)target_collider=GetComponent<Collider2D>();
        if(rigidbody==null)rigidbody=GetComponent<Rigidbody2D>();

        if(base.Hp<=0)
        {
            Dead();
        }

    }
　　
    //経験値を計算するメソッド
    protected override void OnValidate()
    {
        base.OnValidate();
        holding_experience=(uint)NeedExperienceCalculation(Level,(float)base_holding_experience);
        
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if(col.gameObject.GetComponent<Enemy>()==null)
            AddDamage(col.gameObject);
    }

    void OnTriggerStay2D(Collider2D col)

    {
        //Enemyコンポーネントを持っていない場合、攻撃
        if (col.gameObject.GetComponent<Enemy>() == null)

            AddDamage(col.gameObject);

    }
    //死亡時の処理
    void Dead ()
    {
        IStatus player_status=player_object.GetComponent<IStatus>();
        //プレイヤーのステータスが存在する場合
        if(player_status!=null)
        {
            int exp=(int)((holding_experience)*Level*2/(player_status.Level+Level));
            GenerateRandomPoints(holding_experience);
        }       
        GetComponent<SpriteRenderer>().enabled=false;
        target_collider.enabled=false;
        this.enabled=false;
        Destroy(Instantiate(Resources.Load("EnemyDeadEffect"),transform.position,Quaternion.identity),2f); 
        Destroy(transform.parent.gameObject);
    }
    //経験値オーブを生成する
    private void GenerateRandomPoints(uint exp)
    {
        int count= 20;
        GameObject exp_prefab=Resources.Load<GameObject>(EXPERIENCE_PEARL_NAME);
            Debug.Log("ExpPop"+exp);
        while (0 < exp)
        {
            count--;
            int exp_orb_point=(int)Mathf.Pow(10,(int)Mathf.Log10(exp));
            if(exp-exp_orb_point<count)
                exp_orb_point/=10;
            Vector2 random_point = GetRandomPointInCollider();
            GameObject point = Instantiate(exp_prefab, random_point, Quaternion.identity);
            point.transform.localScale*=(float)(1+Mathf.Log10(exp_orb_point)/10);
            var exp_orb=point.GetComponent<ExperiencePearl>();
            exp_orb.add_experience=(ulong)exp_orb_point;
            exp_orb.center_point=this.transform;
            exp-=(uint)exp_orb_point;
        }
    }

    //コライダー内のランダムな位置を取得する
    private Vector2 GetRandomPointInCollider()
    {
        //もしも対象となるコライダーがnullの場合、(0,0)座標を返して処理を終了する
        if(target_collider==null)return Vector2.zero;
        
        //対象となるコライダーの境界を取得する
        Bounds bounds = target_collider.bounds;
        Vector2 random_point;

        do
        {
            //境界内のランダムなx,y座標を生成する
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            random_point = new Vector2(x, y);
        }
        //ランダムに生成された座標が、対象となるコライダー内に存在する限り、繰り返し座標を生成する
        while (!target_collider.OverlapPoint(random_point));
        //コライダー内にランダムな座標を返す

        return random_point;
    }

    public void KnockBack(Transform attacking_character)
    {
        //ノックバックの力の大きさを、初期値として小さい力を設定する
        float force_magnitude=knock_back_force_mini;
        //ノックバックの力を大きさを、HPが一定の値より小さい場合、大きい力に設定し、KnockBackHpを設定する
        if(knock_back_hp>Hp)
        {
            force_magnitude=knock_back_force_big;
            SetKnockBackHp();
        }
        //攻撃されたキャラクターから地震への力の向きを、位置の差分から算出する
        Vector2 force_direction=(transform.position-attacking_character.position).normalized;
        //ノックバックを発生させる
        rigidbody.AddForce(force_direction*force_magnitude);
        knockback_left_time = 0.5f;
        //ノックバックの際に点滅させるエフェクトを、フラッシュ時間を指定して、コルーチンで実行する
        StartCoroutine(ReverseColorFlashing(0.02f,0.3f));

    }
    void SetKnockBackHp()
    {
        //一定の間隔でHP値を超えない範囲で、KnockBackHpの値を更新する
        knock_back_hp=Hp-(Hp-1)%(MaxHp/knock_back_times);
    }

    IEnumerator ReverseColorFlashing(float reverseing_time,float flashing_time)
    {
        //SpriteRendererコンポーネントを取得し、フラッシュの逆転時間と初期状態を設定する
        var sprite_renderer=GetComponent<SpriteRenderer>();
        float left_time=reverseing_time;
        bool is_reversing=true;

        //スプライトレンダラーが存在しない、もしくはマテリアルにThresholdプロパティが存在しない場合、コルーチンを終了
        if(sprite_renderer==null||!sprite_renderer.material.HasProperty("_Threshold")) yield break;
        while(flashing_time>0)
        {
            if(left_time<=0)
            {
                is_reversing=!is_reversing;
                left_time=reverseing_time;
            }
            //リバースフラグに応じてThresholdプロパティを設定する
            sprite_renderer.material.SetFloat("_Threshold",is_reversing?1:0);
            yield return null;
            flashing_time-=Time.deltaTime;
            flashing_time-=Time.deltaTime;
        }
        //Thresholdプロパティが0より大きい場合、0に設定する
        if(sprite_renderer.material.GetFloat("_Threshold")>0)
            sprite_renderer.material.SetFloat("_Threshold",0);
    }
    ulong NeedExperienceCalculation(float _level,float _base_experience)
    {

        return (ulong)(Mathf.Pow(EXPERIENCE_UP_RATE, _level/Mathf.Sqrt(_level)) * _base_experience);

    }

}
