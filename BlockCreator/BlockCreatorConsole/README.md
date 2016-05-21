# About BlockCreator Console

BlockCreator is a command line utility used to create LEGO Mindstorms
EV3-G blocks. Here is what you can use it for:

// TODO: add more documentation about EV3 blocks

Note: it is a good idea to download the EV3 software and hardware
development kits that LEGO Mindstorms has made and read through them
to understand what BlockCreator does. You can get them (and more)
from this URL: https://education.lego.com/en-au/learn/middle-school/mindstorms-ev3/support/ev3-developer-kits

Note: an importable block is one that can be imported and used in the LEGO 
Mindstorms software. A block that is missing components or contains errors
cannot be imported.

block 

Displays help about BlockCreator

block module

block makeblock -n TheBlocksName

Creates a new block; this command creates all of the files necessary
to create the most basic importable block.

# About the directory structure and blocks.xml

All commands except for "block module" calls should be run directly
within the block's main directory. The "block module" calls should be run
within the parent directory of the block.

# Sample blocks.xml using Program.cs members