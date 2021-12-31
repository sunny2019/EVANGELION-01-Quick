

namespace Game.UI
{
    using Cysharp.Threading.Tasks;
    using EVANGELION;

    public class WelcomePanelScreenParam : UIOpenScreenParameterBase
{
    public string mLoadSceneName;
}

    public class WelcomePanelScreen : ScreenBase
    {
        WelcomePanelCtrl mCtrl;
        WelcomePanelScreenParam mParam;


        public override string mResName
        {
            get => UIConst.WelcomePanel;
        }

#pragma warning disable CS1998
        protected override async UniTask OnLoadSuccess()
#pragma warning restore CS1998
        {
            mCtrl = mCtrlBase as WelcomePanelCtrl;
            mParam = mOpenParam as WelcomePanelScreenParam;
            mCtrl.MainBtn.onClick.AddListener(OpenMainPanel);

        }

        protected void OpenMainPanel()
        {
            //GameUIManager.GetInstance().OpenUI<MainPanelScreen>();
            ELUIManager.Ins.CloseUI<WelcomePanelScreen>();

        }

    }
}