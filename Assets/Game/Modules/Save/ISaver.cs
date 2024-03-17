namespace Game.Modules.Save
{
    public interface ISaver<T>
    {
        public void Save(T value);
        public T Load();
        public void Delete();
    }
}
