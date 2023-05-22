using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageT : MonoBehaviour
{
    // Start is called before the first frame update
    //�_���[�W�e�L�X�g�̃v���n�u�I�u�W�F�N�g
    GameObject prefab ;
    void Start()
    {
        //�v���n�u�𗛑^�F���烍�[�h����
        prefab=Resources.Load<GameObject>("DamageText");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //�_���[�W�e�L�X�g�̐���
    public IEnumerator GenerateDamageText(int damage)
    {

        GameObject damage_object= Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(damage_object,0.3f);
        //�_���[�W���l��ݒ肷��
        Text text = damage_object.transform.GetChild(0).gameObject.GetComponent<Text>();
        text.text = damage.ToString();
        //�G�̏ꍇ�̓e�L�X�g�̐F��Ԃɐݒ肷��
        if (GetComponent<Enemy>() != null)
        {
            text.color = Color.red;

        }        
        
        
        
        float limit_time=0.3f;
        while(limit_time>0&&damage_object!=null)
        {
            damage_object.transform.Translate(0,10.0f *Time.deltaTime,0);
            yield return null;
            limit_time-=Time.deltaTime;
        }
    }
}
