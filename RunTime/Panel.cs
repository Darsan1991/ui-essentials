using UnityEngine;

namespace DGames.Essentials.UI
{
    public class Panel : ShowHidable
    {
        [SerializeField] private string _tag;

        public string Tag => _tag;
        
        public UIManager Manager { get; set; }


        public T GetPanel<T>(UIManager manager,string panelTag="") where T : Panel
        {
            if (manager)
            {
                return manager.GetPanel<T>(panelTag);
            }

            return Manager.GetPanel<T>() ?? UIService.GetPanel<T>();
        }
        
        public T GetPanel<T>(bool allowGlobal = false) where T : Panel
        {
            return Manager.GetPanel<T>() ?? (allowGlobal ? UIService.GetPanel<T>() : null);
        }
    }
}