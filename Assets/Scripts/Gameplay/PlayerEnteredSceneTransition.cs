using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{

    /// <summary>
    /// This event is triggered when the player character enters a trigger with a SceneTransition component.
    /// </summary>
    /// <typeparam name="PlayerEnteredVictoryZone"></typeparam>
    public class PlayerEnteredSceneTransition : Simulation.Event<PlayerEnteredSceneTransition>
    {
        //public newScene newScene;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            Debug.Log("Scene Change!");
          //  model.player.animator.SetTrigger("victory");
          //  model.player.controlEnabled = false;
        }
    }
}