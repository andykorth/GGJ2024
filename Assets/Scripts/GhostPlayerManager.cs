using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using TMPro;
using System.Linq;
using Foundational;
using futz.ActGhost;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class GhostPlayerManager : Singleton<GhostPlayerManager>
{
    [HideInInspector]
    public bool readyToBegin = false;
    public GameObject pressPlayToBeginMsg;
    public GameObject needTwoPhonePlayersMsg;

    public TMPro.TMP_Text ghostText;
    public TMPro.TMP_Text goalDebugText;

    public List<GhostPlayer>  ghostPlayers;

    public List<Goal> allSharedGoals;
    public List<Goal> assignedGoals;

    public string[] spiritNames;

    [FormerlySerializedAs("ghostsToFade")]
    public SpriteRenderer[] spiritsToFade;
    public ExitDoor exitDoor;

    public const int SPIRIT_COUNT = 3;

    public void CreateAllGoals(){
        allSharedGoals = new List<Goal>
        {
            new Goal {
                goalString = "I hate the smell of burning cauldrons.",
                goalAction = () => MatchesInteraction(InteractableObject.InteractableType.Cauldron) == 0,
                spiritIndex = 0,
            },
            new Goal {
                goalString = "But I can't stand the dark.",
                goalAction = () => MatchesInteraction(InteractableObject.InteractableType.Candle) >= 1,
                spiritIndex = 0,
            },
            new Goal {
                goalString = "Ooooh that vase reminds me of limes!",
                goalAction = () => MatchesInteraction(InteractableObject.InteractableType.Vase, InteractableObject.InteractableColor.Green) >= 1,
                spiritIndex = 0,
            },


            new Goal {
                goalString = "I always loved pottery.",
                goalAction = () => InteractableObject.allInteractables
                    .Where( (a) => a.interactableType == InteractableObject.InteractableType.Vase )
                    .Where( a => a.interactableColor != InteractableObject.InteractableColor.Blue )
                    .All( a => a.hasBeenEnabledByPlayer ),
                spiritIndex = 1,
            },
            new Goal {
                goalString = "That blue vase is an embarrassment.",
                goalAction = () => MatchesInteraction(InteractableObject.InteractableType.Vase, InteractableObject.InteractableColor.Blue) == 0,
                spiritIndex = 1,
            },
            new Goal {
                goalString = "I do love blue though!",
                goalAction = () => MatchesInteraction(InteractableObject.InteractableType.Cauldron, InteractableObject.InteractableColor.Blue) >= 1,
                spiritIndex = 1,
            },


            new Goal {
                goalString = "Ewww, purple!",
                goalAction = () => InteractableObject.allInteractables
                    .Where( (a) => a.interactableType == InteractableObject.InteractableType.Vase )
                    .Where( a => a.interactableColor != InteractableObject.InteractableColor.Purple )
                    .All( a => a.hasBeenEnabledByPlayer ),
                spiritIndex = 2,
            },
            new Goal {
                goalString = "I didn't know there was a painting of me.",
                goalAction = () => MatchesInteraction(InteractableObject.InteractableType.Portrait, InteractableObject.InteractableColor.Blue) > 0,
                spiritIndex = 2,
            },
            new Goal {
                goalString = "These vases are all hideous!",
                goalAction = () => InteractableObject.allInteractables
                    .Where( (a) => a.interactableType == InteractableObject.InteractableType.Vase )
                    .All( a => a.hasBeenEnabledByPlayer ),
                spiritIndex = 2,
            },

            
            // new Goal {
            //     goalString = "Extinguish all candles.",
            //     goalAction = () => Player.i.InteractCount(InteractableObject.InteractableType.Candle) <= 0
            // },
            // new Goal {
            //     goalString = "Straighten one portrait.",
            //     goalAction = () => Player.i.InteractCount(InteractableObject.InteractableType.Portrait) <= 0
            // },
            // new Goal {
            //     goalString = "I hate crooked paintings!",
            //     goalAction = () => InteractableObject.allInteractables.All(
            //         (a) => a.interactableType != InteractableObject.InteractableType.Portrait
            //         || a.hasBeenEnabledByPlayer )
            //         // that means all the portraits must be "enabled".
            // },
            
        };
    }
    internal int MatchesInteraction(InteractableObject.InteractableType interactionType, InteractableObject.InteractableColor color = InteractableObject.InteractableColor.Any)
    {
        int count = 0;
        foreach(var v in InteractableObject.allInteractables){
            bool matchType = v.interactableType == interactionType;
            bool matchColor = color == InteractableObject.InteractableColor.Any
                || v.interactableColor == color;
            if(matchType && matchColor && v.hasBeenEnabledByPlayer) {
                count += 1;
            }
        }
        return count;
    }

    public void Start(){
        ghostPlayers = new List<GhostPlayer>();
        CreateAllGoals();

        pressPlayToBeginMsg.SetActive(false);
        needTwoPhonePlayersMsg.SetActive(true);
        readyToBegin = false;
        Player.i.gameObject.SetActive(false);
    }

    private float timeTilUpdate = 1.0f;
    public void Update(){

        if(Input.GetKeyDown(KeyCode.C)){
            needTwoPhonePlayersMsg.SetActive(false);
            pressPlayToBeginMsg.SetActive(false);
            GameStarted();
        }

        if(readyToBegin){
            needTwoPhonePlayersMsg.SetActive(false);
            pressPlayToBeginMsg.SetActive(true);
            if(Input.GetKeyDown(KeyCode.Space)){
                GameStarted();
            }
        }

        timeTilUpdate -= Time.deltaTime;
        if(timeTilUpdate < 0f){
            EvaluateGoals();
            timeTilUpdate = 1.0f;
        }
        
        string s = $"Ghosts: {ghostPlayers.Count}\n";
        foreach(var ghost in ghostPlayers){
            s += "   " + ghost.ToString() + "\n";
        }

        ghostText.text = s;
    }

    public string goalDebugString;
    public void EvaluateGoals(){
        if(assignedGoals == null) return;

        goalDebugString = "Debugging String:\n";

        int[] spiritGoalCount = new int[SPIRIT_COUNT];
        int[] spiritGoalComplete = new int[SPIRIT_COUNT];
        foreach(Goal goal in assignedGoals){
            spiritGoalCount[goal.spiritIndex] += 1;
            bool b = goal.Evaluate();
            goalDebugString += (b ? "YES" : "NO" ) + $" spirit {goal.spiritIndex}: {goal.goalString}\n";
            if(b){
                spiritGoalComplete[goal.spiritIndex] += 1;
            }
        }
        // update spirits:
        for(int i = 0; i < SPIRIT_COUNT; i++){
            Color c = spiritsToFade[i].color;
            float alpha = spiritGoalComplete[i] / (float) spiritGoalCount[i];
            spiritsToFade[i].color = c.WithAlpha(alpha);
            Debug.Log($"At {Time.time} spirit {i} = {alpha}");
        }

        if(goalDebugText != null) goalDebugText.text = goalDebugString;
    }


    public void GameStarted(){
        readyToBegin = false;
        Player.i.gameObject.SetActive(true);
        pressPlayToBeginMsg.SetActive(false);
        GhostLogic.BeginRoom(GameSysClip.I.GhostAct.Current).Forget();
        
        var actors = GameSysClip.I.GhostAct.Current.Actors.Current;

        ghostPlayers = new List<GhostPlayer>();
        int goalsPerPlayer = (int) Mathf.Ceil(9.0f / actors.Count);

        assignedGoals = new List<Goal>();
        assignedGoals.AddRange(allSharedGoals);

        foreach(var phonePlayer in actors){
            var ghost = new GhostPlayer
            {
                name = phonePlayer.Nickname,
                actor = phonePlayer
            };

            ghost.ClearHints();
            for(int i = 0; i < goalsPerPlayer; i ++){
                if(allSharedGoals.Count > 0){
                    var goal = allSharedGoals[Random.Range(0, allSharedGoals.Count)];
                    ghost.AssignGoal(goal);
                    allSharedGoals.Remove(goal);
                }
            }

            ghostPlayers.Add(ghost);
        }
   

    }
    
}

