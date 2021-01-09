# About

Chat Example Mobile App

This is an ionic project for Chat application. You need to have Cordova and Ionic 4.0.0 installed on the
system to run it successfully

## Using this project

You must have cordova installed prior to this.

```
    $ npm install -g cordova
```

```
    $ npm install -g ionic
```

NOTE: This app is built and tested on 4.0.0.

## Installation of this project

- Install npm dependecies

```
    $ npm install
```

- Install Resources

```
    $ ionic cordova resources
```

- Add Platform (whichever required)

```
    $ ionic cordova platform add android
    $ ionic cordova platform add ios
```

in few cases, you might need to install the latest platform

```
    $ ionic cordova platform add android@latest
    $ ionic cordova platform add ios@latest
```

- Install Plugins (whichever required)

```
    $ ionic cordova plugin add YOUR_PLUGIN_NAME
```

- Initialize the new git
  `git init`

- Setup the new git remotes accordingly
  `git remote add origin new remote`
- For iOS Build

```
   https://ionicframework.com/docs/building/ios

```

- For Android Build

```
   https://ionicframework.com/docs/building/android

```

- Run app on device

```
    $ ionic cordova run android
    $ ionic cordova run ios --device
```
