using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosJudgeS : MonoBehaviour
{

    public float dx;
    float pos;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 tmp = GameObject.Find("hitline").transform.position;
        pos = tmp.x;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(-1 * dx * Time.deltaTime, 0, 0);//等速移動

        if (Input.GetKeyDown(KeyCode.S))
        {
            if(transform.position.x < pos + dx * 0.03 && transform.position.x > pos - dx * 0.03)            //0.06sの範囲内なら(1秒にdxだけ進む,判定ラインからの距離により測定
            {
                GetComponent<Renderer>().material.color = Color.yellow;
            }else if(transform.position.x < pos + dx * 0.075 && transform.position.x > pos - dx * 0.075)    //0.15sの範囲内なら
            {
                GetComponent<Renderer>().material.color = Color.blue;

            }
        }
    




        
    }

    

    
}
