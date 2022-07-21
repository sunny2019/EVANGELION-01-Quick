using System;

namespace Game
{
    using UnityEngine;
    using EVANGELION;
    using UI;

    [DisallowMultipleComponent]
    public class Init : MonoBehaviour
    {
        private async void Awake()
        {
            ExpData. firstEnterTime=DateTime.Now;
#if DEBUG
             ELDebugger.Init();
#endif
             ELAudioManager.Init();
             ELUIManager.Init();
            
            
             LoadScenePanelScreen.LoadSingleScene(LoadSceneName.Scene_Main, () =>
             {
                 ModalWindowPanelScreen.OpenModalWindowNoTabs("提示", "场景加载完成", true, null, false);
             });
             
        }
    }
}