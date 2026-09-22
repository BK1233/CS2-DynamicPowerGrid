# CS2-DynamicPowerGrid

**Dynamic Power Grid Manager** is a Cities: Skylines II mod that implements automatic power grid balancing. When solar power plants (and other intermittent renewable sources) ramp up generation during the daytime, the grid manager automatically scales down dispatchable thermal and hydro stations (coal, gas, hydro, nuclear, geothermal) to match remaining net electricity demand.

---

## Key Features

- **Automatic Solar Intermittency Handling**: As solar radiation increases at dawn, solar production ramps up to fulfill city demand without fluctuations.
- **Stable Must-Run Renewable Inputs**: Solar and wind generation are treated as unmutated must-run generation inputs based on current environmental conditions (time of day / sun position), eliminating power output oscillation.
- **Hydro & Dispatchable Power Reduction**: Hydro dams, coal, gas, nuclear, and geothermal stations are uniformly throttled down to match remaining net demand once solar generation kicks in.
- **Zero Configuration Required**: Functions as an automated background grid management system without complex UI overhead.
- **Unit Tested**: Full test coverage verifying power dispatch calculations across various daytime, nighttime, and peak load scenarios.

---

## How It Works

1. **Must-Run Renewable Generation**: Solar and Wind plants output their current available generation determined by weather/time of day without being throttled.
2. **Net Demand Calculation**:
   $$\text{Net Dispatch Demand} = \max(0, \text{Total City Demand} - \text{Must-Run Generation})$$
3. **Dispatchable Throttle Ratio**:
   $$\text{Throttle Factor} = \text{Clamp}\left(\frac{\text{Net Dispatch Demand}}{\text{Total Dispatchable Available Capacity}}, 0.0, 1.0\right)$$
4. **Plant Output Adjustment**: Each dispatchable station (Hydro, Coal, Gas, Nuclear, Geothermal) sets its production output to:
   $$\text{Current Output} = \text{Available Capacity} \times \text{Throttle Factor}$$

---

## Build & Test Instructions

### Requirements
- .NET 8.0 SDK

### Building the Project
```bash
dotnet build CS2-DynamicPowerGrid.slnx
```

### Running Unit Tests
```bash
dotnet test
```

---

## Installation

1. Copy `DynamicPowerGrid.dll` from `src/CS2-DynamicPowerGrid/bin/Debug/net8.0/` to your Cities: Skylines II Mods directory (`%AppData%\LocalLow\Colossal Order\Cities Skylines II\Mods\`).
2. Launch Cities: Skylines II and enable the mod.
