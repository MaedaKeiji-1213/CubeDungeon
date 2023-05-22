using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGoalWhenDead : MonoBehaviour
{
    [SerializeField] GameObject Goal;
    // Start is called before the first frame update
    void Start()
    {
        Goal.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnDestroy()
    {
        Goal.SetActive(true);
        
    }


}
