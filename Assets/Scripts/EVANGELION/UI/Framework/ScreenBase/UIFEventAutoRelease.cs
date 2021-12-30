namespace EVANGELION
{
    using System.Collections.Generic;
    using Sirenix.OdinInspector;

    public class UIFEventAutoRelease : SerializedMonoBehaviour
    {
        protected List<IRelease> releaseIDList = new List<IRelease>(); //FEvent句柄管理器

        protected virtual void OnDestroy()
        {
            RemoveAllListener();
        }

        #region Auto Release Listener

        public void AutoRelease(IRelease release)
        {
            releaseIDList.Add(release);
        }

        private void RemoveAllListener()
        {
            foreach (var releaseID in releaseIDList)
            {
                releaseID.Release();
            }
        }

        #endregion
    }
}