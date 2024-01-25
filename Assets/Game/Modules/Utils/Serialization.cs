namespace Game.Modules.Utils
{
    [System.Serializable]
    public class Serialization<T>
    {
        public T data;

        public Serialization(T data)
        {
            this.data = data;
        }
    }
}