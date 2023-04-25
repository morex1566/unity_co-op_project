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
        
        public abstract void HandleInput(ref LevelPlayer player, ref PlayerActionState actionState);
        public virtual void Update(ref LevelPlayer player, ref PlayerActionState actionState){}

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
        
        public override void HandleInput(ref LevelPlayer player, ref PlayerActionState actionState)
        {
            if (Input.GetKeyDown(InputSetting.LeftSlash))
            {
                actionState = new LeftSlash(ref player, ref actionState);
            }
            else
            if (Input.GetKeyDown(InputSetting.RightSlash))
            {
                actionState = new RightSlash(ref player, ref actionState);
            }
        }
    
        // TODO : 애니메이션 관련 정보 여기에, ex) 다시 idle로 돌아오는 애니메이션
        private void action()
        {
        
        }
    }

    public class RightSlash : PlayerActionState
    {
        private IEnumerator<bool> _timeout;

        public RightSlash(ref LevelPlayer player, ref PlayerActionState actionState)
        {
            action(ref player);
            _timeout = revertIdle();
        }
        
        public override void HandleInput(ref LevelPlayer player, ref PlayerActionState actionState)
        {
            if (Input.GetKeyDown(InputSetting.LeftSlash))
            {
                actionState = new LeftSlash(ref player, ref actionState);
            }
            else
            if (Input.GetKeyDown(InputSetting.RightSlash))
            {
                actionState = new RightSlash(ref player, ref actionState);
            }
        }

        public override void Update(ref LevelPlayer player, ref PlayerActionState actionState)
        {
            _timeout.MoveNext();
            
            if (_timeout.Current)
            {
                actionState = new Idle();
            }
        }
    
        private void action(ref LevelPlayer player)
        { 
            
        }
    }

    public class LeftSlash : PlayerActionState
    {
        private IEnumerator<bool> _timeout;

        public LeftSlash(ref LevelPlayer player, ref PlayerActionState actionState)
        {
            action(ref player);
            _timeout = revertIdle();
        }

        public override void HandleInput(ref LevelPlayer player, ref PlayerActionState actionState)
        {
            if (Input.GetKeyDown(InputSetting.LeftSlash))
            {
                actionState = new LeftSlash(ref player, ref actionState);
            }
            else
            if (Input.GetKeyDown(InputSetting.RightSlash))
            {
                actionState = new RightSlash(ref player, ref actionState);
            }
        }
        
        public override void Update(ref LevelPlayer player, ref PlayerActionState actionState)
        {
            _timeout.MoveNext();
            
            if (_timeout.Current)
            {
                actionState = new Idle();
            }
  
        }
    
        private void action(ref LevelPlayer player)
        {
            
        }
    }
}