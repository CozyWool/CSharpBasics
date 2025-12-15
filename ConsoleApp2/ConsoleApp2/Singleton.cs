namespace ConsoleApp2
{
    public class Singleton
    {
        private static Singleton _instance;

        private Singleton()
        {
        }

        public static Singleton GetInstance()
        {
            return _instance ??= new Singleton();
        }
    }
}