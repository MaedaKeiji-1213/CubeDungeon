using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    float hp = 0;           //Enemy�̌��݂�HP
    float maxhp = 0;        //Enemy�G�l�~�[�̍ő�HP
    int lv;                 //Enemy�̃��x��
    private Slider slider;  //HP�o�[��\�����邽�߂�SliderUI�R���|�[�l���g
    private Text text;      //��ق��\�����邽�߂�TextUI�R���|�[�l���g
    IStatus status;         //IStatus�C���^�[�t�F�[�X�����������I�u�W�F�N�g�̏����i�[����C���^�[�t�F�[�X�^�̕ϐ�
    
�@�@//Enemy�̒l���擾���đ��
    void Start()
    {
        slider = GameObject.Find("HP").GetComponent<Slider>();
        text = GameObject.Find("Lv").GetComponent<Text>();
        status = GetComponent<IStatus>();
        lv = status.Level;
        maxhp = status.MaxHp;
        hp = status.Hp;
        hp /= maxhp;
        hp = (int)(slider.maxValue);
    }

    // Update is called once per frame
    void Update()
    {
        HpBar();
    }

    void HpBar()
    {
        hp = status.Hp;
        hp /= maxhp;
        slider.value = hp;
        text.text = "Lv." + lv;
    }
}
