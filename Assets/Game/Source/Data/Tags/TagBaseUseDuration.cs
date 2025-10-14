using System.Collections.Generic;

namespace Game.Source.Tags
{
    public class TagBaseUseDuration : EntityComponentDefinition
    {
        public List<float> Duration;

        public float Get(TagItemLevel itemLevel)
        {
            return Duration[itemLevel.Level];
        }
    }
}