# Lone's EFT DMA Radar (Nutori ver.)

![icon-static](https://github.com/user-attachments/assets/d3bc58ad-a987-4c94-bfe2-dd2236769f19)

## What is this?
- This is a fork of [Lone DMA EFT/Arena Radar](https://github.com/Lone83427/lone-eft-dma-radar) with my personal features and changes.

## Current Changes

### UI
   - Unlocked `Fuser ESP` settings while ESP is running
   - Reduced the size of the Loader UI and changed its background color to black.
   - Removed parentheses around the held weapon ESP.
   - Removed the auto-start functionality from `Auto Fullscreen`.
   - Added `Auto Start` as a standalone option in `Fuser ESP` settings.
   - Added `BTR Render Mode` to the settings.
   - Changed how the distance and height are rendered on the radar. (`D: <distance> H: <height>` -> `<distance>M (<height>)`)
   - Added a `SaveFileDialog` to the backup config handler.
   - Made `Player/AI Render Mode` options mutually **inclusive** instead of exclusive.
   - Removed FPS display from the radar window title.
   - Added player prestige level to the player info widget and radar.
   - Added loot info widget (credit: [Mambo](<https://github.com/Mambo-Noob/eft-dma-radar>)).

### Functionality
   - Split `AdvancedMemWrites` into:
      - `MonoPatches` Controls patching of mono-compiled (game) functions.
      - `NativePatches` Controls patching of native-compiled functions (`NativeHook`, `AntiPage`, `Advanced Chams`, etc.).
   - Removed mono function patching from the silent aimbot (may reintroduce later).
      - Now uses a silent aim method that modifies memory values instead.
   - Added `ToggleWeaponCollisions` (**Risky**, Visible client-side only).
      - Marked as risky due to potential for wall-shooting, which could be checked server-side.
   - Added `UnclampFreeLook` (Visible client-side only).
   - Added `Long Jump` (**Risky**).
      - Marked as risky due to potential server-side checks.
   - Added `Instant Pose Change` (**Visible client-side only**).
   - Added `Instant Plant` (**Risky**).
   - Reworked `PrecisionTimer` for a more accurate ESP FPS lock.

## Planned Changes
   - Make all mono patch features toggleable.
   - Add GUI to show current in-game counters (kills, etc.).
   - Split the `Max Distance` slider to separate sliders for everything under this max distance slider.
   - Add player loot to ESP when hovered.
   - Separate `Always Day/Sunny` into `Always Clear Weather` and `Force Time` with a slider for time.
   - Separate `Fast Weapon Ops` into `Instant ADS` and `Fast Reload`.
   - Add extract switches to extract ESP (D-2 power, Bunker Hermetic power, Parking Gate Power, etc.).
   - Implement 3D look direction.
   - Show limb health on top of the player status.
   - Add `Show Non-FIR` to the `Loot` widget on the radar.
   - Add `Show Roubles` to the `Loot` widget on the radar.
   - Add Aimbot FOV scaling according to the in-game FOV.
   - Add Explosive Gas Tank ESP.
   - Add option to disable the inventory blur.
   - Add throw strength feature.
   - Add option to enable `High Alert` for teammates on radar.
   - Add an advanced quest helper (Maybe save for UI Rework).
   - Add raid deploy countdown on the ESP/Radar.
   - Add option to draw the extracts OBB on ESP/Radar.
   - Add option to show in-game time on ESP/Radar.

## Bugs
   - I do not believe there are any issues with my modifications, but I have not extensively tested the features.
