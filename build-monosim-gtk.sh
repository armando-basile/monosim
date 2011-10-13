#! /bin/bash

TARGET="Debug"

# Clean and Build solution
mdtool build -t:Clean -c:$TARGET monosim-gtk.sln
mdtool build -t:Build -c:$TARGET monosim-gtk.sln
