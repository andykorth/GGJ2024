using Foundational;
using Idealist;
using Regent.Syncers;
using Regent.Workers;

namespace futz.ActGhost
{
    public class GhostBaron : FutzBaron
    {
        public static GhostActivity Act_;
        static GameSysClip GameSys_ => GameSysClip.I;

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
        static void Enabled_Ghost(GhostActivity act)
        {
            Act_ = act;
            GameSys_.GhostAct.Change(act); // HACK
            GhostLogic.Initialize(act);
        }

        [Disabled.Native]
        static void Disabled_Ghost(GhostActivity act)
        {
            Act_ = act;
            GhostLogic.CleanUp(act);
        }

        [Run.Native(ACTIVITY_PK)]
        static void Drain_Packets(GhostActivity act) => GhostLogic.DrainPackets(act);

        // [React.Native(UNSURE_WHAT_STAGE, nameof(GhostActivity.ForceNextRound))]
        // static void React_ForceNextRound(GhostActivity act) => GhostLogic.ForceNextRound(act);


        [React.Native(UNSURE_WHAT_STAGE, nameof(GhostActor.AssignedHints))]
        static void React_AssignedHints(GhostActor actor, TrackList<Hint> hints)
        {
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