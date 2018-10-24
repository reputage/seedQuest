# SeedQuest: A 3D Mnemonic Game for Key Recovery

## Background

Human memory is often incapable of remembering long strings of random characters like those found in a cryptographic key. Often a mnemonic, a technique used to aid in memory retention, is used to improve the recall of complex information. SeedQuest is a 3D mnemonic game used for key recovery. It is designed to be simple and fun, and it only requires a few minutes to complete. In a virtual 3D game world, the player visits a number of locations and performs various actions in a specific order. This order is generated to encode a player supplied 128-bit seed for a one-time pad that encodes a cryptographic key. After rehearsing the generated quest (the series of actions performed at the different locations in the game), the seed can be recovered at any time by playing the game and completing the rehearsed actions in the correct sequence. A stored encoded cryptographic key can then be decoded using the recovered one-time pad seed. As a simple and effective method for key recovery, this mnemonic could be a useful feature for any cryptographic system where the user is required to generate and manage private keys. 

# System Requirements for Development

[Unity](https://unity3d.com/get-unity/download) for MacOS or Windows (Version 2018.2.0 or greater)

## Note about Unity assets

If you are a collaborator wishing to work on the 3D assets, please contact weston.bodily@consensys.net or michael.mendoza@consensys.net for access to the repository with assets.
