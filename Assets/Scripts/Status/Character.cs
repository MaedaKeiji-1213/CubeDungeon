using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour,IStatus,IDamage
{
    [Header("< ステータス >")]
    [SerializeField] private int level = 0;
    [SerializeField,ReadOnly] private int max_hp = 0;   //最大HP
    [SerializeField,ReadOnly] private int hp = 0;       //現在のHP
    [SerializeField,ReadOnly] private int atk = 0;      //攻撃力               
    [SerializeField,ReadOnly] private int def = 0;      //防御力


    [Header("< 基礎ステータス >")]
    [SerializeField] int base_hp;                       //基礎HP
    [SerializeField] int base_atk;                      //基礎攻撃力
    [SerializeField] int base_def;                      //基礎防御力
    [Space(10)]
    
    [SerializeField,Tooltip("無敵時間")] float invincible_time;     //無敵時間
    [SerializeField]GameObject damage_effect;                       //ダメージエフェクト

    private bool is_undamaged=false;        //無敵判定

    const int ATTACK_BLUR_RATE = 5;         //攻撃ブレ率
    const float DAMAGE_CAT_RATE=3f;         //ダメージカット率
    

    /*IStatus*/
    public int Level
    {
        get{ return level;}
        set { level = Mathf.Max(value, 0); }
    }
    public int MaxHp 
    {
        get { return max_hp; }
    }
    public int Hp 
    {
        get { return hp; }
        set { hp = Mathf.Max(value,0); }        //現在のHPが0以下にならないようにする
    }
    public int Def
    {
        get { return def; }
    }

    /*IDamage*/
    public bool IsInvincible
    {
        get{ return is_undamaged;}
    }



    virtual protected void Start()
    {
        Setlevel(level);        //レベルに合わせてステータスを設定する
    }

    virtual protected void Update()
    {
    }

    virtual protected void OnValidate()
    {
        Setlevel(level);        //エディターでレベルが変更された場合、ステータスを設定しなおす
    }

    
    /*IStatus*/
    public void Setlevel(int set_level)
    {
        level=set_level;
        max_hp  = (int)(base_hp  + (base_hp  * level * 0.5f));          //レベルに応じた最大HPを設定する
        hp=max_hp;                                                      //現在のHPを最大HPに設定する
        atk = (int)(base_atk + (base_atk * level * 0.5f));              //レベルに応じた攻撃力を設定する
        def = (int)(base_def + (base_def * level * 0.5f));              //レベルを応じた防御力を設定する
    }

 
    /*IDamage*/
    public void AddDamage(GameObject enemy)
    {
        //enemyのステータスとダメージ処理用コンポーネントを取得
        IStatus enemy_status=enemy.GetComponent<IStatus>();
        IDamage enemy_damage=enemy.GetComponent<IDamage>();
        //敵が生きている場合、攻撃を与える
        if(enemy_status!=null&&(enemy_damage==null||!enemy_damage.IsInvincible))
        {
            //ダメージ計算に使用する値の準備
            float random_magnification=(float)Random.Range(100-ATTACK_BLUR_RATE,100+ATTACK_BLUR_RATE) / 100;//乱数ブレ
            float reduce_rate= 1 + enemy_status.Def / 100;//軽減率
            int damage_cut=(int)Mathf.Pow(enemy_status.Def,1/DAMAGE_CAT_RATE);//ダメージカット

            //敵のHPを減らす
            enemy_status.Hp -=Mathf.Max((Mathf.CeilToInt(atk*random_magnification/reduce_rate)-damage_cut),0);
            //敵にダメージを与える処理があれば実行
            if(enemy_damage!=null)
                enemy_damage.ReceiveDamage(gameObject);

            //ダメージテキスト表示用コンポーネントを取得
            DamageT damageT =enemy.GetComponent<DamageT>();
            //ダメージテキストが存在する場合、表示
            if (damageT != null)
            {
                StartCoroutine(damageT.GenerateDamageText(Mathf.Max((Mathf.CeilToInt(atk * random_magnification / reduce_rate) - damage_cut), 0)));
            }

        }
    }
    public void ReceiveDamage(GameObject enemy)
    {
        //ノックバック処理用コンポーネントを取得
        IKnockBack knock_back=GetComponent<IKnockBack>();
        //ノックバックが可能な場合、実行
        if(knock_back!=null)knock_back.KnockBack(enemy.transform);
        //ダメージエフェクトが存在する場合、表示
        if(damage_effect!=null)
            Destroy(Instantiate(damage_effect,transform.position,Quaternion.identity),2f);
        //無敵時間処理開始
        StartCoroutine(DamageCoolTime(invincible_time));
    }
    

    IEnumerator DamageCoolTime(float time)
    {
        //無敵時間に入る
        is_undamaged=true;
        //指定時間待機
        yield return new WaitForSeconds(time);
        //無敵時間終了
        is_undamaged=false;
    }
}