public class GhostPlayer {

    public string name;
    public int goalsComplete = 0;
    public List<Goal> activeGoals = new List<Goal>();
    internal GhostActor actor;
    
    public override string ToString(){
        return $"{name} score: {goalsComplete}";
    }

    internal void AssignGoal(Goal goal)
    {
        activeGoals.Add(goal);
        var hint = new Hint();
        hint.Message = GhostPlayerManager.i.spiritNames[goal.spiritIndex] + ": " +  goal.goalString;
        Debug.Log("Assign hint to " + this.name + ": " + hint.Message);

        actor.AssignedHints.Add(hint);
    }

    internal void ClearHints()
    {
        actor.AssignedHints.Clear();
    }
}

public class Goal {
    public string goalString;
    public System.Func<bool> goalAction;
    public bool hasBeenFinished = false;
    /// <summary>
    /// spirit index of -1 means it controls the exit.
    /// </summary>
    public int spiritIndex;


    public bool Evaluate(){
        bool b = goalAction();
        Debug.Log("Goal " + goalString + " = " + b);

        if(b){
            if(!hasBeenFinished){
                // first time finishing this goal!
                Debug.Log($"Finished goal for spirit {spiritIndex}: {goalString}");
            }
            hasBeenFinished = true;
        }
        return b;
    }

}
