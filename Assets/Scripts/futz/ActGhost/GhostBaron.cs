using Foundational;
using Idealist;
using Regent.Syncers;
using Regent.Workers;
using Swoonity.CSharp;
using UnityEngine;

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


        [Run.Native(FRAME)]
        static void Run_Timer(GhostActivity act)
        {
	        if (act.Phase.Current != GhostActivity.PhaseEnum.PLAYING_ROOM)
	        {
		        act.TimerString.ChangeDiff("");
		        return;
	        }

	        var dt = Time.deltaTime;

	        if (act.TimeLeftSec <= act.Fig.TimerSlowThreshold)
	        {
		        dt *= act.Fig.TimerSlowMulti;
	        }

	        act.TimeLeftSec -= dt;
	        
	        if (act.TimeLeftSec < 0) act.TimeLeftSec = 0;


	        var mins = (act.TimeLeftSec / 60).FloorToInt();
	        var secs = $"{(act.TimeLeftSec % 60).FloorToInt()}".PadLeft(2, '0');
	        
	        var str = $"{mins}:{secs}";
	        act.TimerString.ChangeDiff(str);
        }
    }
}