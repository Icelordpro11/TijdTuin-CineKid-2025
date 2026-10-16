 <div align="center">

<img src="Docs/Tijdtuintuin-Intro.png" alt="Tijd Tuin — Interactive Garden Experience" width="700">

# Tijd Tuin

**An interactive garden experience where time and nature intertwine, created with Unity.**

Explore a world where your actions influence the flow of time and the growth of the environment. Tijd Tuin was chosen by children as the best experience at Cinekid, where the project received the Cinekid Award for Excellence.

</div>

---

## Overview

Tijd Tuin is an interactive experience about the relationship between time, nature, and the player's actions. Players explore a garden environment where interactions influence how the world develops and changes.

The project combines interactive environments with reusable Unity systems for timed events, trigger-based interactions, and map resets. These systems help coordinate gameplay behavior and support a consistent experience.

### Key Features

- **Interactive Garden** — explore an environment shaped by time and nature.
- **Time-Based Gameplay** — coordinate events and delays with asynchronous timers.
- **Trigger-Based Events** — invoke actions when objects enter or exit trigger areas.
- **Map Reset System** — restore the environment to a saved state after an experience ends.
- **Reusable Gameplay Components** — keep timing, triggers, and map management in separate systems.
- **Award Recognition** — selected by children as the best experience at Cinekid and awarded the Cinekid Award for Excellence.

### Technologies

`Unity` · `C#` · `UniTask` · `UnityEvents` · `Unity Physics` · `Prefab Management`

## Code & System Architecture

The project uses focused Unity components for asynchronous timers, trigger-based event handling, and map lifecycle management. Each system can be configured independently and connected to gameplay events through the Unity Inspector.

### 1. Async Timer Manager

A lightweight asynchronous timer system that allows multiple timers to run in parallel without coroutines. It supports start and end callbacks, cancellation, and global stop control.

The system is useful for sequencing events, adding delays, and managing respawn timers without creating separate coroutine routines.

**Code concepts:** asynchronous delays, parallel timers, start/end callbacks, cancellation, and global timer control.

<p align="center">
  <img src="Docs/Timers%20Manager.png" alt="Async Timer Manager code" width="600">
</p>

### 2. Trigger Event Handler

A flexible trigger-based event system that invokes UnityEvents when an object with a specific tag enters or exits a collider.

This makes it possible to configure interactions for cameras, zones, objects, and cutscenes without writing custom trigger logic for every scenario.

**Code concepts:** `OnTriggerEnter`, `OnTriggerExit`, tag filtering, collider interactions, and Inspector-configured UnityEvents.

<p align="center">
  <img src="Docs/Trigger%20Event%20System.png" alt="Trigger Event Handler code" width="600">
</p>

### 3. Map Reset System

A map-management system that resets or reloads the current map after an experience ends. It stores the current map's transform, destroys the active map instance, waits using the asynchronous timer system, and spawns a fresh map prefab at the saved position and rotation.

**Code concepts:** transform snapshots, prefab instantiation, map cleanup, delayed respawning, and saved position and rotation.

<p align="center">
  <img src="Docs/Reset%20System.png" alt="Map Reset System code" width="600">
</p>

## Extra Media

Additional images and videos from the project, including the experience showcase, environment renders, and development documentation.

### Project Showcase

<p align="center">
  <img src="Docs/Tijdtuintuin-Intro.png" alt="Tijd Tuin project introduction" width="700">
</p>

- [Tijd Tuin Showcase Video](Docs/Tijdtuintuin_ShowcaseVideo.mp4)
- [Tijd Tuin Experience Video](Docs/tijtdtuin.mp4)

### Environment Renders

- [Boarding School Render](Docs/RenderVideoBoardingVIDSchuur.mp4)
- [Garden Render](Docs/RenderVideoBoardingVIDTuin.mp4)

### Project Images

- [Chosen by Cinekid](Docs/Chosen%20Cinekid.jpg)
- [Cinekid Installation](Docs/Cinekid%20Set.jpg)
- [Cinekid Award](Docs/Cinekid%20Win.jpg)
- [IRL Set](Docs/Irl%20Set.jpg)
- [Seed Respawn](Docs/Seed%20Respawn.gif)
- [Time Set](Docs/Time%20Set.jpg)
- [XR Lab Team](Docs/XR%20Lab%20Team.jpg)

---

<div align="center">

**Tijd Tuin**

Unity · C# · Interactive Experience · Cinekid Award for Excellence

</div>
