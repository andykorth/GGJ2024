using System.Collections.Generic;
using Foundational;
using Regent.Core;
using Regent.Workers;
using Logic = ActBewilder.BewilderLogic;
using Act = ActBewilder.BewilderActivity;
using Actor = ActBewilder.BewilderActor;
using static UnityEngine.Debug;

namespace ActBewilder
{
    public class BewilderBaron : FutzBaron
    {
        public static Act Act_;
        // [Registry] public static Registry<Actor> Actors_ = new();
        // [Registry] public static Registry<BewilderCard> Cards_ = new();
        // public static List<BewilderCard> Cards_ = new();

        [Added.Native(POST_SPAWN)]
        static void Enabled_Actor(Actor actor)
        {
            Act_.Actors.Add(actor);
            Logic.ActorAdded(Act_, actor);
        }

        [Disabled.Native]
        static void Disabled_Actor(Actor actor)
        {
            Act_?.Actors.Remove(actor);
            Logic.ActorRemoved(Act_, actor);
        }


        [Enabled.Native]
        static void Enabled_Card(BewilderCard card) => Act_.Cards.Add(card);

        [Disabled.Native]
        static void Disabled_Card(BewilderCard card) => Act_?.Cards.Remove(card);


        [Enabled.Native]
        static void Enabled_Bewilder(Act act)
        {
            Act_ = act;
            Logic.Initialize(act);
        }

        [Disabled.Native]
        static void Disabled_Bewilder(Act act)
        {
            Act_ = act;
            Logic.CleanUp(act);
        }

        // [Added.Native(POST_SPAWN)]
        // static void Added_Actor(Actor actor) => Logic.ActorAdded(Act_, actor);
        //
        // [Disabled.Native]
        // static void Disabled_Actor(Actor actor) => Logic.ActorRemoved(Act_, actor);

        [Run.Native(ACTIVITY_PK)]
        static void Drain_Packets(Act act) => Logic.DrainPackets(act);

        [React.Native(UNSURE_WHAT_STAGE, nameof(Act.ForceNextRound))]
        static void React_ForceNextRound(Act act) => Logic.ForceNextRound(act);
    }
}