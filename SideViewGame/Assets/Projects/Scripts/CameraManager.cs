using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float leftLimit = 0.0f;
    public float rightLimit = 0.0f;
    public float topLimit = 0.0f;
    public float bottomLimit = 0.0f;

    public GameObject subScreen;

    public bool isForceScrollX = false;
    public float forceScrollSpeedX = 0.5f;
    public bool isForceScrollY = false;
    public float forceScrollSpeedY = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            //カメラの座標更新
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = this.transform.position.z;
            //横を同期させる
            if (isForceScrollX)
            {
                //横の強制スクロール
                x = this.transform.position.x + (forceScrollSpeedX * Time.deltaTime);
            }
            else if (x < leftLimit)
            {
                x = leftLimit;
            }
            else if (x > rightLimit)
            {
                x = rightLimit;
            }
            //縦を同期させる
            if (isForceScrollY)
            {
                y = this.transform.position.y + (forceScrollSpeedY * Time.deltaTime);
            }
            else if (y < bottomLimit)
            {
                y = bottomLimit;
            }
            else if (y > topLimit)
            {
                y = topLimit;
            }
            //カメラ位置のVector3を作る
            Vector3 v3 = new Vector3(x, y, z);
            this.transform.position = v3;

            //サブスクリーンスクロール
            if (subScreen != null)
            {
                y = subScreen.transform.position.y;
                z = subScreen.transform.position.z;
                Vector3 v = new Vector3(x / 2.0f, y, z);
                subScreen.transform.position = v;
            }
        }
    }
}
