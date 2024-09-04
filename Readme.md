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

The repo is organized as a standard Unity project with an extra folder non-unity related files. Repo uses git lfs

```
my-document/     # Root directory.
|- build/        # Folder used to store builded (output) files.
|- src/          # Markdowns files; one for each chapter.
|- images/       # Images folder.
|- metadata.yml  # Metadata content (title, author...).
|- Makefile      # Makefile used for building our documents.
```


## The Statues

The interactive living statues are created in Unity3D. 

### Requirements
To run the live interactive versions of the statues, you will need an Azure Kinect sensor with the necessary body tracking SDK installed.

### Building 
`Living Statues` scene. 

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
To run the live interactive versions of the statues, you will need an Azure Kinect sensor with the necessary body tracking SDK installed.

### Printing
For printing, 
Make sure the `PhotoBoothPrinter.py` python script is running and that the printer you want to use is setup as your default printer in your operating system settings. Change the photo directory in the python script. The script is constantly listening for the addition of new images in that directory. 

### Building 
The photobooth is a separate scene in the unity project. 

### Usage
`Esc` closes the project

`Space` starts the countown photo sequence

`R` restart