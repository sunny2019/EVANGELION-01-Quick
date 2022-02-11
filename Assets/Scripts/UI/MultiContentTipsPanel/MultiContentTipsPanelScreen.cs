using System;

namespace Game.UI
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using EVANGELION;
    using Michsky.UI.ModernUIPack;
    using PathologicalGames;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using UnityEngine.Video;

    public class MultiContentTipsPanelScreenParam : UIOpenScreenParameterBase
    {
        public MultiContentTipsType tipType;

        //图片提示参数（大小1482*937）
        public Sprite spriteContent;
        public string spriteContentTitle;
        public string spriteContentDesc;

        //文字提示参数
        public string txtContent;

        //视频提示参数
        public string txtVideoName;

        //长图提示参数（宽1482）
        public Sprite spriteListViewContent;

        //分页窗口
        public List<MultiContentTipsPanelScreen.PageContent> pageContents;
    }

    public enum MultiContentTipsType
    {
        SpriteContent, //  大小1482*937
        TxtContent,
        VideoContent,
        SpriteListView, //宽1482
        PageWindow,
    }


    public class MultiContentTipsPanelScreen : ScreenBase
    {
        MultiContentTipsPanelCtrl mCtrl;
        MultiContentTipsPanelScreenParam mParam;


        public override string mResName
        {
            get => UIConst.MultiContentTipsPanel;
        }

#pragma warning disable CS1998
        protected override async UniTask OnLoadSuccess()
#pragma warning restore CS1998
        {
            mCtrl = mCtrlBase as MultiContentTipsPanelCtrl;
            mParam = mOpenParam as MultiContentTipsPanelScreenParam;

            GameObject showObj = null;
            if (mParam.tipType == MultiContentTipsType.SpriteContent)
            {
                showObj = mCtrl.go_SpriteContent;
                mCtrl.ig_SpriteContent.sprite = mParam.spriteContent;
                if (string.IsNullOrEmpty(mParam.spriteContentTitle))
                {
                    mCtrl.txt_SpriteContentTitle.gameObject.SetActive(true);
                    mCtrl.txt_SpriteContentTitle.text = mParam.spriteContentTitle;
                }
                else
                    mCtrl.txt_SpriteContentTitle.gameObject.SetActive(false);

                if (string.IsNullOrEmpty(mParam.spriteContentDesc))
                {
                    mCtrl.txt_SpriteContentDesc.gameObject.SetActive(true);
                    mCtrl.txt_SpriteContentDesc.text = mParam.spriteContentDesc;
                }
                else
                    mCtrl.txt_SpriteContentDesc.gameObject.SetActive(false);
            }
            else if (mParam.tipType == MultiContentTipsType.TxtContent)
            {
                showObj = mCtrl.go_TxtContent;
                mCtrl.txt_txtContent.text = mParam.txtContent;
            }
            else if (mParam.tipType == MultiContentTipsType.VideoContent)
            {
                showObj = mCtrl.go_VedioConent;
                mCtrl.vp_VideoPlayer.source = VideoSource.Url;
                mCtrl.vp_VideoPlayer.url = Application.streamingAssetsPath + "/Video/" + mParam.txtVideoName + ".mp4";
                mCtrl.vp_VideoPlayer.Play();
                mCtrl.txt_VedioTitle.text = mParam.txtVideoName;
                mCtrl.vp_VideoPlayer.loopPointReached += mCtrl.CloseVideoContent;
            }
            else if (mParam.tipType == MultiContentTipsType.SpriteListView)
            {
                showObj = mCtrl.go_SpirteListView;
                mCtrl.ig_SpirteListViewContent.sprite = mParam.spriteListViewContent;
            }
            else if (mParam.tipType == MultiContentTipsType.PageWindow)
            {
                showObj = mCtrl.go_PageWindow;
                InitPageWindow(mParam.pageContents);
            }

            showObj.SetActive(true);
            showObj.GetComponent<Animator>().Play("ShowContent");


            ETTool.Add(mCtrl.go_Background, EventTriggerType.PointerClick, (arg) => { mCtrl.StartCoroutine(mCtrl.Close(mParam.tipType)); });
        }

        public static async UniTask ShowSpriteContent(Sprite content)
        {
            await ELUIManager.Ins.OpenUI<MultiContentTipsPanelScreen>(new MultiContentTipsPanelScreenParam()
            {
                tipType = MultiContentTipsType.SpriteContent,
                spriteContent = content,
            });
        }

        public static async UniTask ShowTxtContent(string content)
        {
            await ELUIManager.Ins.OpenUI<MultiContentTipsPanelScreen>(new MultiContentTipsPanelScreenParam()
            {
                tipType = MultiContentTipsType.TxtContent,
                txtContent = content,
            });
        }

        public static async UniTask ShowVideoContent(string videoName)
        {
            await ELUIManager.Ins.OpenUI<MultiContentTipsPanelScreen>(new MultiContentTipsPanelScreenParam()
            {
                tipType = MultiContentTipsType.VideoContent,
                txtVideoName = videoName,
            });
        }

        public static async UniTask ShowSpriteListViewContent(Sprite content)
        {
            await ELUIManager.Ins.OpenUI<MultiContentTipsPanelScreen>(new MultiContentTipsPanelScreenParam()
            {
                tipType = MultiContentTipsType.SpriteListView,
                spriteListViewContent = content,
            });
        }

        public static async UniTask ShowPageWindowContent(List<PageContent> content)
        {
            await ELUIManager.Ins.OpenUI<MultiContentTipsPanelScreen>(new MultiContentTipsPanelScreenParam()
            {
                tipType = MultiContentTipsType.PageWindow,
                pageContents = content,
            });
        }

        public void InitPageWindow(List<PageContent> content)
        {
            Transform detailTxtContent = PoolManager.Pools["DetailPanelPool"].prefabs["DetailTxtContent"];
            Transform detailSpriteContent = PoolManager.Pools["DetailPanelPool"].prefabs["DetailSpriteContent"];
            Transform detailWindowButton = PoolManager.Pools["DetailPanelPool"].prefabs["DetailWindowButton"];
            for (int i = 0; i < mParam.pageContents.Count; i++)
            {
                int index = i;
                WindowManager.WindowItem item = new WindowManager.WindowItem();
                item.windowName = mParam.pageContents[index].title;
                item.buttonObject = PoolManager.Pools["DetailPanelPool"].Spawn(detailWindowButton).gameObject;
                item.buttonObject.transform.SetParent(mCtrl.trans_PageWindowButtons);
                item.buttonObject.transform.UIPoolSpawnedResetXYZ(Vector3.zero);
                item.buttonObject.GetComponent<DetailWindowButton>().SetName(mParam.pageContents[index].title);
                item.buttonObject.GetComponent<Button>().onClick.RemoveAllListeners();
                item.buttonObject.GetComponent<Button>().onClick.AddListener(() => { mCtrl.windowManager_PageWindow.OpenPanel(mParam.pageContents[index].title); });
                if (mParam.pageContents[i].isSprite)
                {
                    item.windowObject = PoolManager.Pools["DetailPanelPool"].Spawn(detailSpriteContent).gameObject;
                    item.windowObject.GetComponent<DetailSpriteContent>().SetContent(mParam.pageContents[index].spriteContent);
                    item.windowObject.transform.SetParent(mCtrl.trans_PageWindowWindows);
                    item.windowObject.transform.UIPoolSpawnedResetXYZ(Vector3.zero).UIPoolSpawnedResetSizeDelta(Vector2.zero);
                }
                else
                {
                    item.windowObject = PoolManager.Pools["DetailPanelPool"].Spawn(detailTxtContent).UIPoolSpawnedResetY().gameObject;
                    item.windowObject.GetComponent<DetailTxtContent>().SetContent(mParam.pageContents[index].txtContent);
                    item.windowObject.transform.SetParent(mCtrl.trans_PageWindowWindows);
                    item.windowObject.transform.UIPoolSpawnedResetXYZ(Vector3.zero).UIPoolSpawnedResetSizeDelta(Vector2.zero);
                }

                mCtrl.windowManager_PageWindow.windows.Add(item);
            }
        }

        [CreateAssetMenu(order = 1, menuName = "ELConfigs/DetailItemContent")]
        public class DetailItemContent : SerializedScriptableObject
        {
            public string title;
            public PromptClassification classify;

            [ShowIf("classify", PromptClassification.文字)] [TextArea(3, 10)]
            public string txtContent;

            [ShowIf("classify", PromptClassification.图片)]
            public Sprite spriteContent;

            [ShowIf("classify", PromptClassification.分页)]
            public List<PageContent> pageContents = new List<PageContent>();
        }

        public enum PromptClassification
        {
            文字,
            图片,
            分页,
        }

        /// <summary>
        /// 分页内容
        /// </summary>
        [Serializable]
        public class PageContent
        {
            public string title;
            public bool isSprite = false;
            [HideIf("isSprite")] [TextArea(3, 10)] public string txtContent;
            [ShowIf("isSprite")] public Sprite spriteContent;
        }
    }
}
