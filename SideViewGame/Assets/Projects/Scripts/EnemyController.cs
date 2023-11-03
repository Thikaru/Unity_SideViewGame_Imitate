using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f; //移動速度
    public bool isToRight = false; // true:右向き false:左向き
    public float revTime = 0; //反転時間
    public LayerMask groundLayer; //地面レイヤー

    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (isToRight)
        {
            this.transform.localScale = new Vector2(-1, 1); //向きの変更
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (revTime > 0)
        {
            time += Time.deltaTime;
            if (time >= revTime)
            {
                isToRight = !isToRight; //フラグを反転させる
                time = 0;
                if (isToRight)
                {
                    this.transform.localScale = new Vector2(-1, 1); //向きの変更
                }
                else
                {
                    this.transform.localScale = new Vector2(1, 1); //向きの変更
                }
            }
        }
    }

    void FixedUpdate()
    {
        //地上判定
        bool onGround = Physics2D.CircleCast(this.transform.position, 0.5f, Vector2.down, 0.5f, groundLayer);
        if (onGround)
        {
            //速度を更新する
            // Rigdbody2Dをとってくる
            Rigidbody2D rbody = this.GetComponent<Rigidbody2D>();
            if (isToRight)
            {
                rbody.velocity = new Vector2(speed, rbody.velocity.y);
            }
            else
            {
                rbody.velocity = new Vector2(-speed, rbody.velocity.y);
            }
        }
    }

    //接触
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isToRight = !isToRight; //フラグを反転させる
        time = 0; //タイマーを初期化する
        if (isToRight)
        {
            this.transform.localScale = new Vector2(-1, 1); //向きの変更
        }
        else
        {
            this.transform.localScale = new Vector2(1, 1); //向きの変更
        }
    }
}
