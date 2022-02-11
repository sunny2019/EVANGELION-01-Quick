

namespace Game.UI
{
    using EVANGELION;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;
    using TMPro;
    using UnityEngine.AddressableAssets;
    using UnityEngine.EventSystems;

    public class MoudleChoicePanelScreenParam : UIOpenScreenParameterBase
    {
    }

    public class MoudleChoicePanelScreen : ScreenBase
    {
        public override string mResName => UIConst.MoudleChoicePanelScreen;
        MoudleChoicePanelCtrl mCtrl;
        MoudleChoicePanelScreenParam mParam;

        protected override async UniTask OnLoadSuccess()
        {
            mCtrl = mCtrlBase as MoudleChoicePanelCtrl;
            mParam = mOpenParam as MoudleChoicePanelScreenParam;
            EffectListener();

            mCtrl.expBasisContent= await Addressables.LoadAssetAsync<MultiContentTipsPanelScreen.DetailItemContent>("DetailItemContent_实验基础");
            ETTool.Add(mCtrl.btn_ExpBasis, EventTriggerType.PointerClick, (eventData) =>
            {
                if (mCtrl.expBasisContent!=null)
                    MultiContentTipsPanelScreen.ShowPageWindowContent(mCtrl.expBasisContent.pageContents);
            });
            ETTool.Add(mCtrl.btn_Exercise, EventTriggerType.PointerClick, (eventData) =>
            {
                ExercisePanelScreen.ShowExercise("LabQuestData_习题",((s, i) =>
                {
                    Debug.Log("习题集："+s+ "\t笼共答对数量：" + i);
                }));
            });
            ETTool.Add(mCtrl.btn_Report, EventTriggerType.PointerClick, (eventData) =>
            {
                ELUIManager.Ins.OpenUI<ReportPanelScreen>();
            });
            mCtrl.moudle1Content= await Addressables.LoadAssetAsync<MultiContentTipsPanelScreen.DetailItemContent>("DetailItemContent_基础病认知");
            ETTool.Add(mCtrl.btn_Moudle1, EventTriggerType.PointerClick, (eventData) =>
            {
                if (mCtrl.moudle1Content!=null)
                    MultiContentTipsPanelScreen.ShowPageWindowContent(mCtrl.moudle1Content.pageContents);
            });
            ETTool.Add(mCtrl.btn_Moudle2, EventTriggerType.PointerClick, (eventData) => { });
            ETTool.Add(mCtrl.btn_Moudle3, EventTriggerType.PointerClick, (eventData) => { });
            ETTool.Add(mCtrl.btn_Moudle4, EventTriggerType.PointerClick, (eventData) => { });
        }
        


        #region 效果代码
        
        private void EffectListener()
        {
            ETTool.Add(mCtrl.btn_ExpBasis, EventTriggerType.PointerEnter, OnEnterToolBarBtn);
            ETTool.Add(mCtrl.btn_ExpBasis, EventTriggerType.PointerExit, OnExitToolBarBtn);
            ETTool.Add(mCtrl.btn_Exercise, EventTriggerType.PointerEnter, OnEnterToolBarBtn);
            ETTool.Add(mCtrl.btn_Exercise, EventTriggerType.PointerExit, OnExitToolBarBtn);
            ETTool.Add(mCtrl.btn_Report, EventTriggerType.PointerEnter, OnEnterToolBarBtn);
            ETTool.Add(mCtrl.btn_Report, EventTriggerType.PointerExit, OnExitToolBarBtn);
            ETTool.Add(mCtrl.btn_Moudle1, EventTriggerType.PointerEnter, OnEnterMoudleBtn);
            ETTool.Add(mCtrl.btn_Moudle1, EventTriggerType.PointerExit, OnExitMoudleBtn);
            ETTool.Add(mCtrl.btn_Moudle2, EventTriggerType.PointerEnter, OnEnterMoudleBtn);
            ETTool.Add(mCtrl.btn_Moudle2, EventTriggerType.PointerExit, OnExitMoudleBtn);
            ETTool.Add(mCtrl.btn_Moudle3, EventTriggerType.PointerEnter, OnEnterMoudleBtn);
            ETTool.Add(mCtrl.btn_Moudle3, EventTriggerType.PointerExit, OnExitMoudleBtn);
            ETTool.Add(mCtrl.btn_Moudle4, EventTriggerType.PointerEnter, OnEnterMoudleBtn);
            ETTool.Add(mCtrl.btn_Moudle4, EventTriggerType.PointerExit, OnExitMoudleBtn);
            ETTool.Add(mCtrl.img_umbrella, EventTriggerType.PointerEnter, OnEnterMoudleBtn);
            ETTool.Add(mCtrl.img_umbrella, EventTriggerType.PointerExit, OnExitMoudleBtn);
        }

        private void OnEnterToolBarBtn(BaseEventData arg0)
        {
            PointerEventData PEData = arg0 as PointerEventData;
            PEData.pointerEnter.GetComponentInParent<Button>().GetComponentInChildren<CanvasGroup>().DOFade(1f,0.5f);

        }

        private void OnExitToolBarBtn(BaseEventData arg0)
        {
            PointerEventData PEData = arg0 as PointerEventData;
            PEData.pointerEnter.GetComponentInParent<Button>().GetComponentInChildren<CanvasGroup>().DOFade(0,0.5f);
        }

        private void OnEnterMoudleBtn(BaseEventData arg0)
        {
            PointerEventData PEData = arg0 as PointerEventData;
            PEData.pointerEnter.transform.DOScale(1.2f, 0.5f);
        }

        private void OnExitMoudleBtn(BaseEventData arg0)
        {
            PointerEventData PEData = arg0 as PointerEventData;
            PEData.pointerEnter.transform.DOScale(1, 0.5f);
        }
        
        #endregion
    }
}