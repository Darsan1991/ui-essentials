using System;
using System.Collections.Generic;
using System.Linq;

namespace DGames.Essentials.UI
{
    public static class UIService
    {
        private static readonly List<UIManager> _managers = new();

        public static void RegisterUIManager(UIManager manager)
        {
            if (_managers.Contains(manager))
                throw new InvalidOperationException();
            _managers.Add(manager);
        }

        public static void RegisterUIManagerIfNotAlready(UIManager manager)
        {
            if (_managers.Contains(manager))
            {
                return;
            }
            
            RegisterUIManager(manager);
        }

        public static void RemoveUIManager(UIManager manager)
        {
            _managers.Remove(manager);
        }

        public static T GetPanel<T>(string tag=null) where T : Panel => _managers.Select(m => m.Panels).SelectMany(ps => ps).OfType<T>().FirstOrDefault(p=>string.IsNullOrEmpty(tag) || p.Tag == tag);
        public static IEnumerable<T> GetPanels<T>() where T : Panel => _managers.Select(m => m.Panels).SelectMany(ps => ps).OfType<T>();
    }
}