<div align="center">

<img src="Docs/Tijdtuin%20Intro.png" alt="Tijd Tuin — Interactive Garden Experience" width="700">

# Tijd Tuin

**An interactive garden experience where time and nature intertwine, created with Unity.**

Explore a world where your actions influence the flow of time and the growth of the environment. Tijd Tuin was chosen by children as the best experience at Cinekid, where we received the Cinekid Award for Excellence.

</div>

---

## Overview

Tijd Tuin is an interactive experience about the relationship between time, nature, and the player's actions. Players explore a garden environment where interactions influence how the world develops and changes.

The project combines an interactive environment with reusable Unity systems for timed events, trigger-based interactions, and map resets. These systems help coordinate gameplay behavior and support a consistent experience.

### Key Features

- **Interactive Garden** — explore an environment shaped by time and nature.
- **Time-Based Gameplay** — coordinate events and delays with asynchronous timers.
- **Trigger-Based Events** — invoke actions when objects enter or exit trigger areas.
- **Map Reset System** — restore the environment after an experience ends.
- **Reusable Gameplay Components** — keep timing, triggers, and map management in separate systems.
- **Cinekid Recognition** — selected by children as the best experience at Cinekid and awarded the Cinekid Award for Excellence.

### Technologies

`Unity` · `C#` · `UniTask` · `UnityEvents` · `Unity Physics` · `Prefab Management`

## Code & System Architecture

The project uses focused Unity components for asynchronous timers, trigger-based event handling, and map lifecycle management. Each system can be configured independently and connected to gameplay events through the Unity Inspector.

### 1. Async Timer Manager

A lightweight asynchronous timer system that allows multiple timers to run in parallel without coroutines. It supports start and end callbacks, cancellation, and global stop control.

The system is useful for sequencing events, adding delays, and managing respawn timers without creating separate coroutine routines.

<p align="center">
  <img src="Docs/Timers%20Manager.png" alt="Async Timer Manager code" width="600">
</p>

### 2. Trigger Event Handler

A flexible trigger-based event system that invokes UnityEvents when an object with a specific tag enters or exits a collider.

This makes it possible to configure interactions for cameras, zones, objects, and cutscenes without writing custom trigger logic for every scenario.

<p align="center">
  <img src="Docs/Trigger%20Event%20System%20.png" alt="Trigger Event Handler code" width="600">
</p>

### 3. Map Reset System

A map-management system that resets or reloads the current map after an experience ends. It stores the current map's transform, destroys the active map instance, waits using the asynchronous timer system, and spawns a fresh map prefab at the saved position and rotation.

<p align="center">
  <img src="Docs/Reset%20System%20.png" alt="Map Reset System code" width="600">
</p>

### Cinekid Project Showcase

<p align="center">
  <img src="Docs/Chosen%20Cinekid.jpeg" alt="Chosen by Cinekid" width="420">
  <img src="Docs/Cinekid%20Set.jpeg" alt="Cinekid installation" width="420">
</p>

<p align="center">
  <img src="Docs/Cinekid%20Win.jpeg" alt="Cinekid award" width="420">
  <img src="Docs/XR%20Lab%20Team.jpeg" alt="XR Lab Team" width="420">
</p>

## Extra Media

Additional images and videos from the project, including the experience showcase, environment renders, and development documentation.

- [Tijd Tuin Showcase Video](Docs/Tijdtuin_ShowcaseVideo.mp4)
- [Tijd Tuin Experience Video](Docs/tijtdtuin.mp4)
- [Boarding School Render](Docs/RenderVideoBoardingVIDSchuur.mp4)
- [Garden Render](Docs/RenderVideoBoardingVIDTuin.mp4)
- [Chosen by Cinekid](Docs/Chosen%20Cinekid.jpeg)
- [Cinekid Set](Docs/Cinekid%20Set.jpeg)
- [Cinekid Win](Docs/Cinekid%20Win.jpeg)
- [IRL Set](Docs/Irl%20Set.jpg)
- [Seed Respawn](Docs/Seed%20Respawn.gif)
- [Time Set](Docs/Time%20Set.jpg)
- [XR Lab Team](Docs/XR%20Lab%20Team.jpeg)

---

<div align="center">

**Tijd Tuin**

Unity · C# · Interactive Experience · Cinekid Award for Excellence

</div>
