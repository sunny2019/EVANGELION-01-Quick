namespace Game.UI
{
    using EVANGELION;

    public class HttpRequestPanelScreenParam : UIOpenScreenParameterBase
    {
        public string content;
    }

    public class HttpRequestPanelScreen : ScreenBase
    {
        public override string mResName
        {
            get => UIConst.HttpRequestPanel;
        }

        HttpRequestPanelCtrl mCtrl;
        HttpRequestPanelScreenParam mParam;

        protected override void OnLoadSuccess()
        {
            mCtrl = mCtrlBase as HttpRequestPanelCtrl;
            mParam = mOpenParam as HttpRequestPanelScreenParam;
            mCtrl.content.text = mParam.content;
        }
    }
}