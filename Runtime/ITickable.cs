namespace StdNounou.Tick
{
    public interface ITickable
    {
        public abstract void OnTick(int tick);
        public abstract int RemainingTicks();
        public abstract float RemainingTimeInSeconds();
    } 
}