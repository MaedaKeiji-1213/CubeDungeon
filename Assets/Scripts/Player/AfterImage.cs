using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    [SerializeField]float cool_time;            //�摜��\������Ԋu
    [SerializeField]float lifetime;             //�摜��������܂ł̎���
    [SerializeField,Range(0,255)]ushort max_number;         //�ő�`�拗��
    Queue<GameObject> after_image_queue=new Queue<GameObject>();            //�摜���i�[����L���[
    float left_time;            //�c�莞��
    SpriteRenderer sprite_renderer;

    // Start is called before the first frame update
    void Start()
    {
        sprite_renderer=GetComponent<SpriteRenderer>();
        left_time=cool_time;            //�N�[���^�C����������
    }

    // Update is called once per frame
    void Update()
    {
        left_time-=Time.deltaTime;
        if(left_time<=0)
        {
            GameObject after_image=new GameObject();                        //�V�����摜�I�u�W�F�N�g���쐬
            after_image.transform.position=transform.position;              //�摜�I�u�W�F�N�g�̈ʒu��ݒ�
            after_image.transform.localScale=transform.localScale;          //�摜�I�u�W�F�N�g�̃X�P�[����ݒ�
            after_image.transform.rotation=transform.rotation;              //�摜�I�u�W�F�N�g�̉�]��ݒ�


            after_image_queue.Enqueue(after_image);                         //�摜�I�u�W�F�N�g���L���[�Ɋi�[
            SpriteRenderer image_sprite_renderer=after_image.AddComponent<SpriteRenderer>();
            image_sprite_renderer.sprite=sprite_renderer.sprite;            //gazouobuje�X�v���C�g��ݒ肵�܂��B
            image_sprite_renderer.color=sprite_renderer.color;              //�摜�I�u�W�F�N�g�̐F��ݒ�
            StartCoroutine(DisappearGradually(after_image,image_sprite_renderer,lifetime));//���ł���܂ł̃R���[�`�����Ăяo
            left_time=cool_time;//�c�莞�Ԃ�������

        }
    }

    void OnDestroy()
    {
        while(after_image_queue.Count>0)
        {
            //�摜�I�u�W�F�N�g�̂̃��l���擾
            Destroy(after_image_queue.Dequeue());
        }
    }
    //�c�������ł�����R���[�`��
    IEnumerator DisappearGradually(GameObject _after_image,SpriteRenderer sprite,float _lifetime)
    {
        float sprite_alpha=sprite.color.a;          //�摜�I�u�W�F�N�g�̂̃��l���擾
        float lifetime_left=_lifetime;             //�摜�̎c�莞�Ԃ�������
        while(sprite.color.a>0&&_lifetime>0){       //�摜�I�u�W�F�N�g�̂̃��l��0�ɂȂ�܂ŌJ��Ԃ�
            if(sprite==null)
                yield break;
            yield return null;
            lifetime_left-=Mathf.Min(Time.deltaTime,lifetime_left);

            Color sprite_color=sprite.color;
            sprite_color.a=sprite_alpha*(float)(lifetime_left/_lifetime);
            sprite.color=sprite_color;

            if(after_image_queue.Count>max_number&&after_image_queue.Dequeue()==_after_image)
                break;
        }
        Destroy(_after_image);
    }
}
