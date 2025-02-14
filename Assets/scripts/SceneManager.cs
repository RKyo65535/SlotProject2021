using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlotProject
{

    public class SceneManager : MonoBehaviour
    {

        [SerializeField] ReelService reelService;

        [SerializeField] CoinService coinService;

        [SerializeField] SoundEffectService soundEffectService;

        [SerializeField] ButtonTypeEnum buttonType;

        public void Start()
        {
            // ボタンを押した時に実行するメソッドを設定
            this.GetComponent<Button>().onClick.AddListener(HandlePushButton);
        }

        // 何かしらのボタンが押された
        public void HandlePushButton()
        {
            switch (this.buttonType)
            {
                case ButtonTypeEnum.LEVER:
                    this.HandlePullLever();
                    break;
                case ButtonTypeEnum.LEFT:
                    this.HandlePushButton(ReelTypeEnum.LEFT);
                    break;
                case ButtonTypeEnum.CENTER:
                    this.HandlePushButton(ReelTypeEnum.CENTER);
                    break;
                case ButtonTypeEnum.RIGHT:
                    this.HandlePushButton(ReelTypeEnum.RIGHT);
                    break;
            }
        }

        // レバーを下げたとき
        public void HandlePullLever()
        {
            if (this.reelService.IsAllReelStop() && this.coinService.canInsertCredit())
            {
                this.soundEffectService.PlayLeverSound();
                this.coinService.InsertCredit();
                this.reelService.startAll();
            }
        }

        // ボタンを押したとき
        public void HandlePushButton(ReelTypeEnum reelType)
        {
            if (!this.reelService.IsReelStop(reelType))
            {
                this.soundEffectService.PlayButtonSound();
                this.reelService.StopSpinning(reelType);

                // 揃った図柄に応じた処理
                List<SymbolTypeEnum> symbols = this.reelService.GetObtainedSymbols();
                this.coinService.GivePayout(symbols);
                this.soundEffectService.PlaySymbolSound(symbols);
            }
        }

    }

}
