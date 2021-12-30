namespace Game
{
    using System;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using EVANGELION;
    using UI;

    [DisallowMultipleComponent]
    public class Init : MonoBehaviour
    {
        public enum AAMode
        {
            PreDownload,
            PlayingDownload,
        }

        [SerializeField] private AAMode mAAMode = AAMode.PreDownload;

        private async void Awake()
        {
#if DEBUG
            await ELDebugger.Init();
#endif
            switch (mAAMode)
            {
                case AAMode.PreDownload:
                    await ELAddressables.HotUpdate();
                    break;
                case AAMode.PlayingDownload:
                    await ELAddressables.CheckAndUpdateCataLogs();
                    break;
                default:
                    Addressables.LogError("当前AAMode:" + mAAMode + ",未进行对应模式初始化");
                    throw new ArgumentOutOfRangeException();
            }

            
             await ELUIManager.Init();
             await ELUIManager.Ins.OpenUI<DemoPanelScreen>();

            await Addressables.LoadSceneAsync("Scene_Main").Task;

            //Destroy(gameObject);
        }

        /// <summary>
        /// 在运行过程中进行资源更新
        /// </summary>
        public static async void HotUpdate()
        {
            await ELAddressables.HotUpdate(false);
            // TODO: 如果HotFixDll也更新了就需要重启应用程序
        }
    }
}