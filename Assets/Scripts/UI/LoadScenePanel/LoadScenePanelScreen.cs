namespace Game.UI
{
    using Cysharp.Threading.Tasks;
    using EVANGELION;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;

    public class LoadScenePanelScreenParam : UIOpenScreenParameterBase
    {
        public LoadSceneName mLoadSceneName;
        public LoadSceneMode mode;
    }

    public class LoadScenePanelScreen : ScreenBase
    {
        LoadScenePanelCtrl mCtrl;
        LoadScenePanelScreenParam mParam;

        public override string mResName
        {
            get => UIConst.LoadScenePanel;
        }

        protected override async UniTask OnLoadSuccess()
        {
            mCtrl = mCtrlBase as LoadScenePanelCtrl;
            mParam = mOpenParam as LoadScenePanelScreenParam;

            LoadSceneName sceneName = mParam.mLoadSceneName;
            LoadSceneMode model = mParam.mode;
            //yield return null;
            AsyncOperationHandle<SceneInstance> async = Addressables.LoadSceneAsync(sceneName.ToString(), model);
            while (async.Status != AsyncOperationStatus.Succeeded)
            {
                if (mCtrl.slider_processBar != null)
                {
                    mCtrl.slider_processBar.currentPercent = async.PercentComplete * 100;
                }

                await UniTask.WaitForEndOfFrame();
            }

            mCtrl.txt_Label.text = "场景加载完成";
            mCtrl.slider_Loop.GetComponent<Animator>().enabled = false;
            mCtrl.slider_processBar.currentPercent = 100;
            mCtrl.slider_Loop.bar.fillAmount = 1;
            await UniTask.WaitForEndOfFrame();
        }


        /// <summary>
        /// 加载实验场景使用
        /// </summary>
        /// <param name="nextSceneName"></param>
        /// <param name="mode"></param>
        public static async UniTask LoadSingleScene(LoadSceneName nextSceneName, UnityAction onComplete = null)
        {
            await ELUIManager.Ins.OpenUI<LoadScenePanelScreen>(new LoadScenePanelScreenParam()
            {
                mLoadSceneName = nextSceneName, mode = LoadSceneMode.Single,
            });
            onComplete?.Invoke();
            ELUIManager.Ins.CloseUI<LoadScenePanelScreen>();
        }
    }
}