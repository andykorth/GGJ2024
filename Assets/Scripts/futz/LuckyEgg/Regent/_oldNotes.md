## Olddddd TODO Misc

### 2021 June

- [ ] Write documentation 😅
- [ ] Property drawers for facts
- [ ] Stage/Worker: decide about sorting (in WorkerLup)
- [ ] Profile & Optimize
- [ ] More logging
- [ ] Kitchen: test network stuff more
- [ ] Baron/Worker: test enabling/disabling
- [x] Decide if barons should also be generated at runtime (or have editor helper?)
- [x] Entity: Figure out best way to do caching
    - Devtime vs runtime?
    - Currently not respecting serialization (Dictionary, System.Type, etc.)
- [ ] Validator cleanup
- [ ] Clip/Worker: solution for boilerplate `if (!Clip) return;`
    - Should it be on the Worker or Baron level?
    - Maybe an attribute?
- [x] Clip: designated initializer for Clips
    - Currently doing: `[Added.Native(EARLY)] void Added_FooClip(FooClip _) {}`
    - done: [ClipInit]
- [ ] Worker: better attribute validation (putting an invalid attribute, Run vs Do, etc.)
- [ ] Syncer: apply hashId at devtime during validation
    - There's likely a bug if you Init in a different order than declaration
    - See: InitializeSyncer
- [ ] Worker: support multiple attributes on same function? (define multiple workers)
- [ ] Worker: support `async UniTaskVoid` instead of `void`
- [ ] Bug: Do gets called before Added (used for init clips I guess?)
- [ ] Test entity destruction more (assuming it's messy at the moment)
- [x] Is it okay to have nested entities? see: Item/ItemArt
- [x] Bug: Syncers trigger reaction twice when RPC has `[ServerRpc(requireAuthority = false)]`
    - I think I fixed this by doing an equality check before triggering the reaction
- [ ] Worker: support worker targeting a base class in signature?
- [ ] Syncer: test if Write uses base class
- [x] Remove Mirage dependency in CogFactMakers & put RegentMirage in own assembly
- [ ] Worker: add Verify attribute alias specifically for Declare (to make it more clear)
- [ ] Worker: validate Author workers if target can ever have authority (has liaison?)

### 2021 July

- [x] React: add multi-component signature, so worker can use multiple cogs
    - e.g. (Cat cat, int newIntValOnCat, Fur fur, Tail tail)
- [ ] React: profile and optimize multi-component workers ^
- [ ] Observe worker idea
    - Sort of similar to React, but works on an "observable" component
    - Runs worker whenever anything on observable component changes
    - Can target a Syncer<TypeOfCog>
    - Marks observable as Observed (and un-marks later)
    - This would be useful for UI, e.g. target tooltip needs to update in real time
- [ ] Worker: RunAll (passes in cog list or whatever)

### 2022 March-April

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
- [x] Stages: make code determine order (no changing through inspector)

### 2022 May-June

~~- [ ] Should Native domain go last? When host, often Native used to update unity state after
server change~~

- [x] [Do] is probably an anti-pattern
    - [ ] it's now disabled, but needs to be cleaned up
      ~~- [ ] Formalize clip.List<Cog> pattern~~
- [ ] Optimization: faster checking worker interest in entity that has been seen before (prefab)
- [ ] Look into argument attributes (optional cogs, etc.)
- [x] Entity custom editor
    - [ ] have basic, what improvements to make?
- [ ] React: support for "no argument" signature
- [ ] Perf check Entity component cache vs Unity's GetComponent
- [ ] Could most/all of RegentCatalog be replaced with static generic type cache?
    - barons yes, cogs yes, syncers yes on cog, stages no (but could change to support)
- [no] Could most/all Clips be converted to a Definition/SO?
    - maybe have a master RegentClipSomething with a ref to each (maybe do Zoo thing above?)
    - what to do about clips with scene refs? not actually that many
    - actually this won't work due to some clips having syncers (net and native)

### 2022 Aug

- [ ] Do syncer Rpc methods allocate garbage? `cogThing.ControlChange(newVal, cogThing.Rpc_Garbage)`
- [x] Should syncers/React be innately 1 event per frame? (multi changes still trigger 1 React)
    - would simplify several things (mark EntityLup instead of flow)
    - often already used this way with Trigger / ChangeDiff
    - "queue" type syncers would need another solution, but that sounds cleaner
        - e.g. ItemClip.CueSpawn (Native<ItemSpawnInfo>) could be a special SyncerQueue, etc.
    - when processing, could skip unregistered (pending) entities but not remove (to try again next
      frame)
    - DONE: reworked syncers and React/Verify workers to be simpler
- [x] Is term "Native" confusing due to Unity's NativeList etc.?
    - could use "Track" for local syncers
    - DONE: changed to Track
- [ ] Better pooling support for Entity/CogNative
    - maybe Spawned/Despawned events separate from Added/Removed
    - Added/Removed -> Created/Destroyed

### 2022 Sep

- [ ] list/lifecycle worker (combine added/removed for common use case of "all" list/lup)
- [ ] investigate confusion over spawn & despawn stages/timing
    - does it need to be rigidly staged? queuing spawns add a lot of extra code
    - spawner "drain" functions require extra initialization info depending on where it was called
    - despawning at a specific point makes some sense for destroying objects
    - entities are ADDED only once at the start of Update
        - need special spawning stage before all others that also adds entities
    - entities get CUT at the end of every stage
        - need better Removed/Destroyed worker (run worker before destroyed so it has access to cog)
- [ ] need clarity on ordering of same stage workers (within same Baron & other Barons)
- [x] move worker validations to attribute
- [ ] better validations for syncer workers
- [ ] could clips use Member/Field worker attribute?
- [x] [Registry] worker & Registry<T>

### 2022 Oct

- [ ] allow (native) syncers to be in non-cog objects (like a state object) that exist on a cog
