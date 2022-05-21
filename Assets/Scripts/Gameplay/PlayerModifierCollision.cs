using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{

    /// <summary>
    /// Fired when a Player collides with a Modifier.
    /// </summary>
    /// <typeparam name="EnemyCollision"></typeparam>
    public class PlayerModifierCollision : Simulation.Event<PlayerModifierCollision>
    {
        public ModifierController modifier;
        public PlayerController player;
        public string newModifier;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            newModifier = modifier.modifierType;
            player.modifier = newModifier;
        }
    }
}