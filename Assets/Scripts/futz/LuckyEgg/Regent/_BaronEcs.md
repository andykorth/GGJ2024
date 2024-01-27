# TODO: skim, transfer anything good to _Regent.md, then nuke

# BaronEcs --- a hybrid ECS-inspired architecture

	Not a true ECS, but inspired by its "unwoven" structure.


	## Major Differences from a true ECS
	- An Entity is not just a floating ID, it's an actual component and holds a dictionary cache of its components.
	- The intention is to NOT add/remove components.
	- We don't get any performance boosts that a data-oriented ECS would have.
	- A Baron is more like a manager class. It can have multiple Workers and its own state.
	- Much of BaronEcs is event driven. It uses EventCuer, which implements the Observer pattern.
	- It's built to fully support Mirror, with SyncVars being the primary method of network communication.
	- 	

	## Glossary

	Baron: a manager that can have one or more Workers
	Worker: similar to an ECS "system" or processor, targets one or more components
	Entity: has id, holds a cache of its components
	Cog: a baron-friendly component (note: Workers can target any type of component, not just cogs)

	Native: not networked, local only (active on anything, offline/server/client/host)
	Client: networked, only active on a client, see NetBaron, NetCog, ClientStages
	Server: networked, only active on a server, see NetBaron, NetCog, ServerStages

	TODO TODO TODO
	TODO: this is now out of date and needs to be updated XD

	## Baron Stages 
	Set when a Worker is created, determines:
		1) domain: Local, Client, Server
		2) loop: Update, LateUpdate, FixedUpdate, or manual/custom
		3) order within that loop (INPUT comes before FRAME, etc.)
		
	 
	TODO TODO TODO
			
	internal (but public) vars/funcs are prefixed with __ (double underscore) and should not be used	
	
	 
	## TODO: Jan 2021
	⬜ Clean up above stuff ^^^
	⬜ Add "CanRun" func to Worker, checks before running
	⬜ Or better yet, check if worker has clips before running
	⬜ Cleanup old API stuff, AllBaronStages, etc.
	⬜ skypie: make [Clip] be put on the variable and generate a setter, allow callback (similar to Hook)
	⬜ Allow workers to have Clips inside method signature ************
		-- detect clip by type, split from rest of signature (can still target cogs, etc.)
	⬜ Do reactive workers (ALTERED_COG) need to be called once initially?
	⬜ Allow worker attributes to build Func instead of Action, to support:
		-- async UniTask SomeWorker(SomeCog someCog) 
	⬜ Change stage.WHEN to its own enum/something since it works differently (reactive workers, etc.)
		-- 'Removed' doesn't have access to HasAuthority, how should that work Author stage?
	⬜ Filter for inactive gameObjects or reimplement OnDisable 
	⬜ Authority: is current implementation best? (check author in workers) or switch to separate entity list?
		-- Separate list sounds tricky if we support changing authority
		
	## TODO: Feb 2021
	 ⬜ Clean up above stuff ^^^
	 ⬜ we need Async workers!  -- do we?
	 ⬜ [Added] should allow for targeting Server, Author, etc.
		[Added.Author] or [Added(Author)] etc.
	 	[Removed] would be nice too, but may not be possible
	 ⬜ Audit usage of _ (underscore) prefix for methods (seems inconsistent)
	 ⬜	 - The desire is to underscore public methods that should not be used outside of becs
	 ⬜ Fate: I wish fate AuthorityPaths were more visible in code
	 ⬜ Reactive stuff is starting to smell
	 ⬜ Add 'Remote' stage Domain? it'd be client-side, inverse of Author
	 ⬜	 ^^ NEED THIS SOON ^^
	 ⬜ Better documentation in all the places
	 ⬜ Streamline or make more clear: IsServer, !IsServer, Host crap
	 ⬜ Change it so workers only ever have 1 function (instead of multiple)
	 ⬜ Optimization: create EntityInfo that caches interest between workers & entities/cogs
	 ⬜ See if we can combine cogId/netCogId. Maybe generate IDs at buildtime?
	 ⬜	 - Currently NetCogId is set on server (SyncVar) because it is included in fate header (and it's a byte)
	 ⬜	 - Would be better if cogIds were deterministic or written at buildtime
	 ⬜ Optimization: could collapse fate header (ushort, byte, byte) -> (ushort, byte) if limit to 16cog * 16fate
	 ⬜ EntityId inconsistent type, int vs ushort
	 ⬜ Workers shouldn't ever be removed, just paused (separate the Add/Cut logic from run/don't run)
	 ⬜ React workers: would it be cleaner to have worker hold altered entityIds?
	 ⬜ For the most part, I'm okay with Workers having more state
	 ⬜ Fate server handler attribute: [HandlePropose(nameof(Agent.Nickname))]
	 ✔ Need a "server only" fate path
	 
	## TODO: March 2021
	 ⬜ Clean up above stuff ^^^
	 ✔ ## IMPORTANT: for exotic fates, I don't think we're doing the RPC serialization correctly
	 ✔	 - maybe add a Converter override: FateVar<T1> but Rpc uses T2, converter Func<T2, T1>
	 ⬜ write docs for how becs & fate interact
	 ⬜ Fate: could store _Temp() func and use lambda in declaration instead
	 ⬜ Fate: maybe get rid of _WriteData/_ReadData since we're using extension methods instead
	 ⬜ Make Barons automatically spawned (using Reflection) & support attributes
	 ⬜ Let FateVars have initial value
	 ⬜ Make React workers support more than one cog
	 ⬜ Run0 vs Do: pick one
	 
	## TODO: April 2021
	 ⬜ Clean up above stuff ^^^
	 ⬜ It sucks not being able to easily add/remove components from entities
		- Aspect & Actor system seems promising (see: AspectSet)
	 ⬜ Move to streams (see DQ) instead of Cued, especially for CuedWorkers (which need to be remade anyway)
	 ⬜ Rename CogBaseNative --> CogNative? (and others), or BaseCogNative
	 ⬜ Register events on enable/disable make me hesitant to use cog type on "dumb ref" components, e.g. ActButtonUi
	 ⬜ `if (!clip) return` boilerplate is getting annoying
		- maybe additional attribute to require clip?
		- or clips in signature of worker func will be detected & injected
		- or entire baron requires clip before initializing?
	 ⬜ Add "composite" Entity, that uses children components (instead of just self)
	 ⬜ Should clips be simplified into a blackboard pattern? like pass in clip, use clip.Get<ClipType>()
		- Or Facade pattern? https://www.notion.so/Facade-b48dc31153384c428795ad7ad97dd7eb
		- At what point do we just say: all Clips must be present in the scene barons/workers to run?
	 ⬜ It'd be nice if workers were all static functions (instead of instance)
	 ⬜ Ugh, honestly, it might be worth rewriting FateVar to use ISyncObject, similar to AspectSet
		- Less hacky (doesn't rely on tracking sync var index and SetDirtyBit)
		- How would it work with authority paths?
		- See: _NOTES_ 
	## TODO: May 2021
	 ⬜ Clean up above stuff ^^^
	 ⬜ Remove IEntity, just use Entity
	 