# TopDown Tank Battleground
![Unity 6 LTS](https://img.shields.io/badge/Unity-6000.3.26f1%20LTS-black?style=flat&logo=unity)
![Cinemachine](https://img.shields.io/badge/Cinemachine-3.1.7-blue?style=flat)
![Input System](https://img.shields.io/badge/Input%20System-1.20.1-green?style=flat)
![Platform](https://img.shields.io/badge/Platform-Windows%20Standalone%20x86__64-orange?style=flat)
**Tank Battleground** is an arena-style top-down vehicular combat game developed in Unity. Players pilot an armored tank, navigating tactical battlefields, dodging obstacles, and eliminating hostile enemy combat vehicles using projectile-based weaponry.
---
## 1. Engine & Technical Specifications
* **Target Unity Version:** **Unity 6 LTS (`6000.3.26f1`)**
* **Render Pipeline:** Built-in Render Pipeline (BiRP) with modern Unity 6 shader compilation.
* **Physics Engine:** Unity PhysX with `Time.fixedDeltaTime` integration and continuous collision detection for high-speed projectiles.
* **Package Architecture:**
  * `com.unity.cinemachine` (v3.1.7) — Modular virtual camera management, follow damping, impulse camera shake, and 3D frustum bounding volume confiners.
  * `com.unity.inputsystem` (v1.20.1) — Cross-platform action asset architecture with seamless runtime switching between Gamepad (Xbox/PlayStation) and Keyboard/Mouse.
  * `com.unity.ugui` (v2.0.0) / TextMeshPro — Dynamic UI health meters, victory/game-over progression dialogs, and debug performance profiling HUD.
---
## 2. Controls & Input Mapping
The game features dual-mode input handling with seamless hot-swapping between controllers and mouse/keyboard.
| Action | Gamepad (Xbox / PlayStation) | Keyboard & Mouse |
| :--- | :--- | :--- |
| **Drive Chassis** | **Left Stick (Y-Axis)** | **W / S** (Forward / Reverse) |
| **Turn Hull** | **Left Stick (X-Axis)** | **A / D** (Turn Left / Turn Right) |
| **Aim Turret** | **Right Stick** (360° Smooth Aim with Deadzone Retention) | **Mouse Cursor** (Raycast Aim against Ground Plane) |
| **Fire Cannon** | **Right Trigger (RT) / Right Bumper (RB)** | **Left Mouse Button (LMB)** |
| **Toggle Debug Panel** | **Select / View / Share Button** | **F3** or **Backquote (` / ~)** |
### Input Handling Details
* **Neutral Stick Retention:** When releasing the gamepad right stick, the turret maintains its last aimed direction rather than snapping back to the mouse cursor.
* **Stick Deadzone Protection:** Left-stick movement and right-stick rotation feature calibrated deadzones to eliminate stick drift.
---
## 3. Camera System (Cinemachine 3.x)
* **Camera Perspective:** Fixed 55° isometric top-down vantage point providing clear battlefield situational awareness.
* **Follow Damping:** `CinemachineFollow` with calibrated 0.5 XYZ damping creates smooth follow camera tracking without rigid snapping.
* **Camera Shake (Cinemachine Impulse):**
  * `CinemachineImpulseSource` triggers screen rumble when the player fires cannon shells.
  * Secondary impulse triggers on projectile impact and obstacle detonations.
  * `CinemachineImpulseListener` attached to the virtual camera translates 6-DOF impulse signals into camera shake.
* **Arena Confiner:** `CinemachineConfiner3D` restricts the camera frustum within a designated trigger `BoxCollider` (`BoundingVolume`), preventing views into void background space.

## 4. In-Game Debug Panel & QA Cheats
Press **`F3`**, **`` ` `` (Backquote)**, or controller **`Select / View`** at any time during gameplay to toggle the QA overlay.
### Real-Time Diagnostics
* **Live FPS:** Instantaneous framerate calculated using `Time.unscaledDeltaTime`.
* **Smoothed Average FPS:** Exponential moving average framerate.
* **Frame Time (ms):** Frame render duration in milliseconds.
* **Memory Usage (MB):** Allocated heap and native engine memory via Unity `Profiler.GetTotalAllocatedMemoryLong()`.
* **Entity Counters:** Active enemy vehicles and live projectile counts.
### QA Cheats & Shortcuts
* **God Mode (`[G]` / UI Button):** Toggles complete invincibility on the player tank.
* **Skip Level (`[L]` / UI Button):** Immediately completes the current mission and advances to the next level.
* **Game Speed:** Real-time timescale controls for **Slow Motion (0.5x `[1]`)**, **Normal (1.0x `[2]`)**, and **Fast Motion (1.5x `[3]`)**.
---
## 5. Levels & Campaign Progression
The game features an end-to-end progression loop orchestrated by [`GameManager.cs`](file:///c:/Users/moroc/Documents/Unity/test2/Unity_Dev_test/Assets/Scripts/GameManager.cs):
1. **Main Menu (`Assets/Scenes/MainMenu.unity` — Build Index 0):** start screen with a Start Game button.
2. **Training Level (`Assets/Scenes/Training.unity` — Build Index 1):** Warmup combat arena featuring static target, destructible blocks, and basic navigation.
3. **Level 1 (`Assets/Scenes/Level 1.unity` — Build Index 2):** Tactical urban maze with fortified barricades and multiple aggressive enemy tanks.
4. **Victory Flow:** Eliminating all hostile vehicles displays the Victory dialog with a **Next Level** button. Completing the final level cycles back to the Main Menu.
5. **Game Over Flow:** Health depletion triggers the Game Over dialog with an instantaneous **Retry** button.
---
## 6. Architecture & Optimization Highlights
* **Full Object Pooling:** Projectiles and explosion particle systems are managed via [`SimpleObjectPool.cs`](file:///c:/Users/moroc/Documents/Unity/test2/Unity_Dev_test/Assets/Scripts/SimpleObjectPool.cs). Both player and enemy tanks draw from pre-allocated pools, eliminating per-shot memory allocation.
* **Zero Console / Physics Spam:** Console log statements removed from physics loops, and string equality checks replaced with non-allocating `CompareTag()`.
* **Physics Precision:** All character movement and vehicle rotations use `Time.fixedDeltaTime` inside `FixedUpdate()`.
* For in-depth draw call, light baking, and physics matrix recommendations, refer to [`OPTIMIZATION_TIPS.md`](file:///c:/Users/moroc/Documents/Unity/test2/Unity_Dev_test/OPTIMIZATION_TIPS.md).