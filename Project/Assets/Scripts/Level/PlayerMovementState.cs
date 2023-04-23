using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelPlayerMovement
{
    public abstract class PlayerMovementState
    {
        public abstract void HandleInput(ref LevelPlayer player, ref PlayerMovementState currentState);
        public virtual void Update(ref LevelPlayer player, ref PlayerMovementState movementState){}
        protected abstract void move(ref LevelPlayer player);
    }

    
    public class Idle : PlayerMovementState
    {
        public override void HandleInput(ref LevelPlayer player, ref PlayerMovementState currentState)
        {
            if (Input.GetKeyDown(InputSetting.MoveRight))
            {
                currentState = new RightMove();
            }
            else 
            if (Input.GetKeyDown(InputSetting.MoveLeft))
            {
                currentState = new LeftMove();
            }
            else 
            if (Input.GetKeyDown(InputSetting.Jump))
            {
                currentState = new Jump(ref player);
            }
            else 
            if (Input.GetKeyDown(InputSetting.Slide))
            {
                currentState = new Slide();
            }
        }

        protected override void move(ref LevelPlayer player)
        {
            
        }
    }

    
    public class LeftMove : PlayerMovementState
    {
        public override void HandleInput(ref LevelPlayer player, ref PlayerMovementState currentState)
        {
            if (Input.GetKeyDown(InputSetting.MoveRight))
            {
                currentState = new RightMove();
            }
            else 
            if (Input.GetKeyDown(InputSetting.Jump))
            {
                currentState = new Jump(ref player);
            }
            else 
            if (Input.GetKeyDown(InputSetting.Slide))
            {
                currentState = new Slide();
            }
        }
        
        public override void Update(ref LevelPlayer player, ref PlayerMovementState currentState)
        {
            if (!Input.anyKeyDown)
            {
                currentState = new Idle();
                return;
            }

            move(ref player);
        }

        protected override void move(ref LevelPlayer player)
        {
            
        }
    }


    public class RightMove : PlayerMovementState
    {
        public override void HandleInput(ref LevelPlayer player, ref PlayerMovementState currentState)
        {
            if (Input.GetKeyDown(InputSetting.MoveLeft))
            {
                currentState = new LeftMove();
            }
            else 
            if (Input.GetKeyDown(InputSetting.Jump))
            {
                currentState = new Jump(ref player);
            }
            else 
            if (Input.GetKeyDown(InputSetting.Slide))
            {
                currentState = new Slide();
            }
        }
        
        public override void Update(ref LevelPlayer player, ref PlayerMovementState currentState)
        {
            if (!Input.anyKeyDown)
            {
                currentState = new Idle();
                return;
            }

            move(ref player);
        }

        protected override void move(ref LevelPlayer player)
        {
            
        }
    }

    
    public class Jump : PlayerMovementState
    {
        // INFO : 2단점프를 막기위한 변수
        private bool _isCheckable = false;
        
        public Jump(ref LevelPlayer player)
        {
            move(ref player);
        }
        
        public override void HandleInput(ref LevelPlayer player, ref PlayerMovementState currentState)
        {
            
        }
        
        public override void Update(ref LevelPlayer player, ref PlayerMovementState currentState)
        {
            if (!Input.anyKeyDown && _isCheckable && IsGrounded(ref player))
            {
                currentState = new Idle();
            }
        }

        protected override void move(ref LevelPlayer player)
        {
            Rigidbody rigidbody = player.GetRigidbody();

            rigidbody.AddForce(new Vector3(0, player.JumpPower, 0));
            CoroutineUtility.Initialize();
            CoroutineUtility.StartCoroutine(SetIsCheckable());
        }

        // INFO : 점프 후, 땅에 닿았는지 확인
        private bool IsGrounded(ref LevelPlayer player)
        {
            float rayLength = 0.1f;
    
            int groundLayerMask = LayerMask.GetMask("Default");
    
            RaycastHit hit;
            bool isGrounded = Physics.Raycast(player.FootPosition, Vector3.down, out hit, rayLength, groundLayerMask);
    
            return isGrounded;
        }

        private IEnumerator SetIsCheckable()
        {
            yield return new WaitForSeconds(0.5f); // 1초 대기
            _isCheckable = true;
        }
    }

    
    public class Slide : PlayerMovementState
    {
        public override void HandleInput(ref LevelPlayer player, ref PlayerMovementState currentState)
        {
            if (Input.GetKeyDown(InputSetting.MoveRight))
            {
                currentState = new RightMove();
            }
            else 
            if (Input.GetKeyDown(InputSetting.MoveLeft))
            {
                currentState = new LeftMove();
            }
        }
        
        public override void Update(ref LevelPlayer player, ref PlayerMovementState currentState)
        {
            if (!Input.anyKeyDown)
            {
                currentState = new Idle();
                return;
            }
        }

        protected override void move(ref LevelPlayer player)
        {
            
        }
    }
}