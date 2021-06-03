namespace SnekWrastler
{
    public class SnekConfig
    {
        public DirectionKeys DirectionKeys;
        public StartSettings StartSettings;
    }

    public class DirectionKeys
    {
        public string Up;
        public string Down;
        public string Left;
        public string Right;
    }

    public class StartSettings
    {
        public int Length;
        public int Speed;
        public int SpeedStep;
        public int FoodCount;
    }
}