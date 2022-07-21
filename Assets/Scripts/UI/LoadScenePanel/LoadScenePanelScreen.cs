using System;
using System.Collections;

namespace Game.UI
{
    using EVANGELION;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;

    public class LoadScenePanelScreenParam : UIOpenScreenParameterBase
    {
        public LoadSceneName mLoadSceneName;
        public LoadSceneMode mode;

        public UnityAction OnComplete;
    }

    public class LoadScenePanelScreen : ScreenBase
    {
        LoadScenePanelCtrl mCtrl;
        LoadScenePanelScreenParam mParam;

        public override string mResName
        {
            get => UIConst.LoadScenePanel;
        }

        protected override void OnLoadSuccess()
        {
            mCtrl = mCtrlBase as LoadScenePanelCtrl;
            mParam = mOpenParam as LoadScenePanelScreenParam;


            mCtrl.StartCoroutine(AsyncLoadScene());
        }


        private IEnumerator AsyncLoadScene()
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(mParam.mLoadSceneName.ToString(), mParam.mode);
            while (ao.isDone != true)
            {
                if (mCtrl.slider_processBar != null)
                {
                    mCtrl.slider_processBar.currentPercent = ao.progress * 100;
                }

                yield return new WaitForEndOfFrame();
            }

            mCtrl.txt_Label.text = "场景加载完成";
            mCtrl.slider_Loop.GetComponent<Animator>().enabled = false;
            mCtrl.slider_processBar.currentPercent = 100;
            mCtrl.slider_Loop.bar.fillAmount = 1;
            yield return new WaitForEndOfFrame();
            GC.Collect();
            Resources.UnloadUnusedAssets();
            mParam.OnComplete?.Invoke();
            ELUIManager.Ins.CloseUI<LoadScenePanelScreen>();
        }
        


        /// <summary>
        /// 加载实验场景使用
        /// </summary>
        /// <param name="nextSceneName"></param>
        /// <param name="mode"></param>
        public static void LoadSingleScene(LoadSceneName nextSceneName, UnityAction OnComplete = null)
        {
            
            ELUIManager.Ins.OpenUI<LoadScenePanelScreen>(new LoadScenePanelScreenParam()
            {
                mLoadSceneName = nextSceneName, mode = LoadSceneMode.Single,
                OnComplete=OnComplete,
            });
        }
    }
}