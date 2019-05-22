# SeedQuest: A 3D Mnemonic Game for Key Recovery

![Logo](https://github.com/reputage/seedQuest/blob/master/media/SeedQuestLogo-Github.png)

![SeedQuest-V0.2.2](https://img.shields.io/badge/SeedQuest(beta)-V0.2.2-orange.svg)
![Platforms-MacOS-Windows](https://img.shields.io/badge/Platform-MacOS%20%7C%20Windows-blue.svg)

## You have just found SeedQuest

SeedQuest is 3D mnemonic game used for key recovery. It is designed to be simple and fun, and only require a few minutes to complete. 
Please try out the lastest prototype for SeedQuest, and complete a survey to give feedback on the game.

You can find the latest version of the SeedQuest [here](https://github.com/reputage/seedQuest/blob/master/docs/releases.md). 

To report issues with the application or bugs, please use the [Issues Page](https://github.com/reputage/seedQuest/issues).
 
For help with SeedQuest or talk with developers about this project, visit our [Slack Channel](https://join.slack.com/t/seedquest/shared_invite/enQtNDgyMjUyNzQ2OTAxLWQwYmIwMjIyYTEwZGJjYzNhY2RhNjlhZWE2MDVkOThmMTU5MDhlMTkyZGViNGUyNjYyNTVkYjYyNmRmM2YyZTI). 

### Lastest News

SeedQuest Beta is coming soon. We'll release on github to the public on May 20th. Look forward to that!

## How does SeedQuest work?

In SeedQuest, the player will visit a few different locations and perform various actions in a specific orfer. This order of actions is used to encode a key which will be used as a seed string. This seed is used to randomly generate the series of actions in the game that you'll need to remember. You have no control over that encoding, but you can select the seed string you would like to use. You can practice learning the order of actions in 'learn' mode, and then you can prove your memory in 'recover' mode. In 'recover' mode you can play the game to decode and recover your seed string by completeing the actions in the correct sequence. 

## SeedQuest Community

SeedQuest is an open source project and we want to grow a community of people working on and activately improving the SeedQuest game. We welcome feedback on the project. To facilitate this we have created a Slack channel (seedquest.slack.com) that anyone can join. Join the Slack channel [here](https://join.slack.com/t/seedquest/shared_invite/enQtNDgyMjUyNzQ2OTAxLWQwYmIwMjIyYTEwZGJjYzNhY2RhNjlhZWE2MDVkOThmMTU5MDhlMTkyZGViNGUyNjYyNTVkYjYyNmRmM2YyZTI). 

## Collaboration

If you would like to be a collaborator or work on the 3D assets, please contact michael.mendoza@consensys.net or weston.bodily@consensys.net for information and access to the assets.

## Background

Human memory is often incapable of remembering long strings of random characters like those found in a cryptographic key. Often a mnemonic, a technique used to aid in memory retention, is used to improve the recall of complex information. SeedQuest is a 3D mnemonic game used for key recovery. It is designed to be simple and fun, and it only requires a few minutes to complete. In a virtual 3D game world, the player visits a number of locations and performs various actions in a specific order. This order is generated to encode a player supplied 128-bit seed for a one-time pad that encodes a cryptographic key. After rehearsing the generated quest (the series of actions performed at the different locations in the game), the seed can be recovered at any time by playing the game and completing the rehearsed actions in the correct sequence. A stored encoded cryptographic key can then be decoded using the recovered one-time pad seed. As a simple and effective method for key recovery, this mnemonic could be a useful feature for any cryptographic system where the user is required to generate and manage private keys. 

## Media

![alt text](https://github.com/reputage/seedQuestAssets/blob/master/concept%20art/Media/2019-05-22_SeedQust_V0_2_4.gif "SeedQuest Prototype GIF") 
