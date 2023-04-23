
namespace LevelPlayerMovement
{
    public abstract class PlayerMovementState
    {
        public abstract void HandleInput();
        public virtual void Update(){}
        protected abstract void move();
    }

    
    public class Idle : PlayerMovementState
    {
        public override void HandleInput()
        {
            
        }

        protected override void move()
        {
            
        }
    }

    
    public class LeftMove : PlayerMovementState
    {
        public override void HandleInput()
        {
            
        }

        protected override void move()
        {
            
        }
    }


    public class RightMove : PlayerMovementState
    {
        public override void HandleInput()
        {
            
        }

        protected override void move()
        {
            
        }
    }

    
    public class Jump : PlayerMovementState
    {
        public override void HandleInput()
        {
            
        }

        protected override void move()
        {
            
        }
    }

    
    public class Slide : PlayerMovementState
    {
        public override void HandleInput()
        {
            
        }

        protected override void move()
        {
            
        }
    }
}