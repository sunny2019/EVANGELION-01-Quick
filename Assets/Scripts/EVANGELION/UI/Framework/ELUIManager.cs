

namespace EVANGELION
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.Rendering.Universal;
    using UnityEngine.SceneManagement;

    public class ELUIManager : MonoSingleton<ELUIManager>
    {
        public GameObject uiRoot;

        [ReadOnly] [ShowInInspector]
        // UI列表缓存
        Dictionary<Type, ScreenBase> mTypeScreens = new Dictionary<Type, ScreenBase>();

        public int mUIOpenOrder = 0; // UI打开时的Order值 用来标识界面层级顺序

        // uicamera
        Camera uiCamera;
        private Canvas canvas;

        public static Camera UiCamera
        {
            get => Ins.uiCamera;
        }


        public static Vector3 MouseWorldPosition
        {
            get
            {
                Vector3 uipos = Ins.uiCamera.ScreenToWorldPoint(Input.mousePosition);
                return uipos;
            }
        }

        public static Vector3 MouseScreenPosition
        {
            get { return Input.mousePosition; }
        }


        protected override async UniTask OnInit()
        {
            // 初始化UI根节点
            uiRoot = (await Addressables.InstantiateAsync("UIRoot", transform).Task).gameObject;
            
            uiRoot.transform.position=Vector3.zero;

            uiCamera = uiRoot.GetComponentInChildren<Camera>();

            SceneManager.sceneLoaded += delegate(Scene arg0, LoadSceneMode mode)
            {
                Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(uiCamera);
            };
            Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(uiCamera);
        }

        /// <summary>
        ///  UI打开入口没有判断条件直接打开
        /// </summary>
        private async UniTask<ScreenBase> OpenUI(Type type, UIOpenScreenParameterBase param = null)
        {
            ScreenBase sb = GetUI(type);
            mUIOpenOrder++;

            // 如果已有界面,则不执行任何操作
            if (sb != null)
            {
                if (sb.CtrlBase != null && !sb.CtrlBase.ctrlCanvas.enabled)
                {
                    sb.CtrlBase.ctrlCanvas.enabled = true;
                }

                // 处理最上层界面
                if (sb.CtrlBase.mHideOtherScreenWhenThisOnTop)
                {
                    ProcessUIOnTop();
                }

                return sb;
            }

            //sb = (ScreenBase) Activator.CreateInstance(type, param);


            sb = (ScreenBase) Activator.CreateInstance(type);
            await sb.StartLoad(param);
            
                
            mTypeScreens.Add(type, sb);
            sb.SetOpenOrder(mUIOpenOrder); // 设置打开序号

            // 处理最上层界面
            if (sb.CtrlBase.mHideOtherScreenWhenThisOnTop)
            {
                ProcessUIOnTop();
            }

            return sb;
        }

        /// <summary>
        ///  UI打开入口没有判断条件直接打开
        /// </summary>
        public async UniTask<TScreen> OpenUI<TScreen>(UIOpenScreenParameterBase param = null) where TScreen : ScreenBase
        {
            Type type = typeof(TScreen);
            return  (TScreen) await OpenUI(type, param);
        }

        /// <summary>
        /// UI外部调用的获取接口
        /// </summary>
        private ScreenBase GetUI(Type type)
        {
            if (!typeof(ScreenBase).IsAssignableFrom(type)) return default;
            ScreenBase sb = null;
            if (mTypeScreens.TryGetValue(type, out sb))
                return sb;
            return null;
        }

        /// <summary>
        /// UI外部调用的获取接口
        /// </summary>
        public TScreen GetUI<TScreen>() where TScreen : ScreenBase
        {
            Type type = typeof(TScreen);

            return (TScreen) GetUI(type);
        }

        /// <summary>
        /// UI外部调用的关闭接口
        /// </summary>
        private bool CloseUI(Type type)
        {
            ScreenBase sb = GetUI(type);
            if (sb != null)
            {
                if (type == typeof(ScreenBase)) // 标尺界面是测试界面 不用关闭
                    return false;
                else
                    sb.OnClose();
                return true;
            }

            return false;
        }

        /// <summary>
        /// UI外部调用的关闭接口
        /// </summary>
        public bool CloseUI<TScreen>() where TScreen : ScreenBase
        {
            Type type = typeof(TScreen);
            return CloseUI(type);
        }


        public void CloseAllUI()
        {
            // 销毁会从容器中删除 不能用正常遍历方式
            List<Type> keys = new List<Type>(mTypeScreens.Keys);
            foreach (var k in keys)
            {
                if (k == typeof(ScreenBase)) // 标尺界面是测试界面 不用关闭
                {
                    continue;
                }

                if (mTypeScreens.ContainsKey(k))
                    mTypeScreens[k].OnClose();
            }
        }

        /// <summary>
        /// UI创建时候自动处理的UI打开处理 一般不要手动调用
        /// </summary>
        public void AddUI(ScreenBase sBase)
        {
            sBase.mPanelRoot.transform.SetParent(GetUIRootTransform());

            sBase.mPanelRoot.transform.localPosition = Vector3.zero;
            sBase.mPanelRoot.transform.localScale = Vector3.one;
            sBase.mPanelRoot.transform.localRotation = Quaternion.identity;
            sBase.mPanelRoot.name = sBase.mPanelRoot.name.Replace("(Clone)", "");

            // 处理最上层界面 如果是异步加载界面建议逻辑写在这里
            if (sBase.CtrlBase.mHideOtherScreenWhenThisOnTop)
            {
                ProcessUIOnTop();
            }
        }

        /// <summary>
        /// UI移除时候自动处理的接口 一般不要手动调用
        /// </summary>
        public void RemoveUI(ScreenBase sBase)
        {
            if (mTypeScreens.ContainsKey(sBase.GetType())) // 根据具体需求决定到底是直接销毁还是缓存
                mTypeScreens.Remove(sBase.GetType());
            sBase.Dispose();

            // 处理最上层界面
            if (sBase.CtrlBase.mHideOtherScreenWhenThisOnTop)
            {
                ProcessUIOnTop();
            }
        }

        // 处理最上层的界面逻辑
        List<ScreenBase> sortTemp = new List<ScreenBase>();

        void ProcessUIOnTop()
        {
            sortTemp.Clear();
            foreach (var s in mTypeScreens.Values)
            {
                sortTemp.Add(s);
            }

            // 排序 按照层级高->低的顺序
            sortTemp.Sort(
                (a, b) =>
                {
                    if (a.mSortingLayer == b.mSortingLayer)
                    {
                        return b.mOpenOrder.CompareTo(a.mOpenOrder);
                    }

                    return b.mSortingLayer.CompareTo(a.mSortingLayer);
                });

            // 先找到第一个控制的UI层
            int index = 0;

            for (int i = 0; i < sortTemp.Count; i++)
            {
                var tempC = sortTemp[i];
                if (tempC.CtrlBase.mHideOtherScreenWhenThisOnTop)
                {
                    // 找到第一个需要被隐藏的界面 隐藏就好
                    tempC.CtrlBase.ctrlCanvas.enabled = true;
                    index = i; // 因为是一个有序的List 所以找到第一个需要控制的界面之后记录序号，然后从它接着遍历即可
                    break;
                }
            }

            // 如果没有找到 可能的情况是就是关闭了最上层界面 所以现在最上层的应该是空的
            if (index == 0)
            {
                for (int i = 0; i < sortTemp.Count; i++)
                {
                    var tempC = sortTemp[i];
                    // 找到第一个需要被隐藏的界面 隐藏就好
                    if (!tempC.CtrlBase.ctrlCanvas.enabled)
                    {
                        tempC.CtrlBase.ctrlCanvas.enabled = true;
                        index = i; // 因为是一个有序的List 所以找到第一个需要控制的界面之后记录序号，然后从它接着遍历即可
                        break;
                    }
                }
            }

            // 找到下面需要隐藏的 
            for (int i = index + 1; i < sortTemp.Count; i++)
            {
                var tempC = sortTemp[i];
                if (!tempC.CtrlBase.mAlwaysShow)
                {
                    // 找到需要被隐藏的界面 隐藏就好
                    tempC.CtrlBase.ctrlCanvas.enabled = false;
                }
            }
        }



        #region 通用API

        //获取UIRoot节点
        public Transform GetUIRootTransform()
        {
            return transform;
        }

        public Camera GetUICamera()
        {
            return uiCamera;
        }


        #endregion
    }
}