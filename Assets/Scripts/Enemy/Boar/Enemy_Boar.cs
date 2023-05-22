using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boar : Enemy
{
    Rigidbody2D rb;
    GameObject player;
    Camera cam;

    float timer = 0;                //�_�b�V���p������
    float outer_product;            //�v���C���[�Ƃ̊O��
    float distance;             
    public float shake_duration = 0.1f;       // �V�F�C�N�̌p������
    public float shake_magnitude = 0.1f;      // �V�F�C�N�̋���

    private Vector3 originalPosition;       // �V�F�C�N�O�̓G�̈ʒu
    private Vector2 my_forward;             //�����Ă������
    private Vector2 my_right;               //�E����
    private Vector2 player_vec;             //�ʒu�x�N�g��
    Vector3 camera_view_area;               //�J�����͈̔�

    [SerializeField] private float move_speed;          //�G�̈ړ����x
    [SerializeField] private float dash_continue_time;  //�_�b�V���̌p������
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
        my_forward.y = Mathf.Sin((transform.eulerAngles.z + 90) * Mathf.Deg2Rad);       //�����Ă���������v�Z
        my_forward.x = Mathf.Cos((transform.eulerAngles.z + 90) * Mathf.Deg2Rad);

        my_right.y = Mathf.Sin((transform.eulerAngles.z ) * Mathf.Deg2Rad);         //�E�������v�Z
        my_right.x = Mathf.Cos((transform.eulerAngles.z ) * Mathf.Deg2Rad);

        player_vec = (player.transform.position - transform.position).normalized;       //�v���C���[�ƓG�̈ʒu�x�N�g�����v�Z
        outer_product = my_forward.x * player_vec .y- my_forward.y * player_vec.x;      //�v���C���[�Ƃ̊O�ς��v�Z
        distance =(player_vec - my_forward).magnitude;                                  //�v���C���[�Ƃ̋������v�Z
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
        //�J�������猩�ĉ�ʓ��ɂ���ꍇ
        if (camera_view_area.x <= 1 && camera_view_area.y <= 1)
        {
            //�V�F�C�N���łȂ���΃^�[�Q�b�g�̕���������
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
        //�G�𐳖ʂɐi�߂�
        rb.velocity = transform.up * move_speed;
    }
    void StartShake()
    {
        //�V�F�C�N�O�̈ʒu��ۑ�����
        originalPosition = transform.position;
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < shake_duration)
        {
            // �V�F�C�N�p�̃����_���ȍ��W�𐶐�
            float x = Mathf.Sign(Random.Range(-1f, 1f));

            rb.velocity = my_right*x;
            
            elapsed += Time.deltaTime;

            yield return null;
        }

        // �V�F�C�N���I��������A�J���������̈ʒu�ɖ߂�
        transform.position = originalPosition;
        state = my_state.dush;
        timer = dash_continue_time;
    }

}
