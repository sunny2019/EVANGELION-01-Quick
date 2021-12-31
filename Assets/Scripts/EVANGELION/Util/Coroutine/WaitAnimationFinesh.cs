namespace EVANGELION
{
    using UnityEngine;

    public class WaitAnimationFinesh : CustomYieldInstruction
    {
        public override bool keepWaiting
        {
            get { return animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1 && animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName); }
        }

        private Animator animator;
        private string stateName;
        private int layer;

        public WaitAnimationFinesh(Animator animator, string stateName, int layer)
        {
            this.animator = animator;
            this.stateName = stateName;
            this.layer = layer;
        }
    }
}