# Living Statues Documenation

## Description

This repo contains the source project and sample builds for the living statues interactive installations created for the [Public Arts Futures Lab AiR program](https://publicartfutureslab.com/meet-the-2024-air) by [Elijah Zulu](https://www.elijahzulu.com). The repo also contains additional tools/scripts/design files created as residency.

## Tools Used
List of programs used:
  - Unity 3D (assembly)
  - Blender 3D (modelling/texturing/animation/rendering)
  - Make Human (modelling)
  - Azure Kinect viewer (debugging)
  - Jupyter Notebook
  - Adobe Photoshop/Premiere/AE

## Folder structure

The repo is organized as a standard Unity project with an extra folder non-unity related files. Builds are also in this extra folder. The extra folder is called `Non-Unity`. Repo uses git lfs


## The Statues

A build of the living statues executable can found in the `Non-Unity` folder. To turn on and off specific features (interactivity, switching models, saving photos, saving recorded animation etc) you will have to edit the managers in the Unity project and build a new executable. 

### Requirements
To run the live interactive versions of the statues, you will need an [Azure Kinect Sensor](https://learn.microsoft.com/en-us/previous-versions/azure/kinect-dk/about-sensor-sdk) with the necessary body tracking SDK installed.

### Building 
Build the project as you would a standard Unity project targeted for windows. Double check the build settings to make sure you only have one scene in the `Scenes In Build` section and that this scene is the part of the project you want to build i.e photobooth scene for photobooth. 

### Usage
`Esc` closes the project

`Space` forces a change of state

`R` restart

### Misc
Option to save motion data is turned off by default but can be activated in the unity project an included into a build. This does slow down the project a bit depending on your machine --additional optmization needed. 

## Human Segmentation
Segmentation was an important part of processing image data collected during the lifespan of an installation. The segmentation code feeds a batch of images into machine learning model and a spits out a transparent png of humans detected in the images.

Segmentation script can be found in the `Non-Unity` folder. Simply change the input and output directories and run the script. 

## Photobooth Setup

A build of the photobooth can be found in the `Non-Unity` folder

### Requirements
[Azure Kinect Sensor](https://learn.microsoft.com/en-us/previous-versions/azure/kinect-dk/about-sensor-sdk) needed to run the photobooth experience. Technically, the executable does work with the sensor and you will be able to take photos but without any of the interactive elements/camera feeds.  

### Printing
For printing, 
Make sure the `PhotoBoothPrinter.py` python script is running and that the printer you want to use is setup as your default printer in your operating system settings. Change the photo directory in the python script. The script is constantly listening for the addition of new images in that directory. 

### Building 
Same build instructions as above --just make sure the photobooth scene is the only scene in the build settings.  

### Usage
`Esc` closes the project

`Space` starts the countown photo sequence

`R` restart