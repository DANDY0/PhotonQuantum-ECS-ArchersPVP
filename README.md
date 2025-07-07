This is a PvP archery prototype built with Unity, Photon Quantum, and a custom ECS architecture.  
⬇️ The preview below demonstrates a real-time **duel between archers** ⬇️

![Preview](Preview.gif)

## 🎯 Features

- 🔁 **Deterministic multiplayer simulation** via Photon Quantum
- 🧠 **Custom matchmaking system** with configurable room parameters
- 🤖 **Bot opponent mode** to test mechanics without another player
- 🏹 **Archero-like controls** — auto-attack while moving/stopping
- 🧪 **Ability deck selection & combination** in the menu before the match
- 🔥 **Stackable ability effects** — e.g., fire + poison arrows, orbiting weapons
- 🌿 **Bush stealth system** inspired by Brawl Stars — attacking reveals your position
- 🕹 **Game state flow**: match intro → countdown → gameplay → round end → reset → game over
- ♻️ **Rounds system** supporting Bo1, Bo3, Bo5 formats
- ⚙️ **Hybrid architecture**: ECS for gameplay logic, OOP for UI/menu systems
