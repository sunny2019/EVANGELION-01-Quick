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
#if DEBUG
            await ELDebugger.Init();
#endif
            await ELAudioManager.Init();
            await ELUIManager.Init();
            
            
            await LoadScenePanelScreen.LoadSingleScene(LoadSceneName.Scene_Main);
            await ELUIManager.Ins.OpenUI<MoudleChoicePanelScreen>();
            //Destroy(gameObject);
        }
    }
}