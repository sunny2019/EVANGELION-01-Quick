

namespace EVANGELION
{
    using UnityEngine;

    public class ScreenBase
    {
        public virtual string mResName { get; }
        public GameObject mPanelRoot = null;
        protected UICtrlBase mCtrlBase;

        public int mOpenOrder = 0; // 界面打开顺序
        public int mSortingLayer = 0; // 界面层级

        // 界面打开的传入参数
        protected UIOpenScreenParameterBase mOpenParam;

        public UICtrlBase CtrlBase
        {
            get => mCtrlBase;
        }

       
        public  void StartLoad( UIOpenScreenParameterBase param = null)
        {
            
            mOpenParam = param;
            
            var ctrl = Object.Instantiate(Resources.Load<GameObject>("Prefabs/UI/"+mResName),ELUIManager.Ins.GetUIRootTransform());

            
             PanelLoadComplete(ctrl);
        }

        // 资源加载完成
        void PanelLoadComplete(GameObject ctrl)
        {
            mPanelRoot = ctrl;
            // 获取控件对象
            mCtrlBase = mPanelRoot.GetComponent<UICtrlBase>();

            // 更新层级信息
            UpdateLayoutLevel();

            // 调用加载成功方法
             OnLoadSuccess();

            // 添加到控制层
            ELUIManager.Ins.AddUI(this);
        }

        // 脚本处理完成
        protected virtual  void OnLoadSuccess()
        {
            
        }


        public virtual void OnClose()
        {
            ELUIManager.Ins.RemoveUI(this);
        }


        // 设置渲染顺序
        public void SetOpenOrder(int openOrder)
        {
            mOpenOrder = openOrder;
            if (mCtrlBase != null && mCtrlBase.ctrlCanvas != null)
            {
                mCtrlBase.ctrlCanvas.sortingOrder = openOrder;
            }
        }

        // 更新UI的层级
        private void UpdateLayoutLevel()
        {
            var camera = ELUIManager.Ins.GetUICamera();
            if (camera != null)
            {
                mCtrlBase.ctrlCanvas.worldCamera = camera;
            }

            mCtrlBase.ctrlCanvas.pixelPerfect = true;
            mCtrlBase.ctrlCanvas.overrideSorting = true;
            mCtrlBase.ctrlCanvas.sortingLayerID = (int) mCtrlBase.sceenPriority;
            mSortingLayer = (int) mCtrlBase.sceenPriority;
            mCtrlBase.ctrlCanvas.sortingOrder = mOpenOrder;
        }

        public virtual void Dispose()
        {
            Object.Destroy(mPanelRoot);
        }
    }
}