using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : Character, IExperience
{



    Rigidbody2D rb;




    Vector2 input_move_vector = Vector2.zero;       //移動の入力値を格納する変数
    Vector2 input_attack_vector = Vector2.zero;     //攻撃の入力値を格納する変数
    float timer = 0;                                //攻撃インターバルタイマー
    bool is_attack = false;                         //攻撃判定



    static int now_level = 1;
    static ulong hold_experience = 0;
    [SerializeField, ReadOnly] private ulong need_experience;        //次のレベルまでに必要な経験値           
    [SerializeField] private ulong now_experience = 0;              //現在の経験値
    [SerializeField] private uint base_experience = 1;              //基本の経験値
    [SerializeField] float move_speed = 10;                         //移動速度
    [SerializeField] float attack_interval = 1;                     //攻撃インターバル
    [SerializeField] private GameObject attack_prefab;



    StageController stage_controller;



    const float EXPERIENCE_UP_RATE = 1.5f;                            //経験値のアップレート値          



    public ulong NeedExperience
    {
        get { return need_experience; }
    }
    public ulong CurrentExperience
    {
        get { return now_experience; }
    }




    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        need_experience = NeedExperienceCalculation(Level, base_experience);
        stage_controller = GetComponent<StageController>();
        Setlevel(HoldStatus.current_level);
        now_experience = HoldStatus.current_experience;
    }




    // Update is called once per frame
    void Update()
    {
        InputControl();
        if (Input.GetKey(KeyCode.LeftShift) && input_attack_vector.x * input_attack_vector.y == 0)
            input_attack_vector = Vector2.zero;
        if (input_attack_vector != Vector2.zero)
        { Attack(); }



        timer -= Time.deltaTime;
        if (timer <= 0) { is_attack = false; }
        if (Hp <= 0)
        {
            stage_controller.ChangeScene();
            Hp = MaxHp;
        }
    }



    //値が変わった時に呼び出す
    protected override void OnValidate()
    {
        base.OnValidate();
        need_experience = NeedExperienceCalculation(Level, base_experience);



    }
    private void FixedUpdate()
    {
        MoveControl();
    }



    //入力制御
    void InputControl()
    {
        input_move_vector.x = Input.GetAxisRaw("Horizontal");
        input_move_vector.y = Input.GetAxisRaw("Vertical");
        input_attack_vector.x = Input.GetAxisRaw("AttackHorizontal");
        input_attack_vector.y = Input.GetAxisRaw("AttackVertical");
    }



    //移動制御
    void MoveControl()
    {
        //攻撃中は何もしない
        if (is_attack) { return; }
        var move_spd = rb.velocity;
        move_spd = input_move_vector;
        rb.velocity = move_spd.normalized * move_speed;
    }



    //攻撃制御
    void Attack()
    {
        float direction;
        if (is_attack) { return; }
        is_attack = true;
        rb.velocity = Vector2.zero;
        direction = Mathf.Atan2(input_attack_vector.y, input_attack_vector.x) * Mathf.Rad2Deg;     //攻撃の方向を計算
        Instantiate(attack_prefab, transform.position, Quaternion.Euler(0, 0, direction)).GetComponent<Attack>().master = gameObject;
        timer = attack_interval;
    }



    //経験値の計算
    public void ExperienceCalculation(ulong experience)
    {
        now_experience += experience;
        if (now_experience >= need_experience)
        {
            Setlevel(Level + 1);
            now_experience -= need_experience;
            need_experience = NeedExperienceCalculation(Level, base_experience);
        }
    }
    //必要な経験値を計算
    ulong NeedExperienceCalculation(float _level, float _base_experience)
    {
        return (ulong)(Mathf.Pow(EXPERIENCE_UP_RATE, _level / Mathf.Sqrt(_level)) * _base_experience);
    }
}