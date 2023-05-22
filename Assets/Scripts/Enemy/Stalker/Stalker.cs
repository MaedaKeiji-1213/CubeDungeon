using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : Enemy
{
    [SerializeField] private Transform childObject; //�q�I�u�W�F�N�g��Transform�R���|�[�l���g���i�[����ϐ�
    Transform player; // �v���C���[�I�u�W�F�N�g�̃R���|�[�l���gTransform���i�[����ϐ�
    [SerializeField] float moveSpeed = 5f; //�ړ����x�𒲐�����ϐ�

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

    //�v���C���[�Ɍ������Ĉړ����郁�\�b�h
    private void Move()
    {
        if (knockback_left_time <= 0)
        {
            //�v���C���[�Ǝ��g�̈ʒu�Ƃ̍������擾���Đ��K�������x�N�g�����i�[
            Vector2 direction = (player.position - transform.position).normalized;
            //�t���[�����[�g�Ɉˑ����Ȃ��ړ�������
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

        }
    }
    //�q�I�u�W�F�N�g����]�����郁�\�b�h
    private void Child()
    {
        childObject.Rotate(Vector3.forward, Time.deltaTime * 1000f);
    }
}
