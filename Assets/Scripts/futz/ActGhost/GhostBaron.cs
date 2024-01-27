using Foundational;
using Idealist;
using Regent.Syncers;
using Regent.Workers;

namespace futz.ActGhost
{
    public class GhostBaron : FutzBaron
    {
        public static GhostActivity Act_;
        // [Registry] public static Registry<Actor> Actors_ = new();
        // [Registry] public static Registry<BewilderCard> Cards_ = new();
        // public static List<BewilderCard> Cards_ = new();

        [Added.Native(POST_SPAWN)]
        static void Enabled_Actor(GhostActor actor)
        {
            Act_.Actors.Add(actor);
            GhostLogic.ActorAdded(Act_, actor);
        }

        [Disabled.Native]
        static void Disabled_Actor(GhostActor actor)
        {
            Act_?.Actors.Remove(actor);
            GhostLogic.ActorRemoved(Act_, actor);
        }


        [Enabled.Native]
        static void Enabled_Card(GhostCard card) => Act_.Cards.Add(card);

        [Disabled.Native]
        static void Disabled_Card(GhostCard card) => Act_?.Cards.Remove(card);


        [Enabled.Native]
        static void Enabled_Bewilder(GhostActivity act)
        {
            Act_ = act;
            GhostLogic.Initialize(act);
        }

        [Disabled.Native]
        static void Disabled_Bewilder(GhostActivity act)
        {
            Act_ = act;
            GhostLogic.CleanUp(act);
        }

        // [Added.Native(POST_SPAWN)]
        // static void Added_Actor(Actor actor) => Logic.ActorAdded(Act_, actor);
        //
        // [Disabled.Native]
        // static void Disabled_Actor(Actor actor) => Logic.ActorRemoved(Act_, actor);

        [Run.Native(ACTIVITY_PK)]
        static void Drain_Packets(GhostActivity act) => GhostLogic.DrainPackets(act);

        [React.Native(UNSURE_WHAT_STAGE, nameof(GhostActivity.ForceNextRound))]
        static void React_ForceNextRound(GhostActivity act) => GhostLogic.ForceNextRound(act);


        [React.Native(UNSURE_WHAT_STAGE, nameof(GhostActor.AssignedHints))]
        static void React_AssignedClues(GhostActor actor, TrackList<Hint> hints)
        {
	        // var hints = actor.AssignedHints.Current;
	        var hintsString = hints.Current.Join(h => h.Message, "|");
	        
	        Act_.PkHints.SendTo(
		        actor,
		        new Pk_GhostHints {
			        Hints = hintsString,
		        }
	        );
        }
    }
}