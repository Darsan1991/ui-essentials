using System;

namespace DGames.Essentials.UI
{
    public interface IAtomicAnimation
    {
        event Action<IAtomicAnimation> Completed;
        
        bool Loop { get; set; }
        string Name { get; }
        float Normalized { get; }
        float Duration { get;  }
        void Play(float normalized=0f);
        void Stop();
        void Pause();
        void Resume();
        float SpeedMultiplexer { get; set; }
        Status CurrentStatus { get; }
        
        public enum Status
        {
            None,Playing,Paused
        }
    }
}