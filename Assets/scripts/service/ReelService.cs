using System;
using System.Collections.Generic;
using UnityEngine;

namespace SlotProject
{

    public class ReelService : MonoBehaviour
    {

        [SerializeField] int frameRate;

        [SerializeField] ReelFactory reelFactory;

        private SymbolFactory symbolFactory = new SymbolFactory();

        private ReelModel[] reels;

        // FIXME: サービスが状態保つのはNG
        private bool isPushedLastButton;

        public void Start()
        {
            // フレームレート
            Time.fixedDeltaTime = 1.0f / this.frameRate;

            // リールオブジェクトを取得
            this.reels = new ReelModel[3] {
                reelFactory.create(ReelTypeEnum.LEFT),
                reelFactory.create(ReelTypeEnum.CENTER),
                reelFactory.create(ReelTypeEnum.RIGHT),
            };

            // 初期画面の図柄を表示
            this.publishAllDisplayedSymbols();
        }

        public void FixedUpdate()
        {
            // 1フレーム分回転させる
            foreach (ReelModel reel in this.reels)
            {
                // 回転中なら次の図柄へ移動
                if (reel.GetIsSpinning())
                {
                    reel.SpinNextFrame();
                }
            }
            this.publishAllDisplayedSymbols();
        }

        // リール図柄を画面に配置
        public void publishAllDisplayedSymbols()
        {
            foreach (ReelModel reel in this.reels)
            {
                reel.PublishSymbolSprite(this.symbolFactory.create(reel.GetCurrentSymbol(DisplayedSymbolTypeEnum.TOP)), DisplayedSymbolTypeEnum.TOP);
                reel.PublishSymbolSprite(this.symbolFactory.create(reel.GetCurrentSymbol(DisplayedSymbolTypeEnum.CENTER)), DisplayedSymbolTypeEnum.CENTER);
                reel.PublishSymbolSprite(this.symbolFactory.create(reel.GetCurrentSymbol(DisplayedSymbolTypeEnum.UNDER)), DisplayedSymbolTypeEnum.UNDER);
            }
        }

        // 全てのリールを回転させる
        public void startAll()
        {
            this.StartSpinning(ReelTypeEnum.LEFT);
            this.StartSpinning(ReelTypeEnum.CENTER);
            this.StartSpinning(ReelTypeEnum.RIGHT);

            this.isPushedLastButton = false;
        }

        // 特定のリールを回転させる
        private void StartSpinning(ReelTypeEnum reelType)
        {
            this.reels[(int)reelType].StartSpinning();
        }

        // 特定のリールを停止させる
        public void StopSpinning(ReelTypeEnum reelType)
        {
            if (this.IsAllReelStop())
            {
                return;
            }

            this.reels[(int)reelType].StopSpinning();
            this.isPushedLastButton = this.IsAllReelStop();
        }

        // リールが全て停止しているか？
        public bool IsAllReelStop()
        {
            bool result = true;
            foreach (ReelModel reel in this.reels)
            {
                result = result && !reel.GetIsSpinning();
            }

            return result;
        }

        // 指定したリールが停止しているか？
        public bool IsReelStop(ReelTypeEnum reelType)
        {
            return !this.reels[(int)reelType].GetIsSpinning();
        }

