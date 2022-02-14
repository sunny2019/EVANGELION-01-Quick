namespace Game.UI
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using PathologicalGames;
    using UnityEngine;
    using UnityEngine.UI;
    using EVANGELION;
    using DG.Tweening;
    using Cysharp.Threading.Tasks;

    public class ReportPanelScreenParam : UIOpenScreenParameterBase
    {
    }

    public class ReportPanelScreen : ScreenBase
    {
        ReportPanelCtrl mCtrl;
        ReportPanelScreenParam mParam;

        public override string mResName => UIConst.ReportPanel;


        protected override async UniTask OnLoadSuccess()
        {
            base.OnLoadSuccess();
            mCtrl = mCtrlBase as ReportPanelCtrl;

            //初始化实验报告
            InitReport();
            InitRepotItems();
            mCtrl.panel.DOFade(1, 0.5f);
        }

        #region 初始化实验报告

        public static bool ReportIsCommit = false;
        public static int StartsIndex = -1;
        public static string Conclusion=string.Empty;

        private void InitReport()
        {
            mCtrl.txt_Date.text = DateTime.Now.ToString("yyyy-MM-dd");
            mCtrl.btn_Close.onClick.AddListener(() =>
            {
                if (!ReportIsCommit)
                    NotificationsPanelScreen.ShowNotifications("实验报告提示", "完成试验后不要忘记提交实验报告哦~");
                ELUIManager.Ins.CloseUI<ReportPanelScreen>();
            });

            mCtrl.input_Conclusion.text = Conclusion;
            mCtrl.input_Conclusion.onEndEdit.AddListener((arg0) =>
            {
                if (arg0.Length > 200)
                    NotificationsPanelScreen.ShowNotifications("实验报告提示", "心得体会不可超过200字,请进行修改！");
                Conclusion = arg0;
            });


            mCtrl.StarRating.selectorEvent.AddListener((index) => { StartsIndex = index; });
            if (StartsIndex != -1)
            {
                mCtrl.StarRating.defaultIndex = StartsIndex;
            }

            StartsIndex = mCtrl.StarRating.defaultIndex;
            mCtrl.btn_ReportCommit.onClick.AddListener(ReportCommit);


            if (ReportIsCommit)
            {
                Button[] btns = mCtrl.StarRating.GetComponentsInChildren<Button>();
                for (int i = 0; i < btns.Length; i++)
                {
                    btns[i].interactable = false;
                }

                mCtrl.btn_ReportCommit.interactable = false;
            }

        }

        public List<ReportDataItem> ReportDataItems = new List<ReportDataItem>();

        public void InitRepotItems()
        {
            SpawnPool spawnPool = PoolManager.Pools["Assignment module"];
            Transform ReportItem = spawnPool.prefabs["ReportItem"];

            //--------------------------------------- 根据数据生成实验得分项 --------------------------------------
            
            ReportDataItems.Add(spawnPool.Spawn(ReportItem, mCtrl.ReportList).UIPoolSpawnedResetY()
                .GetComponent<ReportDataItem>().Assignment("名称", "得分", Extension.GetTime(1000), "备注"));
            
            //---------------------------------------------------------------------------------------------------
        }

        #endregion

        #region 实验报告提交

        [DllImport("__Internal")]
        private static extern string LoadParams();

        private static extern string getUrlParams(string name);

        protected void ReportCommit()
        {
            if (Conclusion.Length < 200)
                ModalWindowPanelScreen.OpenModalWindowNoTabs("实验报告提示", "实验报告提交后不可以进行任何操作，是否确认提交？", true, () => { ReportCommitting(); });
            else
            {
                NotificationsPanelScreen.ShowNotifications("实验报告提示", "心得体会不可超过200字！");
            }
        }

        protected void ReportCommitting()
        {
#if UNITY_EDITOR
            string[] userInfos = new string[]
            {
                "评审专家",
                "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleGVyY2lzZXMiOjAsInNjaG9vbCI6ImlsYWLlubPlj7DnlKjmiLciLCJyb2xlSWQiOjQsImxvZ2luTmFtZSI6Im1qXzEzNjQ2NzkwMTQ2IiwiaWQiOjM3NywiYWNjZXNzVG9rZW4iOiJaJTJCVE8lMkZETzlJWVJHdnlFbDdGRlZhWUtuVHFISjQ5UGRIZDJBVDd3eVFzM283YVZuQlVSTTNxbXVTeWNnYVNVQzNDZG8lMkJlb1VNMk9Qdm9LZ0phQUhiUld1U1daeFYlMkJyU1R3M01TaDRhV3YzcElLV0JUSlYzNktuODZkRiUyQmxyWWEiLCJleHAiOjE2MjYxNTgwMzE2MDEsInVzZXJuYW1lIjoibWpfMTM2NDY3OTAxNDYifQ.J8mmTL2Y_UHi_mhJdzhQnY1_mfq0PbFfaCwGN52DM8c",
                "http://www.simtop.online/ZSD_SJQRHJ_II/"
            };
#else
        string[] userInfos = LoadParams().Split('~');
#endif
            Debug.Log("提交实验报告");
        }

        #endregion
    }
}