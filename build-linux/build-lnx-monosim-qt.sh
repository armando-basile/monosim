#! /bin/bash

TARGET="Debug"
DIRNAME="$(dirname "$(readlink -f "$0")")"

# detect if there is a target specified
if [ $# -gt 0 ] ; then
  TARGET="$1"
fi

# Clean and Build
xbuild /t:Rebuild /p:Configuration=$TARGET  $DIRNAME/../solutions/monosim-qt.sln

