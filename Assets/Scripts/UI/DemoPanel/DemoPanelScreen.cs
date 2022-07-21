namespace Game.UI
{
    using EVANGELION;
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

        protected override void OnLoadSuccess()
        {
            mCtrl = mCtrlBase as DemoPanelCtrl;
            mParam = mOpenParam as DemoPanelScreenParam;

            
        }

    }
}