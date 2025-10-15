using System;
using System.Collections.Generic;

namespace Game.Source.Utils
{
    public interface IDescriptionProvider
    {
        public string GetDescription(ItemState state);
    }
    public class DescriptionComposer
    {
        public List<IDescriptionProvider> all = new();

        public void Init()
        {
            var allTypes = ReflectionUtil.FindAllSubclassesIncludingInterfaces<IDescriptionProvider>();
            foreach (var t in allTypes)
                all.Add(Activator.CreateInstance(t) as IDescriptionProvider);
        }

        public List<T> FindAll<T>()
        {
            return DescriptionCache<T>.FindAll(this);
        }
    }

    public static class DescriptionCache<T>
    {
        public static List<T> all;

        public static List<T> FindAll(DescriptionComposer composer)
        {
            if (all != null)
                return all;
            all = new List<T>(64); foreach(var a in composer.all)
                if (a is T ast)
                    all.Add(ast);
            return all;
        }
    }
}