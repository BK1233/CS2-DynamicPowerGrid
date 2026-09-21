# CS2-DynamicPowerGrid

**Dynamic Power Grid Manager** is a Cities: Skylines II mod that implements automatic power grid balancing. When solar power plants (and other intermittent renewable sources) ramp up generation during the daytime, the grid manager automatically scales down dispatchable thermal and hydro stations (coal, gas, hydro, nuclear, geothermal) to match remaining net electricity demand.

---

## Key Features

- **Automatic Solar Intermittency Handling**: As solar radiation increases at dawn, solar production ramps up to fulfill city demand.
- **Uniform Dispatch Throttling**: The grid manager calculates the remaining net power demand and uniformly throttles dispatchable power plants (Coal, Gas, Hydro, Nuclear, Geothermal) down to the exact ratio required to satisfy demand without over-generating electricity.
- **Zero Configuration Required**: Functions as an automated background grid management system without complex UI overhead.
- **Unit Tested**: Full test coverage verifying power dispatch calculations across various daytime, nighttime, and peak load scenarios.

---

## How It Works

1. **Must-Run Generation**: Solar and Wind plants run at available weather capacity.
2. **Net Demand Calculation**:
   $$\text{Net Dispatch Demand} = \max(0, \text{Total City Demand} - \text{Must-Run Generation})$$
3. **Dispatchable Throttle Ratio**:
   $$\text{Throttle Factor} = \text{Clamp}\left(\frac{\text{Net Dispatch Demand}}{\text{Total Dispatchable Capacity}}, 0.0, 1.0\right)$$
4. **Plant Output Adjustment**: Each dispatchable station (Coal, Gas, Hydro, Nuclear, Geothermal) sets its production output to:
   $$\text{Output} = \text{Max Capacity} \times \text{Throttle Factor}$$

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
