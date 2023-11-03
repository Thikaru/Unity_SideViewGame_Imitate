using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualPad : MonoBehaviour
{
    public float MaxLength = 70; //タブが動く最大距離
    public bool is4DPad = false; //上下左右に動かすフラグ
    GameObject player; //操作するプレイヤーのGameObject
    Vector2 defPos; //タブの初期位置
    Vector2 downPos; //タッチ位置

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーキャラクタを取得
        player = GameObject.FindGameObjectWithTag("Player");
        //タブの初期位置
        defPos = this.GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //ダウンイベント
    public void PadDown()
    {
        //マウスポイントのスクリーン座標
        downPos = Input.mousePosition;
    }

    //ドラッグイベント
    public void PadDrag()
    {
        // マウスポイントのスクリーン座標
        Vector2 mousePosition = Input.mousePosition;
        //新しいタブの位置を求める
        Vector2 newTabPos = mousePosition - downPos; //マウスダウン位置からの移動差分
        if (is4DPad == false)
        {
            newTabPos.y = 0; //横スクロールの場合はY軸を0にする
        }

        //移動ベクトルを計算する
        Vector2 axis = newTabPos.normalized; //ベクトルを正規化する
        //2点の距離を求める
        float len = Vector2.Distance(defPos, newTabPos);
        if (len > MaxLength)
        {
            //限界距離を超えたので限界座標を設定する
            newTabPos.x = axis.x * MaxLength;
            newTabPos.y = axis.y * MaxLength;
        }

        //タブを移動させる
        this.GetComponent<RectTransform>().localPosition = newTabPos;
        //プレイヤーキャラクタを移動させる
        PlayerController plcnt = player.GetComponent<PlayerController>();
        plcnt.SetAxis(axis.x, axis.y);
    }

    //アップイベント
    public void PadUp()
    {
        //タブの位置を初期化
        this.GetComponent<RectTransform>().localPosition = defPos;
        //プレイヤーキャラクタを停止させる
        PlayerController plcnt = player.GetComponent<PlayerController>();
        plcnt.SetAxis(0, 0);
    }
}
