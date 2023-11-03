using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;
    float axisH = 0.0f;
    public float speed = 3.0f;
    public float jump = 9.0f;
    public LayerMask groundLayer;
    bool goJump = false;

    //アニメーション対応
    Animator animator;
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";

    //ゲームの状態を管理
    public static string gameState = "playing";

    public int score = 0; //ゲームスコア

    // タッチスクリーン対応追加
    bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        //Animatorを取ってくる．
        animator = GetComponent<Animator>();
        nowAnime = "stopAnime";
        oldAnime = "stopAnime";
        gameState = "playing";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != "playing")
        {
            return;
        }

        //移動
        if (isMoving == false)
        {
            //水平方向の入力をチェックする
            axisH = Input.GetAxisRaw("Horizontal");
        }

        //向きの調整
        if (axisH > 0.0f)
        {
            //右移動
            // Debug.Log("右移動");
            this.transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            //左移動
            // Debug.Log("左移動");
            this.transform.localScale = new Vector2(-1, 1);
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (gameState != "playing")
        {
            return;
        }

        bool onGround = Physics2D.CircleCast(this.transform.position,
                                            0.2f,
                                            Vector2.down,
                                            0.0f,
                                            groundLayer);
        if (onGround || axisH != 0)
        {
            //地面の上OR速度がではない
            //速度を更新する
            rbody.velocity = new Vector2(axisH * speed, rbody.velocity.y);
        }

        if (onGround && goJump)
        {
            //地面の上でジャンプキーが押された．
            //ジャンプさせる
            Vector2 jumpPw = new Vector2(0, jump);
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);
            goJump = false;
        }

        //アニメーションを更新
        if (onGround)
        {
            //地面の上
            if (axisH == 0)
            {
                nowAnime = stopAnime;
            }
            else
            {
                nowAnime = moveAnime;
            }
        }
        else
        {
            //空中
            nowAnime = jumpAnime;
        }

        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);
        }
    }

    //ジャンプ
    public void Jump()
    {
        //ジャンプフラグを立てる
        goJump = true;
    }

    //接触開始
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal();
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();
        }
        else if (collision.gameObject.tag == "ScoreItem")
        {
            ItemData item = collision.gameObject.GetComponent<ItemData>();
            score = item.value;
            Destroy(collision.gameObject);
        }
    }

    //ゴール
    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameclear";
        GameStop();
    }

    //ゲームオーバー
    public void GameOver()
    {
        animator.Play(deadAnime);

        gameState = "gameover";
        GameStop();
        //ゲームオーバー演出をつける
        //あたり判定の削除
        this.GetComponent<CapsuleCollider2D>().enabled = false;
        //プレイヤーを上に少し跳ね上げる演出
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }

    //ゲーム停止
    void GameStop()
    {
        //Rigidbody2Dをとってくる
        Rigidbody2D rbody = this.GetComponent<Rigidbody2D>();
        //速度を0にして強制停止
        rbody.velocity = new Vector2(0, 0);
    }

    // タッチスクリーン対応追加
    public void SetAxis(float h, float v)
    {
        axisH = h;
        if (axisH == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }
}

