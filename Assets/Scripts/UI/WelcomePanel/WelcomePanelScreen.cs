

namespace Game.UI
{
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

        protected override void OnLoadSuccess()
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