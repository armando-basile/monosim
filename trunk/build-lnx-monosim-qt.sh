#! /bin/bash

TARGET="Debug"

# Clean and Build solution
mdtool build -t:Clean -c:$TARGET monosim-qt.sln
mdtool build -t:Build -c:$TARGET monosim-qt.sln
