using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float moveX = 0.0f;  //X移動距離
    public float moveY = 0.0f; //Y移動距離
    public float times = 0.0f; //時間
    public float wait = 0.0f; //停止時間
    public bool isMoveWhenOn = false; // 乗った時に動くフラグ
    public bool isCanMove = true; //動くフラグ
    Vector3 startPos; //初期位置
    Vector3 endPos; //移動位置
    bool isReverse = false; //反転フラグ

    float movep = 0; //移動補完値

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        endPos = new Vector2(startPos.x + moveX, startPos.y + moveY);
        if (isMoveWhenOn)
        {
            //乗った時に動くので最初は動かさない
            isCanMove = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCanMove)
        {
            float distance = Vector2.Distance(startPos, endPos);
            float ds = distance / times;
            float df = ds * Time.deltaTime;
            movep += df / distance;
            if (isReverse)
            {
                this.transform.position = Vector2.Lerp(endPos, startPos, movep);
            }
            else
            {
                this.transform.position = Vector2.Lerp(startPos, endPos, movep);
            }

            if (movep >= 1.0f)
            {
                movep = 0.0f;
                isReverse = !isReverse;
                isCanMove = false;
                if (isMoveWhenOn == false)
                {
                    //乗った時にフラグをON
                    Invoke("Move", wait);
                }
            }
        }
    }

    //移動フラグを立てる
    public void Move()
    {
        isCanMove = true;
    }

    //移動フラグを下す
    public void Stop()
    {
        isCanMove = false;
    }

    //接触開始
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //接触したのがプレイヤーなら移動床の子にする
            collision.transform.SetParent(transform);
            if (isMoveWhenOn)
            {
                //乗った時に動くフラグON
                isCanMove = true;
            }
        }
    }

    //接触終了
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //接触したのがプレイヤーなら移動床の子から外す
            collision.transform.SetParent(null);
        }
    }

    // 移動範囲表示
    void OnDrawGizmosSelected()
    {
        Vector2 fromPos;
        if (startPos == Vector3.zero)
        {
            fromPos = this.transform.position;
        }
        else
        {
            fromPos = startPos;
        }
        //移動線
        Gizmos.DrawLine(fromPos, new Vector2(fromPos.x + moveX, fromPos.y + moveY));
        // スプライトのサイズ
        Vector2 size = this.GetComponent<SpriteRenderer>().size;
        // 初期位置
        Gizmos.DrawWireCube(fromPos, new Vector2(size.x, size.y));
        // 移動位置
        Vector2 toPos = new Vector2(fromPos.x + moveX, fromPos.y + moveY);
        Gizmos.DrawWireCube(toPos, new Vector2(size.x, size.y));
    }
}
