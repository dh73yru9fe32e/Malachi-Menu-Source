# Malachis_Menu_Reborn.dll — deobfuscation notes

Source DLL: `C:\Users\kenzo\Downloads\Malachis_Menu_Reborn (5).dll`
- 572,416 bytes, MZ/.NET Framework 4.8, assembly `Malachis_Menu_Reborn` v1.0.0.0
- SHA256: D829620E1FCAE80E5ED5B416093A34C6F962F937FFFDDE98B1110F75E2345916
- BepInEx plugin: GUID `malachisreborn`, name `malachis.reborn` (Plugin.cs), plus a second BepInEx plugin `malachis.reborn.GUI` (GuiPlugin.cs)

## Obfuscator identification
- **Renaming obfuscation only.** No string encryption markers, no crypto/AES/Base64
  decryption routines, no embedded resources, empty `<Module>` (no injected
  bootstrap). All UI strings (mod names, descriptions) are plaintext in the
  #US heap and decompile cleanly.
- Tell-tale renamed identifiers: single letters (`a`, `b`, `c`, `s`), short
  codes (`QD`, `CR`, `AB`, `UT`, `UAJ`…), junk names (`Bullshit`, `Bullshit2/3/4`,
  `RigShit`), and a junk namespace (`dfgsfe`). Consistent with ConfuserEx-style
  renaming. One Harmony attribute (`QD`, now `EmptyHarmonyPatch`) has no
  decodable target — it is a dead patch whose `Prefix()` returns `false`.
- Decompiled with ILSpy (`ilspycmd`), then renamed systematically (see below).

## Rename map (top level)
| Obfuscated | Deobfuscated | Purpose |
|---|---|---|
| `a` | `ReportDispatchPatch` | Prefix on `MonkeAgent.DispatchReport` → `false` |
| `b` | `RpcCallTrackerPatch` | Prefix on `MonkeAgent.GetRPCCallTracker` → `false` |
| `c` | `CheckReportsPatch` | Prefix on `MonkeAgent.CheckReports` → `false` |
| `QD` | `EmptyHarmonyPatch` | `[HarmonyPatch]` with no target, Prefix → `false` (dead) |
| `Bullshit` | `RigDeregisterPatch` | Prefix on `VRRigJobManager.DeregisterVRRig` (own rig guard) |
| `Bullshit2` | `RigPostTickPatch` | Prefix on `VRRig.PostTick` (local-rig guard) |
| `Bullshit3` | `RigAwakePatch` | Prefix on `VRRig.Awake` (skip "Local Gorilla Player(Clone)") |
| `Bullshit4` | `RigPosePatch` | Postfix on `VRRig.PostTick` — upside-down/backwards/forward/crawl/spaz/spin/flip own-rig posing + Grab Rig |
| `CR` | `CoroutineRunner` | Singleton `MonoBehaviour` coroutine host |
| `RigShit` | `RigUtils` | Rig/player lookup helpers (AllRigs, RigFromPlayer, Map, …) |
| `collider` | `ReflectionEnums` | `RPC` + `Fields` enums used for reflection field indices in `Mods` |
| `dfgsfe.Gui` | `MalachiTemp.Gui.GuiPlugin` | Second BepInEx plugin (PC GUI / controller-input faker `EmPa`) |

## WristMenu nested patch classes (renamed by patch target)
Each was a Harmony patch nested inside `WristMenu`, renamed to
`<TargetType><TargetMethod>Patch`, e.g.:
- `AB` → `GameEntityManagerCreateItemRPCPatch` (blocks foreign item-create RPCs)
- `Pick` → `VoxelPickaxeUpdateInteractionPointPatch` (mining radius/cooldown)
- `AD` → `LoadBalancingTransportonEventActionVoiceClientPatch` (voice event block)
- `AGF` → `VRRigGrabbedByPlayerPatch` (anti-grab by non-GUARDIAN)
- `APQ`/`APQ2` → `SecondLookSkeletonFloatPlayerPatch` / `…ChuckPlayerPatch`
- `s` → `PhotonNetworkRunViewUpdatePatch` (event hook)
- `Knockback`/`BKnockback` → `GTPlayerApplyKnockbackPatch` / `SIPlayerPlayerKnockbackPatch`
- `abc` → `BuilderTableNetworkingPieceCreatedByShelfRPCPatch`
- `UpdateColor` → `GorillaScoreBoardRedrawPlayerLinesPatch`
- `UT/UTA/UY/UP/US/UBB/UBBB/UA/UAG/UAJ` → matching `SIGadget*` patches
- `AL/ALF` → `LurkerGhostSeekPlayerPatch` / `HalloweenGhostChaserRiseGrabbedLocalPlayerPatch`
- `ACK` → `PhotonNetworkControllerAttemptToFollowIntoPubPatch`
- `VIM` → `SubscriptionManagerIsLocalSubscribedPatch`

Inside `WristMenu`, other nested classes each have their own `public static float/dictionary s`
field (a changed value cache) — those `s` fields belong to their enclosing patch class and
were left as-is except where the class `s` rename collided with named arguments (`s:`),
which were restored.

## Structure of the deobfuscated project
- `Malachis_Temp/Plugin.cs` — BepInEx entry, `Harmony.PatchAll()`
- `Malachis_Temp/UpdatePatch.cs` — `GTPlayer.FixedUpdate` patch that spawns the runtime
  GameObject (WristMenu, CoroutineRunner, RigUtils, Mods, GhostPatch, NotifiLib) once
- `MalachiTemp.UI/WristMenu.cs` (429 KB) — wrist menu UI + all button/mod definitions
- `MalachiTemp.Backend/Mods.cs` (463 KB) — the mod implementations
- `MalachiTemp.Gui/GuiPlugin.cs` (77 KB) — PC IMGUI menu + input faker
- plus small patches/helpers listed above

## Build status
Decompilation is complete and readable, but a full recompile was **not** verified:
- The `.csproj` (net48) references game assemblies (`Assembly-CSharp`, Unity, Photon,
  BepInEx, SteamVR, Modio, …) that must come from a Gorilla Tag install's
  `Gorilla Tag_Data\Managed` — same setup as the Sonder project.
- ILSpy output can contain artifacts (e.g. `(Object)(object)` casts, `//IL_` comments
  were stripped) that may need small fixes to compile against current Unity/Photon versions.

Original DLL is untouched in Downloads; this folder is a separate deobfuscated copy.
