using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UIを使用するのに必要

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;
    public Sprite gameOverSpr;
    public Sprite gameClearSpr;
    public GameObject panel;
    public GameObject restartButton;
    public GameObject nextButton;

    Image titleImage; //画像を表示しているImageコンポーネント

    //時間制限を追加
    public GameObject timeBar;
    public GameObject timeText;
    TimeController timeCnt;

    //スコアを追加
    public GameObject scoreText;
    public static int totalScore;
    public int stageScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        //画像を非表示にする
        Invoke("InactiveImage", 1.0f);
        //ボタン・パネルを非表示にする
        panel.SetActive(false);

        //時間制限追加
        timeCnt = this.GetComponent<TimeController>();
        if (timeCnt != null)
        {
            if (timeCnt.gameTime == 0.0f)
            {
                //制限時間なしなら隠す
                timeBar.SetActive(false);
            }
        }

        //スコア追加
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == "gameclear")
        {
            //ゲームクリア
            mainImage.SetActive(true);
            panel.SetActive(true);
            //RESTARTボタンを無効化する
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameClearSpr;
            PlayerController.gameState = "gameend";

            //時間制限を追加
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true; //時間カウントを停止
                //スコアを追加
                int time = (int)timeCnt.displayTime;
                totalScore += time * 10;//残り時間をスコアに加える
            }

            //スコアを追加
            totalScore += stageScore;
            stageScore = 0;
            UpdateScore();
        }
        else if (PlayerController.gameState == "gameover")
        {
            //ゲームオーバ
            mainImage.SetActive(true);
            panel.SetActive(true);
            //RESTARTボタンを無効化する
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            PlayerController.gameState = "gameend";

            //時間制限を追加
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true; //時間カウントを停止
            }
        }
        else if (PlayerController.gameState == "playing")
        {
            //ゲーム中 
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerController playerCnt = player.GetComponent<PlayerController>();
            //時間制限追加
            if (timeCnt != null)
            {
                if (timeCnt.gameTime > 0.0f)
                {
                    //変数に代入することで少数を省く
                    int time = (int)timeCnt.displayTime;
                    //タイム更新
                    timeText.GetComponent<Text>().text = time.ToString();
                    if (time == 0)
                    {
                        playerCnt.GameOver(); //ゲームオーバにする
                    }
                }
            }

            //スコアを追加
            if (playerCnt.score != 0)
            {
                stageScore += playerCnt.score;
                playerCnt.score = 0;
                UpdateScore();
            }
        }
    }

    // 画像を非表示にする
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    //スコアを追加
    void UpdateScore()
    {
        int score = stageScore + totalScore;
        scoreText.GetComponent<Text>().text = score.ToString();
    }
}
