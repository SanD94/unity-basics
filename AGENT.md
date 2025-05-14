# Unity Project Agent Guidelines

## Build/Run Commands
- No traditional test commands; testing done via Play mode testing

## Code Style Guidelines

### Imports
- Use specific imports with full namespaces (e.g., `using UnityEngine;`)
- Group Unity imports first, then system libraries
- Use static imports when appropriate (e.g., `using static Unity.Mathematics.math;`)

### Naming Conventions
- PascalCase for class names, public methods, and properties
- camelCase for local variables and parameters
- Use descriptive names that reflect purpose

### Formatting
- Use 4-space indentation
- One statement per line
- Group related code blocks with blank lines

### Unity-Specific Patterns
- Use [SerializeField] for editor-exposed private fields
- Implement OnEnable/OnDisable pairs for resource management
- Use Job System with appropriate attributes (e.g., [BurstCompile], [ReadOnly])
- Clean up native collections and compute buffers in OnDisable