namespace Game.UI
{
    using EVANGELION;

    public class TopToolbarPanelScreenParam : UIOpenScreenParameterBase
    {
    }

    public class TopToolbarPanelScreen : ScreenBase
    {
        public override string mResName => UIConst.TopToolbarPanel;
        TopToolbarPanelCtrl mCtrl;
        TopToolbarPanelScreenParam mParam;

        protected override void  OnLoadSuccess()
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