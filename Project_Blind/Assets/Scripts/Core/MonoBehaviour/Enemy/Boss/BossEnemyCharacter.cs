using System;

namespace Blind
{
    public class BossEnemyCharacter : EnemyCharacter
    {
        
        protected enum State
        {
            Phase,
            Hitted,
            Stun,
        };

        protected State state;

        protected void Init()
        {
            base.Init();
            state = State.Phase;
        }

        protected virtual void FixedUpdate()
        {
            // switch (state)
            // {
            //     case State.Phase:
            //         updatePhase();
            //         break;
            //     case State.Hitted:
            //         updateHitted();
            //         break;
            //     case State.Stun:
            //         updateStun();
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException();
            // }
        }

        protected virtual void updatePhase()
        {
            throw new NotImplementedException();
        }
        
        protected virtual void updateHitted()
        {
            throw new NotImplementedException();
        }
        protected virtual void updateStun()
        {
            throw new NotImplementedException();
        }
        
    }
}