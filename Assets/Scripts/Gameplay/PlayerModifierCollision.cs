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
    /// <typeparam name="PlayerModifierCollision"></typeparam>
    public class PlayerModifierCollision : Simulation.Event<PlayerModifierCollision>
    {
        public ModifierController modifier;
        public PlayerController player;
        public string newModifier;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            newModifier = modifier.modifierType;
            Debug.Log(newModifier);
            if (newModifier == "wormhole")
            {
                if (modifier.isLinked == false)
                {
                    player.activeModifier = "wormhole";
                }
                else
                {
                    Debug.Log("Teleporting Player");
                    player.transform.position = modifier.getWormholeExit().transform.position;
                }
            }
            else
            {
                if(!player.modifiers.Contains(newModifier))
                {
                    player.modifiers.Add(newModifier);
                }
            }
        }
    }
}