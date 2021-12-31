namespace Game.UI
{
    using Cysharp.Threading.Tasks;
    using EVANGELION;
    using Michsky.UI.ModernUIPack;
    using UnityEngine;
    using UnityEngine.UI;

    public class NotificationsPanelScreenParam : UIOpenScreenParameterBase
    {
    }

    public class NotificationsPanelScreen : ScreenBase
    {
        NotificationsPanelCtrl mCtrl;
        NotificationsPanelScreenParam mParam;

        public override string mResName
        {
            get => UIConst.NotificationsPanel;
        }

#pragma warning disable CS1998
        protected override async UniTask OnLoadSuccess()
#pragma warning restore CS1998
        {
            mCtrl = mCtrlBase as NotificationsPanelCtrl;
            mParam = mOpenParam as NotificationsPanelScreenParam;
        }

        public static async UniTask ShowNotifications(string title, string description, NPAppearMode mode = NPAppearMode.Sliding, NPLocation location = NPLocation.TR)
        {
            NotificationsPanelScreen notificationsPanelScreen = await ELUIManager.Ins.OpenUI<NotificationsPanelScreen>();
            NotificationManager notificationManager = notificationsPanelScreen.mCtrl.notificationManagerDic[mode + "Notification" + location];
            notificationManager.title = title;
            notificationManager.description = description;
            notificationManager.UpdateUI();
            notificationManager.OpenNotification();
        }

        private bool FloatNoticeIsShow = false;

        public static async UniTask ShowFloatNotice(string content, Transform target, bool needFloow = false)
        {
            NotificationsPanelScreen notificationsPanelScreen = await ELUIManager.Ins.OpenUI<NotificationsPanelScreen>();
            if (notificationsPanelScreen.FloatNoticeIsShow) CloseFloatNotice(); //如果提示没关掉，就自己关掉

            notificationsPanelScreen.mCtrl.txt_FloatContent.text = content;


            Vector2 viewport = Camera.main.WorldToViewportPoint(target.position);
            Vector2 referenceResolution = notificationsPanelScreen.mCtrl.GetComponent<CanvasScaler>().referenceResolution;
            Vector2 uiPos = new Vector2(viewport.x * referenceResolution.x, viewport.y * referenceResolution.y);

            notificationsPanelScreen.mCtrl.go_FloatNotice.GetComponent<RectTransform>().anchoredPosition = uiPos;
            notificationsPanelScreen.mCtrl.go_FloatNotice.SetActive(true);
            if (needFloow)
            {
                notificationsPanelScreen.mCtrl.floatNoticeCoroutine.Add(notificationsPanelScreen.mCtrl.StartCoroutine(notificationsPanelScreen.mCtrl.FloatNoticeFllowTarget(target)));
            }

            notificationsPanelScreen.FloatNoticeIsShow = true;
        }

        public static void CloseFloatNotice()
        {
            NotificationsPanelScreen notificationsPanelScreen = ELUIManager.Ins.GetUI<NotificationsPanelScreen>();
            notificationsPanelScreen.mCtrl.go_FloatNotice.SetActive(false);
            notificationsPanelScreen.mCtrl.StopFloatCoroutine();
            notificationsPanelScreen.FloatNoticeIsShow = false;
        }


        private bool HoverNoticeIsShow = false;

        public static async UniTask ShowHoverNotice(string content, bool needFloowMouse = true)
        {
            NotificationsPanelScreen notificationsPanelScreen = await ELUIManager.Ins.OpenUI<NotificationsPanelScreen>();
            if (notificationsPanelScreen.HoverNoticeIsShow) CloseHoverNotice(); //如果提示没关掉，就自己关掉

            notificationsPanelScreen.mCtrl.txt_HoverContent.text = content;

            Vector2 viewport = Camera.main.ScreenToViewportPoint(ELUIManager.MouseScreenPosition);
            Vector2 referenceResolution = notificationsPanelScreen.mCtrl.GetComponent<CanvasScaler>().referenceResolution;
            Vector2 uiPos = new Vector2(viewport.x * referenceResolution.x, viewport.y * referenceResolution.y);

            notificationsPanelScreen.mCtrl.go_HoverNotice.GetComponent<RectTransform>().anchoredPosition = uiPos;
            notificationsPanelScreen.mCtrl.go_HoverNotice.SetActive(true);
            if (needFloowMouse)
            {
                notificationsPanelScreen.mCtrl.hoverNoticeCoroutine.Add(notificationsPanelScreen.mCtrl.StartCoroutine(notificationsPanelScreen.mCtrl.HoverNoticeFllowMouse()));
            }

            notificationsPanelScreen.HoverNoticeIsShow = true;
        }

        public static void CloseHoverNotice()
        {
            NotificationsPanelScreen notificationsPanelScreen = ELUIManager.Ins.GetUI<NotificationsPanelScreen>();
            notificationsPanelScreen.mCtrl.go_HoverNotice.SetActive(false);
            notificationsPanelScreen.mCtrl.StopHoverCoroutine();
            notificationsPanelScreen.HoverNoticeIsShow = false;
        }


        private bool FingerPointingIsShow = false;

        public static async UniTask ShowFingerPointing(Transform target, bool needFloow = false)
        {
            NotificationsPanelScreen notificationsPanelScreen = await ELUIManager.Ins.OpenUI<NotificationsPanelScreen>();
            if (notificationsPanelScreen.FingerPointingIsShow) CloseFingerPointing(); //如果提示没关掉，就自己关掉

            Vector2 viewport = Camera.main.WorldToViewportPoint(target.position);
            Vector2 referenceResolution = notificationsPanelScreen.mCtrl.GetComponent<CanvasScaler>().referenceResolution;
            Vector2 uiPos = new Vector2(viewport.x * referenceResolution.x, viewport.y * referenceResolution.y);

            notificationsPanelScreen.mCtrl.go_FingerPointing.GetComponent<RectTransform>().anchoredPosition = uiPos;
            notificationsPanelScreen.mCtrl.go_FingerPointing.SetActive(true);
            if (needFloow)
            {
                notificationsPanelScreen.mCtrl.fingerPointingCoroutine.Add(notificationsPanelScreen.mCtrl.StartCoroutine(notificationsPanelScreen.mCtrl.FingerPointingFllowTarget(target)));
            }

            notificationsPanelScreen.FingerPointingIsShow = true;
        }

        public static void CloseFingerPointing()
        {
            NotificationsPanelScreen notificationsPanelScreen = ELUIManager.Ins.GetUI<NotificationsPanelScreen>();
            notificationsPanelScreen.mCtrl.go_FingerPointing.SetActive(false);
            notificationsPanelScreen.mCtrl.StopFingerPointingCoroutine();
            notificationsPanelScreen.FingerPointingIsShow = false;
        }
    }
}