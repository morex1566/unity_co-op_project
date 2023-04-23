using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelPlayerAction
{
    public abstract class PlayerActionState
    {
        private readonly float _revertTime;
        public PlayerActionState()
        {
            _revertTime = 0.5f;
        }
        
        public abstract void HandleInput(ref PlayerActionState actionState);
        public virtual void Update(ref PlayerActionState actionState){}
        protected abstract void action();
        
        protected IEnumerator<bool> revertIdle()
        {
            float elapsedTime = 0.0f;
    
            while (elapsedTime < _revertTime)
            {
                elapsedTime += Time.deltaTime;
                yield return false;
            }

            yield return true;
        }
    }

    public class Idle : PlayerActionState
    {
        public Idle()
        {
            action();
        }
        
        public override void HandleInput(ref PlayerActionState actionState)
        {
            if (Input.GetKeyDown(InputSetting.leftSlash))
            {
                actionState = new LeftSlash(ref actionState);
            }
            else
            if (Input.GetKeyDown(InputSetting.rightSlash))
            {
                actionState = new RightSlash(ref actionState);
            }
        }
    
        // TODO : 애니메이션 관련 정보 여기에, ex) 다시 idle로 돌아오는 애니메이션
        protected sealed override void action()
        {
        
        }
    }

    public class RightSlash : PlayerActionState
    {
        private IEnumerator<bool> _timeout;

        public RightSlash(ref PlayerActionState actionState)
        {
            action();
            _timeout = revertIdle();
        }
        
        public override void HandleInput(ref PlayerActionState actionState)
        {
            if (Input.GetKeyDown(InputSetting.leftSlash))
            {
                actionState = new LeftSlash(ref actionState);
            }
            else
            if (Input.GetKeyDown(InputSetting.rightSlash))
            {
                actionState = new RightSlash(ref actionState);
            }
        }

        public override void Update(ref PlayerActionState actionState)
        {
            _timeout.MoveNext();
            
            if (_timeout.Current)
            {
                actionState = new Idle();
            }
        }
    
        protected sealed override void action()
        { 
            
        }
    }

    public class LeftSlash : PlayerActionState
    {
        private IEnumerator<bool> _timeout;
        public LeftSlash(ref PlayerActionState actionState)
        {
            action();
            _timeout = revertIdle();
        }

        public override void HandleInput(ref PlayerActionState actionState)
        {
            if (Input.GetKeyDown(InputSetting.leftSlash))
            {
                actionState = new LeftSlash(ref actionState);
            }
            else
            if (Input.GetKeyDown(InputSetting.rightSlash))
            {
                actionState = new RightSlash(ref actionState);
            }
        }
        
        public override void Update(ref PlayerActionState actionState)
        {
            _timeout.MoveNext();
            
            if (_timeout.Current)
            {
                actionState = new Idle();
            }
        }
    
        protected sealed override void action()
        {
           
        }
    }
}