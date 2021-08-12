namespace MidnightCoder.Game
{
    public class FSMStateInfo
    {
        //
        // Fields
        //
        public FSMStateEventDg enter;

        public FSMStateEventDg update;

        public FSMStateEventDg exit;

        //
        // Constructors
        //
        public FSMStateInfo(FSMStateEventDg _enter, FSMStateEventDg _update, FSMStateEventDg _exit)
        {
            this.enter = _enter;
            this.update = _update;
            this.exit = _exit;
        }
    }
}
