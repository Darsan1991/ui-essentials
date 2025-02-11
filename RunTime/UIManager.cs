using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DGames.Essentials.UI
{
    // ReSharper disable once HollowTypeName
    public class UIManager : MonoBehaviour
    {
        [SerializeField] protected List<Panel> panels = new();

        public IEnumerable<Panel> Panels => panels;

        public T GetPanel<T>(string panelTag=null) where T : Panel => GetPanels<T>().FirstOrDefault(p=> string.IsNullOrEmpty(panelTag) || p.Tag == panelTag);

        public IEnumerable<T> GetPanels<T>() where T : Panel
        {
            return Panels.OfType<T>();
        }


        protected virtual void Awake()
        {
            panels.AddRange(GetComponentsInChildren<Panel>(true).Except(panels));
            foreach (var panel in Panels)
            {
                panel.Manager = this;
            }
            
            UIService.RegisterUIManagerIfNotAlready(this);
        }


        protected virtual void OnDestroy()
        {
            UIService.RemoveUIManager(this);
        }
    }
}