#! /bin/bash

mkdir -p ../Dependencies
cd ../Dependencies
rm -rf master.tar.gz
rm -rf comex-project
wget -O master.tar.gz https://github.com/armando-basile/comex-project/archive/master.tar.gz 
tar xf master.tar.gz
rm -rf master.tar.gz
mv comex-project-master comex-project
cd ../build-linux
