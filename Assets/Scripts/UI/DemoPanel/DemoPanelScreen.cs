namespace Game.UI
{
    using EVANGELION;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Networking;
    using UnityEngine.UI;

    public class DemoPanelScreenParam : UIOpenScreenParameterBase
    {
    }

    public class DemoPanelScreen : ScreenBase
    {
        public override string mResName => UIConst.DemoPanel;
        DemoPanelCtrl mCtrl;
        DemoPanelScreenParam mParam;

        protected override async UniTask OnLoadSuccess()
        {
            mCtrl = mCtrlBase as DemoPanelCtrl;
            mParam = mOpenParam as DemoPanelScreenParam;

            await DownLoadSprite("https://gimg2.baidu.com/image_search/src=http%3A%2F%2Finews.gtimg." +
                                 "com%2Fnewsapp_bt%2F0%2F14140880260%2F1000&refer=http%3A%2F%2Finews.gtimg." +
                                 "com&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=jpeg?sec=1642173955&" +
                                 "t=8fdefe32030efcfb709f61c4d2c5bfe3", mCtrl.img_Head);
            
            mCtrl.btn_GetInfo.onClick.AddListener(() =>
            {
                NotificationsPanelScreen.ShowNotifications("测试", "测试内容").GetAwaiter();
            });
        }

        private async UniTask DownLoadSprite(string url, Image image)
        {
            var asyncRequest = await UnityWebRequest.Get(url).SendWebRequest();
            if (asyncRequest.isHttpError|| asyncRequest.isNetworkError)
            {
                Debug.LogError("请求失败");
                return;
            }

            if (asyncRequest.isDone)
            {
                byte[] texBytes = asyncRequest.downloadHandler.data;
                Texture2D tex = new Texture2D(300, 300);
                tex.LoadImage(texBytes);
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                image.sprite = sprite;
            }
            else
            {
                Debug.LogError(asyncRequest.error);
            }
        }
    }
}