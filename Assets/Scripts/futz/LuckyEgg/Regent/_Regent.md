# Regent

#### Stage Domain Order

- Native
- Server
- Client
- Author
- Remote

#### React Workers

Supported signatures:

- (cog)
- (cog, val)
- (cog, val, otherComponent1)
- (cog, val, otherComponent1, otherComponent2)
- (cog, val, otherComponent1, otherComponent2, otherComponent3)

## Optimizations

- [ ] Stage & worker iteration
- [ ] Entity/cog caching
- [ ] Runtime initialization
- [ ] Sanity check memory (entity caches, workers, etc.)
- [ ] somewhat concerned with every new worker needing to check added entities
- [ ] pooled entities could track which workers are relevant (skipping AddIfRelevant check on every
  worker)
- [ ] entity could have Flags for most-used cogs to speed up worker AddIfRelevant checks?

## Open Questions

- [ ] Baron/Worker required clips: what if a Baron was turned off until req Clips were present?
- [ ] Perf check Entity component cache vs Unity's GetComponent
- [ ] Could most/all of RegentCatalog be replaced with static generic type cache?
    - barons yes, cogs yes, syncers yes on cog, stages no (but could change to support)
- [no] Could most/all Clips be converted to a Definition/SO?
    - maybe have a master RegentClipSomething with a ref to each (maybe do Zoo thing above?)
    - what to do about clips with scene refs? not actually that many
    - actually this won't work due to some clips having syncers (net and native)
- [ ] Observe worker idea
    - Sort of similar to React, but works on an "observable" component
    - Runs worker whenever anything on observable component changes
    - Can target a Syncer<TypeOfCog>
    - Marks observable as Observed (and un-marks later)
    - This would be useful for UI, e.g. target tooltip needs to update in real time
- [ ] could clips use Member/Field worker attribute?
- [ ] Maybe change static clips to be passed in as first arg
    - maybe Q is blackboard/query-er
        - `static void Run_Thing(Q q, Thing thing)`
        - `q.Input.Can(...)`
        - or Z = zoo
    - Could define Baron with a type of Q/blackboard
        - `public class ThingBaron : WabiBaron<Q_Cooking>`
    - Baron could have optional Q/clip funcs
        - `override bool CanRun(Q q) => IsValid(q.Clip1, q.Clip2, ...)` (workers only run when true)
        - `override void Init(Q q, ThingClip clip) => clip.Foo = true; ...` (called when clip is set
          on Q)
- [ ] possible entity/worker optimization for worker `AddIfRelevant` checking every entity
    - generate hash for entity based on its components, i.e. an Archetype
    - when a worker checks interest in entity, first check a hashset of known archetypes (hashset is
      fast lookup)
    - if not in known archetypes, maybe check "bad" hashset?
    - then do full interest check, add to archetype hashset if relevant
    - is this actually faster? component lookup is already a hashset
    - also what about author/server/etc. checks, see: `IsInterestedIn`

## TODO

### <=2022 leftovers

- [ ] better validations for syncer workers
- [ ] Worker: support multiple attributes on same function? (define multiple workers)
- [ ] Worker: support `async UniTaskVoid` instead of `void`
- [ ] Worker: support worker targeting a base class in signature?
- [ ] Worker: add Verify attribute alias specifically for Declare (to make it more clear)
- [ ] Worker: validate Author workers if target can ever have authority (has liaison?)
- [ ] Worker: RunAll (passes in cog list or whatever)
- [ ] need clarity on ordering of same stage workers (within same Baron & other Barons)
- [ ] (re)enable hash collision checking

### 2023 Feb

- [ ] [Registry] worker documentation
- [ ] [Timed] worker NEEDS TESTING
- [ ] Validation: use reflection to test worker creation (so it fails before running)
- [ ] maybe allow naming worker functions the same? see: InvokeMaker_Method
- [ ] Spawning: Regent should have an explicit "Instantiate" method that ensures the SPAWN stage is
  used
- [ ] [Syncers] auto initialize on owner awake/etc. (instead of constantly checking IsInitialized)

### 2023 May

- [x] Worker: check existing entities when adding ENABLED or REGISTRY workers
- [ ] Coordinator could be simplified to not care about worker types (or fewer types)
    - push more timing logic to workers
    - queued entities (spawned) is still awkward, workers could have a separate "process spawned"
      call at start of frame

### 2023 September

- [ ] Worker creation & validation could be moved to Attributes to simplify? (move to sep files)
- [ ] Entity.GetOrThrow time starts to add up, iterating on a Registry is faster
  - should RunWorker be more like Registry? or use a shared Registry?
