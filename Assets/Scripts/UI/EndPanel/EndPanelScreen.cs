namespace Game.UI
{
    using Cysharp.Threading.Tasks;
    using EVANGELION;
    using UnityEngine;

    public class EndPanelScreenParam : UIOpenScreenParameterBase
    {
        /// <summary>
        /// 是否透明，true则透明底，false则为纯黑底
        /// </summary>
        public bool isTransparent;

        /// <summary>
        /// 提示内容
        /// </summary>
        public string content;
    }

    public class EndPanelScreen : ScreenBase
    {
        EndPanelCtrl mCtrl;
        EndPanelScreenParam mParam;

        public override string mResName
        {
            get => UIConst.EndPanel;
        }

#pragma warning disable CS1998
        protected override async UniTask OnLoadSuccess()
#pragma warning restore CS1998
        {
            mCtrl = mCtrlBase as EndPanelCtrl;
            mParam = mOpenParam as EndPanelScreenParam;
            mCtrl.txt_EndPanelTXTContent.text = mParam.content;
            if (!mParam.isTransparent)
            {
                mCtrl.ig_Mask.color = Color.black;
            }
        }

    }
}