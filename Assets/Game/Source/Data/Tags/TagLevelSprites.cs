using System.Collections.Generic;
using UnityEngine;

namespace Game.Source.Tags
{
    public class TagLevelSprites : EntityComponentDefinition
    {
        [SerializeReference] public List<Sprite> LevelSprites;
    }
}