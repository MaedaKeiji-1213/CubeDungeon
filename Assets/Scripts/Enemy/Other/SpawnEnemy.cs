using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using System.Linq;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class SpawnEnemy : MonoBehaviour

{

    public int enemy;

    public uint set_level;




    GameObject enemy_object;




    const float SPAWN_AREA_RANGE = 5;

    public const string ENEMY_FOLDER_PATH = "Enemy";//敵のプレハブがおいてあるファイルのパス




    // Update is called once per frame

    void Update()

    {

        Vector2 this_position = transform.position;

        Vector2 spawn_area_origin = Camera.main.ViewportToWorldPoint(Vector3.zero) - Vector3.one * SPAWN_AREA_RANGE;

        Vector2 spawn_area_end = Camera.main.ViewportToWorldPoint(Vector3.one) + Vector3.one * SPAWN_AREA_RANGE;

        if (enemy_object == null)

        {

            if (spawn_area_origin.x <= this_position.x && spawn_area_origin.y <= this_position.y &&

              spawn_area_end.x >= this_position.x && spawn_area_end.y >= this_position.y)

            {

                Debug.Log("IsSpawnArea");

                GameObject enemy_prefab = Resources.LoadAll<GameObject>(ENEMY_FOLDER_PATH)[enemy];

                enemy_object = Instantiate<GameObject>(enemy_prefab, transform.position, Quaternion.identity, transform);

                IStatus status = enemy_object.GetComponent<IStatus>();

                if (status != null) status.Setlevel((int)set_level);

            }

        }

        else

        {

            Vector2 enemy_position = enemy_object.transform.position;

            if (spawn_area_origin.x > enemy_position.x || spawn_area_origin.y > enemy_position.y ||

              spawn_area_end.x < enemy_position.x || spawn_area_end.y < enemy_position.y)

            {

                Destroy(enemy_object);

                // Debug.Log("origin"+spawn_area_origin);

                Debug.Log("end" + spawn_area_origin);

                // Debug.Log("position"+enemy_position);

            }

        }

    }






}




#if UNITY_EDITOR

[CustomEditor(typeof(SpawnEnemy))]

public class SpawnEnemyEditor : Editor

{



    public override void OnInspectorGUI()

    {

        var spawn_enemy = target as SpawnEnemy;//インスペクターを編集したいクラス




        /*インスペクターでリソース内にある敵のプレハブを選択できるようにする*/

        spawn_enemy.enemy = EditorGUILayout.Popup(

      "Enemy",//インスペクターに表示されるラベル

            spawn_enemy.enemy,//選択中の敵

            Resources.LoadAll<GameObject>(SpawnEnemy.ENEMY_FOLDER_PATH).Select((x) => x.name.ToString()).ToArray()//ポップアップの選択肢

        );




        spawn_enemy.set_level = (uint)EditorGUILayout.IntField("Level", (int)spawn_enemy.set_level);//敵のレベルを入力できるようにする

    }

}

#endif