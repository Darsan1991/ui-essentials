namespace DGames.Essentials.UI
{
    public  class SingletonUIManager<T> : UIManager where T:SingletonUIManager<T>
    {
        public static T Instance { get; private set; }
        protected override void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)this;
                DontDestroyOnLoad(gameObject);
                base.Awake();
                OnInit();

            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnInit()
        {
        }

        public static TJ Panel<TJ>() where TJ:Panel => Instance != null ? Instance.GetPanel<TJ>() : null;
    }
}