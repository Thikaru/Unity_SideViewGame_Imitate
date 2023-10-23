using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;
    float axisH = 0.0f;
    public float speed = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //水平方向の入力をチェックする
        axisH = Input.GetAxisRaw("Horizontal");
        //向きの調整
        if (axisH > 0.0f)
        {
            //右移動
            Debug.Log("右移動");
            this.transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            //左移動
            Debug.Log("左移動");
            this.transform.localScale = new Vector2(-1, 1);
        }
    }

    void FixedUpdate()
    {
        rbody.velocity = new Vector2(axisH * speed, rbody.velocity.y);
    }
}

