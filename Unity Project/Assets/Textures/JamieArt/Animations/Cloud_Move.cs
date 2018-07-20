using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_Move : MonoBehaviour {
    
    public float speed = 1;
    public float out_reset;
    RectTransform rect;
    public float min_speed = 75f;
    public float max_speed = 125f;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        speed = Random.Range(min_speed, max_speed);
    }

    // Update is called once per frame
    void Update () {


        Vector3 pos = rect.anchoredPosition;
        pos.x = pos.x + (this.transform.right * (speed * Time.deltaTime)).x;
        //this.transform.Translate(this.transform.right * (speed * Time.deltaTime));
        //Vector3 position = this.transform.position;
        rect.anchoredPosition = pos;
        pos = rect.anchoredPosition;
        if(pos.x > out_reset)
        {
            pos.x = -out_reset;
            rect.anchoredPosition = pos;
            speed = Random.Range(min_speed, max_speed);
        }
        if(pos.x < -out_reset)
        {
            pos.x = out_reset;
            rect.anchoredPosition = pos;            
            speed = Random.Range(-max_speed, -min_speed);
        }
        
	}
}
