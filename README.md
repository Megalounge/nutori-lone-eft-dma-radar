# Lone's EFT DMA Radar (Nutori ver.)

![icon-static](https://github.com/user-attachments/assets/d3bc58ad-a987-4c94-bfe2-dd2236769f19)

## What is this?
- This is a fork of [Lone DMA EFT/Arena Radar](https://github.com/Lone83427/lone-eft-dma-radar) with my personal features and changes.

## Current Changes
- Split `AdvancedMemWrites` into `MonoPatches` and `NativePatches`
   - `MonoPatches` controls the patching of mono compiled functions (game functions)
   - `NativePatches` controls the patching of native compiled functions (NativeHook, AntiPage, Advanced Chams, etc..)

- Disabled the locking of the Fuser ESP settings while the ESP is running

## UI
   - Made the Loader UI smaller and changed the background color to black
   - Removed the parentheses around the held weapon esp
   - Changed `Grp` to `Acct Type`
   - Removed the auto start functionality of `Auto Fullscreen`
   - Added `Auto Start` as a stand-alone option to the `Fuser ESP` settings

## Functionality
   - Removed mono function patching from the silent aimbot entirely (Might add back later). I've replaced it with a silent aim method that only writes memory values.
   - Added `ToggleWeaponCollisions` (Risky, Only visible client-side)
      - Marked as risky because of the potential to shoot through walls with it, which could be checked server-side.
   - Added `UnclampFreeLook` (Only visible client-side)
   - Added `Long Jump` (Risky)
      - Marked as risky because of potential server-side checks.
   - Added `Instant Pose Change` (Only visible client-side)
   - Reworked how `PrecisionTimer` works for a more accurate ESP fps lock

## Planned Changes
   - Seperate `Always Day/Sunny` into `Always Clear Weather` and `Force Time` with a slider for time
   - Seperate `Fast Weapon Ops` into `Instant ADS` and `Fast Reload`
   - Add loot list to radar/esp
   - Add player loot to esp when hovered
   - Make all Mono Patch features toggleable

## Bugs
   - I do not believe there are any issues with my modifications, but I have not extensively tested the features.
