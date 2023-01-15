using Cysharp.Threading.Tasks;

namespace Common.Saving
{
    public interface IDataProvider<T>
    {
        public UniTask<T> Load();
        public void Save();
    }
}