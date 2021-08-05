﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelRoll : MonoBehaviour
{

    const float ROLL_ONE_FLAME = 0.1f;

    Transform TF;
    public bool isRolling;
    ReelZugara reelZugara;

    [SerializeField] int initialReelIndex;
    [SerializeField] int indicateReelLocation;//これは、リールの図柄の為に利用しているものです。
    [SerializeField]int reelIndexOffset;//何回分ずれたか


    public float topY;//スロットの天井となるy座標
    public float bottomY;//スロットの底となるy座標
    public int symbolCount;//いくつのシンボルをまとめて扱っているか


    [SerializeField] Sprite[] sprites;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        reelZugara = new ReelZugara();
        TF = GetComponent<Transform>();//キャッシュ(GetCOnponentはクソ思い)
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //図柄の場所をずらす
        if (isRolling)
        {

            //図柄のy座標を移動する
            float nextPositionY = TF.position.y - ROLL_ONE_FLAME;
            TF.position = new Vector3(TF.position.x, nextPositionY, TF.position.z);

            //思ったよりも下に行くようなら画像を変えて、一番上にもってく
            if (TF.position.y < bottomY)
            {
                reelIndexOffset = (reelIndexOffset + symbolCount) % reelZugara.reel.Length;
                int nextSymbolIndex = (reelIndexOffset + initialReelIndex) % reelZugara.reel.Length;
                spriteRenderer.sprite = sprites[reelZugara.reel[nextSymbolIndex]];

                TF.position = new Vector3(TF.position.x, topY, TF.position.z);

            }




        }
        else
        {
            
        }
    }

    /// <summary>
    /// 図形をゲットする関数です
    /// </summary>
    public int GetZugara()
    {
        int symbolIndex = (reelIndexOffset + initialReelIndex) % reelZugara.reel.Length;
        return reelZugara.reel[symbolIndex];
    }

}
