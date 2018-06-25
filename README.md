# SeedQuest

## Background

Key recovery is a widespread problem in the blockchain space, since human memory has difficulty remembering long strings of numbers, and having backups of keys(physical or digital) can create security vulnerabilities.

If someone wants to use multiple private keys, or use a system of rotating keys, the problem is exacerbated, since the individual would need to keep track of multiple keys at once. (https://github.com/SmithSamuelM/Papers/blob/master/presentations/QuestForTheMnemonSeed_IIW_20180404.pdf)

Using a rotating key system can solve some of the security problems associated with cryptocurrency keys, but increases the difficulty of recovering keys.

By using a one-time pad (https://en.wikipedia.org/wiki/One-time_pad) to encrypt pre-rotated keys, the keys themselves can stored in plain sight, and be decoded when needed.

Using this method will only require the individual to keep the one-time pad secure, instead of their private keys themselves.

This project seeks to create a way for an individual to recreate a one-time pad using visual mnemonics through a simple game.

Having a reliable method to reproduce the one-time pad through a seeded CSPRNG algorithm will make it easier to manage an account with pre-rotated keys.

## Design

By creating a game world with possible routes to take, and specific action to take at certain locations, a human will only need to remember a specific journey, rather than a long string of numbers or words.

This game will have an explorable world with multiple “sites” or buildings/houses/caves where the player can find action spots. 

At each action spot, the player will have a choice of different actions to take. 

A journey through this world to take four actions at four different sites will be reasonable for a human to remember, and provide enough shards of entropy to be combined into a 128-bit string used for a seed to create a one-time pad.  
  
The world built for seed quest would include 256 map areas, with 4 sites (buildings) in each area, with 8 action spots in each site (building), with 8 possible actions at each site. 

The number of sites per area and the number of action spots per site could be variable as long as there are a total of 32 action spots per map area. 

256 areas = 2^8 bits of entropy, 32 action spots per area = 2^5 bits of entropy, 8 items per spot = 2^3 bits of entropy, with 4 actions per item = 2^2 bits of entropy.   
  
With a visual world to explore, the mnemonic load of remembering a seed is significantly reduced. 

As long as someone can remember four actions at four locations, the seed can be recovered at any time by playing the game. 

## Note about Unity assets

While this is an open-source project, we are using some assets purchased through the Unity store that are in a separate repository. 

If you are a collaborator wanting to work on the 3D assets, please contact weston.bodily@consensys.net or michael.mendoza@consensys.net for access to the private repository with the Unity assets.
