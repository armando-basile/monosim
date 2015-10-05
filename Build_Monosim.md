# Introduction #

This page lists methods to build Monosim on Linux and Windows os.

# Build on Linux #

## Build and install from tarballs ##

Can find tarballs in [Downloads area](http://code.google.com/p/monosim/downloads/list).

### For monosim-gtk ###

Build Dependencies:
  * mono devel package
  * log4net devel package
  * comex-base devel (see [comex-base install](http://code.google.com/p/comex-project/Wiki/Install_Comex) or [comex-base build](http://code.google.com/p/comex-project/Wiki/Build_Comex))
  * gtk-sharp2 devel package
  * glade-sharp2

To build and install can use:
```
 $ ./configure --prefix=/usr
 $ make
 # make install (as root)
```

---


### For monosim-qt ###

Build Dependencies:
  * mono devel package
  * log4net devel package
  * comex-base devel (see [comex-base install](http://code.google.com/p/comex-project/Wiki/Install_Comex) or [comex-base build](http://code.google.com/p/comex-project/Wiki/Build_Comex))
  * qyoto devel package
  * qyoto

To build and install can use:
```
 $ ./configure --prefix=/usr
 $ make
 # make install (as root)
```

<br />


## Build from sources ##
Need to obtain sources using checkout instructions contained in [Source Tabs](http://code.google.com/p/monosim/source/checkout).

### For monosim-gtk ###

Build Dependencies:
  * mono devel package
  * log4net devel package
  * comex-base devel (see [comex-base install](http://code.google.com/p/comex-project/Wiki/Install_Comex) or [comex-base build](http://code.google.com/p/comex-project/Wiki/Build_Comex))
  * gtk-sharp2 devel package
  * glade-sharp2

To build can use build script:
```
 $ ./build-lnx-monosim-gtk.sh
```
build output will be generated in monosim-gtk/bin/Debug folder. Can run using
```
 $ mono --debug monosim-gtk.exe --log-console
```

---


### For monosim-qt ###

Build Dependencies:
  * mono devel package
  * log4net devel package
  * comex-base devel (see [comex-base install](http://code.google.com/p/comex-project/Wiki/Install_Comex) or [comex-base build](http://code.google.com/p/comex-project/Wiki/Build_Comex))
  * qyoto devel devel package
  * qyoto

To build can use build script:
```
 $ ./build-lnx-monosim-qt.sh
```
build output will be generated in monosim-qt/bin/Debug folder. Can run using
```
 $ mono --debug monosim-qt.exe --log-console
```

<br />


# Build on Windows #

## Build from sources ##
Need to obtain sources using checkout instructions contained in [Source Tabs](http://code.google.com/p/monosim/source/checkout).


After can use [SharpDevelop IDE](http://sharpdevelop.net/opensource/sd/) to build all solutions. Note that you need to change reference to comex-base and log4net because default reference is in GAC.