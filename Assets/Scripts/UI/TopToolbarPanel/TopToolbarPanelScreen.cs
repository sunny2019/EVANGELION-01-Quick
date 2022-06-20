namespace Game.UI
{
    using EVANGELION;
    using Cysharp.Threading.Tasks;

    public class TopToolbarPanelScreenParam : UIOpenScreenParameterBase
    {
    }

    public class TopToolbarPanelScreen : ScreenBase
    {
        public override string mResName => UIConst.TopToolbarPanel;
        TopToolbarPanelCtrl mCtrl;
        TopToolbarPanelScreenParam mParam;

        protected override async UniTask OnLoadSuccess()
        {
            mCtrl = mCtrlBase as TopToolbarPanelCtrl;
            mParam = mOpenParam as TopToolbarPanelScreenParam;

            mCtrl.btn_Back.onClick.AddListener(() =>
            {
                ELUIManager.Ins.CloseAllUI();
                LoadScenePanelScreen.LoadSingleScene(LoadSceneName.Scene_Main, () =>
                {
                    
                });
            });
            
        }

       
    }
}