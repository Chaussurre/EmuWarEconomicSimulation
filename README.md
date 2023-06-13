# EmuWarEconomicSimulation
This is a series of tools and building blocs for game developpement. None of these tools are finished, and I don't recommend using them for now as they are subject to changes.

However, I made this repository public so I could show what I was currently working on.

# The tools

So far there are three tools, none of which are completed. I am currently working on two of them 

First, there's the CombatSystem block. It is a small collections of tools based on the Observer Pattern with the purpose of handling combat systems that allows for behavior and content to be added without having to take it into account in the first place. To put it in simpler terms, the goal is to design a system that handle the characters hp, mana, etc, where the designers can freely invent new abilities and characters, even very complex ones, without modifying the core of the system.

Then, there's Tabletop. The goal of it is to use the CombatSystem to make multiplayer tabletop games. So far only card games are being developped, but as time goes I might add other types of tabletop games.

Lastly, there's walker, what will be a series of component to handle 3D character walking, in such a way that new movements actions can be inserted in the list of existing ones. (for example, your character can walk and crouch, but you can insert your own code for some wall climbing if you want).
Walker is on pause and I have no plan to continue it for now.
