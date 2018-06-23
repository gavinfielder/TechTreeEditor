# TechTreeEditor


This is a game development tool I'm developing for an RTS project I'm working on. It uses SQL to develop and maintain a tech tree.

## Features

 - Techs identified with a 32-bit hex value and store a name, category, name of research field, a cost per day and number of days to research.
 - Techs are connected to other techs through prerequisites
 - Techs can also be connected by "grantrequisite" and "permanizes" relationships, which are specific features of the game I'm designing.

## Future Plans

 - Can visualize local graph of a selected tech: its prerequisites, what techs it is a prerequisite for, and possibly grantrequisite connections as well.
 - Can calculate the total cost and research time of a tech including all of its prerequisites up to root techs.



WIP

Readme last updated 6/23/2018