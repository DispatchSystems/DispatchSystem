# How to build solution

Building the solution is actually very easy. Everything to build is included with the mod. The basics of it is just to open the `src` folder from the clone, open the project file, and build it.

## Common errors

These errors have not occured with anyone, but I just want to make sure that they are out of the way

* Both project file say that there are missing dependencies/references
  1. Just add both of the references back to the right project, the references can be found in the `ref` folder in the clone
  2. Sometimes the NuGet packages are weird and don't follow the clone, so just download the package CloNET & MaterialSkin again.
> TODO: More to add once more errors popup

## Build location

The build location of the files will be in `src/bin/Debug/*` or if built in Release then will be in `src/bin/Release/*`
The directory of the build is changed so that both of the build files will end up in the same place, just easier for workflow.
