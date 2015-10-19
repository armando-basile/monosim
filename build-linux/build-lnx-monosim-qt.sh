#! /bin/bash

TARGET="Debug"

# detect if there is a target specified
if [ $# -gt 0 ] ; then
  TARGET="$1"
fi

# Clean and Build
xbuild /t:Rebuild /p:Configuration=$TARGET  ../monosim-qt.sln

