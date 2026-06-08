# Nitrogen

Desktop application for industrial process monitoring and control.

Built with Avalonia and .NET.

## Technology Stack

* C#
* .NET 10
* Avalonia 12
* ReactiveUI
* NModbus
* SQLite (planned)

Supported platforms:

* Windows
* Linux

---

# Project Structure

```text
Nitrogen
├─ Configuration
├─ Controls
├─ Resources
├─ Services
├─ Views
├─ App.axaml
└─ Program.cs
```

---

# Views

Each screen is stored in its own folder.

Example:

```text
Views
└─ Pressure
   ├─ PressureView.axaml
   ├─ PressureView.axaml.cs
   └─ PressureViewModel.cs
```

ViewModel is always located next to its View.

No separate `ViewModels` folder is used.

---

# Configuration

Application configuration files.

```text
Configuration
└─ Modbus
   ├─ modbus_connection.json
   ├─ input_registers.json
   └─ holding_registers.json
```

---

# Services

Application logic and infrastructure services.

```text
Services
├─ Localization
├─ Modbus
├─ SystemTime
└─ Units
```

---

# Modbus

```text
Services
└─ Modbus
   ├─ Configuration
   ├─ Connection
   ├─ Polling
   ├─ Rx
   └─ Writing
```

## Configuration

Responsible for loading and validating Modbus configuration files.

```text
Services
└─ Modbus
   └─ Configuration
      ├─ Loading
      ├─ Models
      │  ├─ Connection
      │  └─ Registers
      └─ Validation
```

### Models

Connection models:

```text
ModbusConnectionConfig
```

Register models:

```text
ModbusRegisterConfig
ModbusRegisterValueType
FloatByteOrder
```

### Loading

Responsible for loading JSON configuration files.

```text
ModbusConfigLoader
```

### Validation

Reserved for configuration validation logic.

---

## Connection

Responsible for:

* TCP connection
* Automatic reconnect
* Request synchronization
* Request queue
* NModbus communication

A single connection is used for the entire application.

---

## Polling

Responsible only for cyclic reading of Input Registers.

Polling does **not**:

* Manage TCP connections
* Perform reconnect logic
* Communicate directly with NModbus
* Manage write operations
* Maintain request queues

---

## Writing

Responsible only for writing Holding Registers.

---

## Rx

Reactive data distribution layer.

Current pipeline:

```text
ModbusPoller
    ↓
ModbusRxService
    ↓
ViewModel
```

---

# Current Status

Implemented:

* Modbus configuration models
* JSON configuration files
* JSON loading via System.Text.Json
* Enum-based configuration values
* Reactive polling pipeline
* Input/Holding register configuration

Configuration loading successfully verified.

Current Poller uses test data generation.

---

# Planned Work

Next milestone:

```text
ModbusConnectionManager
    ↓
ModbusPoller
    ↓
ModbusRxService
    ↓
ViewModel
```

Planned tasks:

* Implement ModbusConnectionManager
* Integrate NModbus
* Connect Poller to real Modbus communication
* Add register writing support
* Add SQLite logging

---

# Design Goals

* Simplicity
* Readability
* Predictable structure
* Long-term maintainability
* Minimal dependencies
* Cross-platform support

