using System;
using Cysharp.Threading.Tasks;

namespace Common.Loading
{
    public interface ILoadingStage
    {
        public event Action<float> Loaded;
        public string Description { get; }
        public UniTask Load();
    }
}