using DG.Tweening;

namespace Game.UI
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using EVANGELION;
    using PathologicalGames;
    using UnityEngine;
    using System;
    public class ExercisePanelScreenParam : UIOpenScreenParameterBase
    {
        public string exerciseName;
        public Action<string, int> commitCallBack;
    }

    public class ExercisePanelScreen : ScreenBase
    {
        ExercisePanelCtrl mCtrl;
        ExercisePanelScreenParam mParam;

        public override string mResName => UIConst.ExercisePanel;


        private LabQuestData currentLabQuestData = null;

        protected override async UniTask OnLoadSuccess()
        {
            base.OnLoadSuccess();
            mCtrl = mCtrlBase as ExercisePanelCtrl;
            mParam = mOpenParam as ExercisePanelScreenParam;
            currentLabQuestData =  Resources.Load<LabQuestData>("Configs/Exercise/"+mParam.exerciseName);
            //初始化实验习题
            InitQuestion();
            mCtrl.panel.DOFade(1, 0.5f);
        }

        public static async UniTask ShowExercise(string exerciseName, Action<string, int> commitCallBack)
        {
            await ELUIManager.Ins.OpenUI<ExercisePanelScreen>(new ExercisePanelScreenParam()
            {
                exerciseName = exerciseName,
                commitCallBack = commitCallBack,
            });
        }

        #region 实验习题

        private static Dictionary<string, bool> LabQuestIsCommit = new Dictionary<string, bool>();
        private static Dictionary<string, List<List<ChoiceIndexStr>>> UserChoice = new Dictionary<string, List<List<ChoiceIndexStr>>>();

        private void InitQuestion()
        {
            if (!LabQuestIsCommit.ContainsKey(mParam.exerciseName))
                LabQuestIsCommit.Add(mParam.exerciseName, false);
            if (!UserChoice.ContainsKey(mParam.exerciseName))
                UserChoice.Add(mParam.exerciseName, new List<List<ChoiceIndexStr>>());
            InitLabQuest();
            InitLabQuestCommit();
            mCtrl.btn_LabQuestRefresh.onClick.AddListener(LabQuestRefresh);
            mCtrl.btn_LabQuestClose.onClick.AddListener(CloseExersise);
        }

        private void CloseExersise()
        {
            if (LabQuestIsCommit[mParam.exerciseName])
                ELUIManager.Ins.CloseUI<ExercisePanelScreen>();
            else
                ModalWindowPanelScreen.OpenModalWindowNoTabs("系统提示", "知识考核结果未提交前退出不会保存已选择结果哦~", true, () => { ELUIManager.Ins.CloseUI<ExercisePanelScreen>(); });
        }

        public List<LabQuestDataItem> labQuestDataItems = new List<LabQuestDataItem>();

        /// <summary>
        /// 初始化习题
        /// </summary>
        protected void InitLabQuest()
        {
            //清除labQuestDataItems引用
            labQuestDataItems.Clear();
            //加载试题开始生成
            List<Question> quests = currentLabQuestData.questions; //LabQuestData
            SpawnPool spawnPool = PoolManager.Pools["LabQuest"];
            Transform labQuestItem = spawnPool.prefabs["LabQuestItem"];
            for (int i = 0; i < quests.Count; i++)
            {
                labQuestDataItems.Add(spawnPool.Spawn(labQuestItem, mCtrl.trans_LabQuestParent).UIPoolSpawnedResetY()
                    .GetComponent<LabQuestDataItem>().Init(quests[i], i + 1));
            }
        }

        /// <summary>
        /// 根据习题是否提交进行习题初始化
        /// </summary>
        protected void InitLabQuestCommit()
        {
            if (LabQuestIsCommit[mParam.exerciseName])
            {
                ReloadLabQuestChoice();
                mCtrl.btn_LabQuestCommit.interactable = false;
            }
            else
            {
                mCtrl.btn_LabQuestCommit.onClick.AddListener(VerificationLabQuest);
            }
        }

        /// <summary>
        /// 重新加载用户选项
        /// </summary>
        protected void ReloadLabQuestChoice()
        {
            for (int i = 0; i < labQuestDataItems.Count; i++)
            {
                labQuestDataItems[i].ReloadLabQuestChoice(UserChoice[mParam.exerciseName][i]);
                labQuestDataItems[i].Sumbit();
            }
        }


        /// <summary>
        /// 检查用户是否已经做完习题
        /// </summary>
        /// <returns>返回true为已经全部都进行选择，返回false说明有试题未进行选择</returns>
        protected bool CheckUserIsChoice()
        {
            for (int i = 0; i < labQuestDataItems.Count; i++)
            {
                if (!labQuestDataItems[i].CheckUserIsCheck())
                {
                    return false;
                }
            }

            return true;
        }


        protected void VerificationLabQuest()
        {
            if (!CheckUserIsChoice())
            {
                ModalWindowPanelScreen.OpenModalWindowNoTabs("试题提示", "存在试题未完成，是否确认提交试题？", true, CommitLabQuest);
            }
            else
            {
                CommitLabQuest();
            }
        }

        /// <summary>
        /// 提交实验习题
        /// </summary>
        protected void CommitLabQuest()
        {
            int rightNum = 0;
            for (int i = 0; i < labQuestDataItems.Count; i++)
            {
                if (labQuestDataItems[i].Sumbit())
                {
                    rightNum++;
                }

                UserChoice[mParam.exerciseName].Add(labQuestDataItems[i].SaveLabQuestChoice());
            }

            mCtrl.btn_LabQuestCommit.onClick.RemoveAllListeners();
            mCtrl.btn_LabQuestCommit.interactable = false;

            LabQuestIsCommit[mParam.exerciseName] = true;

            mParam.commitCallBack?.Invoke(mParam.exerciseName, rightNum);
        }

        protected void LabQuestRefresh()
        {
            ModalWindowPanelScreen.OpenModalWindowNoTabs("试题提示",
                LabQuestIsCommit[mParam.exerciseName] ? "重置后所有习题将被重置并且习题模块已得分将清零，继续重置么？" : "重置后当前所有已选择选项将被重置，继续重置么？", true,
                ResetAllLabQuest);
        }

        protected void ResetAllLabQuest()
        {
            PoolManager.Pools["LabQuest"].DespawnAll();

            mCtrl.btn_LabQuestCommit.interactable = true;
            LabQuestIsCommit[mParam.exerciseName] = false;
            UserChoice[mParam.exerciseName].Clear();
            InitLabQuest();
            InitLabQuestCommit();
        }

        #endregion
    }
}