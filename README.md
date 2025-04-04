# Algorithm Visualizer

A WPF application that visualizes sorting algorithms in real-time. The project demonstrates how different sorting algorithms work by showing step-by-step animations of the sorting process.

## Features

- Real-time visualization of sorting algorithms
- Support for multiple sorting algorithms:
  - Bubble Sort
  - Selection Sort
  - Insertion Sort
  - Merge Sort
  - Quick Sort
- Step-by-step animation with detailed state information
- Adjustable animation speed
- Visual indicators for active and swapped elements
- Customizable array size

## Requirements

- Windows 10/11
- .NET 8.0 Runtime or later

## Installation

### Option 1: Using the executable
1. Download the latest release from the Releases page
2. Extract the ZIP file
3. Run `AlgorithmVisualizer.WPF.exe`

### Option 2: Building from source
1. Clone the repository:
```bash
git clone https://github.com/yourusername/Algorithm-Visualizer.git
```

2. Navigate to the project directory:
```bash
cd Algorithm-Visualizer
```

3. Build the solution:
```bash
dotnet build
```

4. Run the application:
```bash
dotnet run --project AlgorithmVisualizer.WPF
```

## Creating a Release

To create a self-contained executable:

```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

The executable will be created in:
`AlgorithmVisualizer.WPF/bin/Release/net8.0-windows/win-x64/publish/AlgorithmVisualizer.WPF.exe`

## Usage

1. Select a sorting algorithm from the dropdown menu
2. Enter the desired array size (1-100)
3. Click "Generate Array" to create a new random array
4. Use the controls to:
   - Play/Pause the animation
   - Step through the algorithm
   - Adjust animation speed
5. Watch the visualization and read the algorithm description