        // 揃っている図柄を取得
        public List<SymbolTypeEnum> GetObtainedSymbols()
        {
            List<SymbolTypeEnum> obtainedSymbols = new List<SymbolTypeEnum>();

            // 最後のボタンが押された時のみ図柄を判定する
            if (this.isPushedLastButton)
            {
                this.isPushedLastButton = false;
            }
            else
            {
                return obtainedSymbols;
            }

            // 上段揃い
            if (this.reels[(int)ReelTypeEnum.LEFT].GetCurrentSymbol(DisplayedSymbolTypeEnum.TOP) == //
                this.reels[(int)ReelTypeEnum.CENTER].GetCurrentSymbol(DisplayedSymbolTypeEnum.TOP) && //
                this.reels[(int)ReelTypeEnum.LEFT].GetCurrentSymbol(DisplayedSymbolTypeEnum.TOP) == //
                this.reels[(int)ReelTypeEnum.RIGHT].GetCurrentSymbol(DisplayedSymbolTypeEnum.TOP))
            {
                obtainedSymbols.Add(this.reels[(int)ReelTypeEnum.LEFT].GetCurrentSymbol(DisplayedSymbolTypeEnum.TOP));
            }

            // 中央段揃い
            if (this.reels[(int)ReelTypeEnum.LEFT].GetCurrentSymbol(DisplayedSymbolTypeEnum.CENTER) == //
                this.reels[(int)ReelTypeEnum.CENTER].GetCurrentSymbol(DisplayedSymbolTypeEnum.CENTER) && //
                this.reels[(int)ReelTypeEnum.LEFT].GetCurrentSymbol(DisplayedSymbolTypeEnum.CENTER) == //
                this.reels[(int)ReelTypeEnum.RIGHT].GetCurrentSymbol(DisplayedSymbolTypeEnum.CENTER))
            {
                obtainedSymbols.Add(this.reels[(int)ReelTypeEnum.LEFT].GetCurrentSymbol(DisplayedSymbolTypeEnum.CENTER));
            }

            // 下段揃い
            if (this.reels[(int)ReelTypeEnum.LEFT].GetCurrentSymbol(DisplayedSymbolTypeEnum.UNDER) == //
                this.reels[(int)ReelTypeEnum.CENTER].GetCurrentSymbol(DisplayedSymbolTypeEnum.UNDER) && //
                this.reels[(int)ReelTypeEnum.LEFT].GetCurrentSymbol(DisplayedSymbolTypeEnum.UNDER) == //
                this.reels[(int)ReelTypeEnum.RIGHT].GetCurrentSymbol(DisplayedSymbolTypeEnum.UNDER))
            {
                obtainedSymbols.Add(this.reels[(int)ReelTypeEnum.LEFT].GetCurrentSymbol(DisplayedSymbolTypeEnum.UNDER));
            }

            // 左斜め揃い（\）
            if (this.reels[(int)ReelTypeEnum.LEFT].GetCurrentSymbol(DisplayedSymbolTypeEnum.TOP) == //
                this.reels[(int)ReelTypeEnum.CENTER].GetCurrentSymbol(DisplayedSymbolTypeEnum.CENTER) && //
                this.reels[(int)ReelTypeEnum.LEFT].GetCurrentSymbol(DisplayedSymbolTypeEnum.TOP) == //
                this.reels[(int)ReelTypeEnum.RIGHT].GetCurrentSymbol(DisplayedSymbolTypeEnum.UNDER))
            {
                obtainedSymbols.Add(this.reels[(int)ReelTypeEnum.LEFT].GetCurrentSymbol(DisplayedSymbolTypeEnum.TOP));
            }

            // 右斜め揃い（/）
            if (this.reels[(int)ReelTypeEnum.LEFT].GetCurrentSymbol(DisplayedSymbolTypeEnum.UNDER) == //
                this.reels[(int)ReelTypeEnum.CENTER].GetCurrentSymbol(DisplayedSymbolTypeEnum.CENTER) && //
                this.reels[(int)ReelTypeEnum.LEFT].GetCurrentSymbol(DisplayedSymbolTypeEnum.UNDER) == //
                this.reels[(int)ReelTypeEnum.RIGHT].GetCurrentSymbol(DisplayedSymbolTypeEnum.TOP))
            {
                obtainedSymbols.Add(this.reels[(int)ReelTypeEnum.LEFT].GetCurrentSymbol(DisplayedSymbolTypeEnum.UNDER));
            }

            return obtainedSymbols;
        }

    }

}
