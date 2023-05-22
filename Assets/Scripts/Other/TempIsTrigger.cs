using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using UnityEngine.Events; 

[Serializable] public class UnityEvent_Collider:UnityEvent<Collider2D>{};
public class TempIsTrigger : MonoBehaviour
{

    //[SerializeField] UnityEvent on_trigger_enter_event;
    [SerializeField] UnityEvent_Collider on_trigger_enter_event_collider;
    //[SerializeField] UnityEvent on_trigger_stay_event;
    [SerializeField] UnityEvent_Collider on_trigger_stay_event_collider;
    //[SerializeField] UnityEvent on_trigger_exit_event;
    [SerializeField] UnityEvent_Collider on_trigger_exit_event_collider;
    [SerializeField] bool is_it_other_tag;
    [SerializeField] List<string> trigger_tags;
    [SerializeField] bool is_it_other_layer;
    [SerializeField] List<int> trigger_layers;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        
        bool is_valid=false;
        is_valid=TagCheck(col.tag)?true:is_valid;
        is_valid=LayerCheck(col.gameObject.layer)?true:is_valid;

        if(is_valid){
            Debug.Log("ontriggerenter"+col);
            on_trigger_enter_event_collider.Invoke(col);
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        bool is_valid=false;
        is_valid=TagCheck(col.tag)?true:is_valid;
        is_valid=LayerCheck(col.gameObject.layer)?true:is_valid;

        if(is_valid){
            Debug.Log("ontriggerenter"+col);
            on_trigger_stay_event_collider.Invoke(col);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        bool is_valid=false;
        is_valid=TagCheck(col.tag)?true:is_valid;
        is_valid=LayerCheck(col.gameObject.layer)?true:is_valid;

        if(is_valid){
            Debug.Log("ontriggerenter"+col);
            on_trigger_exit_event_collider.Invoke(col);
        }
    }

    bool TagCheck(string collider_tag)
    {
        bool is_included=false;
        //if()
        
        foreach(string _tag in trigger_tags)
        {
            if(_tag==collider_tag)
            {
                is_included=true;
                break;
            }
        }
        if(is_it_other_tag)
            is_included=!is_included;
        return is_included;
    }
    bool LayerCheck(int collider_layer)
    {
        bool is_included=false;
        foreach(int layer in trigger_layers)
        {
            if(layer==collider_layer)
            {
                is_included=true;
                break;
            }
        }
        if(is_it_other_layer)
            is_included=!is_included;
        return is_included;
    }
}
