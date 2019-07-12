# Unity-Editable-Enum
 
A simple set of scriptable objects to generate and update an enumerator from the Unity inspector.  
Made because of multiple cases of wanting to edit an enumerator without leaving the Unity editor. 
Made in Unity 2018.3.3f1.  

Usage  
-----
Create the enumerator parameters in Create/Editable Enumerator and fill in the 
fields. After the parameters are set, press Generate to create the enumerator. 
The enumerator will be put in a file in the same folder as the parameters. 
Any updates are not automatically updated and Update will need to be pressed 
to do so. The generator will not check for naming conflicts. 

Parameters
----------
* File Name must be a valid filename.  
* Namespace Name can include multiple levels, but each level must be a valid identifier. Leave the field blank to not have a namespace.  
* Class Name must be a valid identifier, leave bank to not have a class.  
* Class Access Type must be None, Public, or Internal.  
* Enumerator Name must be a valid identifier.  
* Enumerator Access Type must be None, Public, or Internal if a class is not specified.  
* Value Names cannot be empty and cannot have duplicates. Each name must be a valid identifier.

Provided Variants  
-----------------
* SimpleEnum: Takes a list of strings and directly creates an enumerator  
* ColorEnum: Provides a list of name-Color pairs and creates an enumerator from the name  
* PrefabEnum: Provides a list of GameObjects, using their names as enumerator names  
* AudioClipEnum: Has AudioClips and makes their names as enumerator names  
* SpriteEnum: Lists sprites, making the enumerator from their names  

Making New Variants
-------------------
To make a new variant, there are three abstract classes that can be extended 
to quickly make a new enumerator type, AbstractEnum, AbstractUnityObjectEnum, 
and AbstractPairEnum.  

AbstractEnum is the lowest level class. The generic argument is the type that 
will show up in the values list and must be serializable by Unity's serialization 
to appear in the inspector. Any child class needs to define GetValueName() 
so that the base class knows what is the name of the value when creating the 
enumerator. An example for reference is SimpleEnum.  

AbstractUnityObjectEnum is a class to enumerate Unity.Objects. A child class 
will only need to set the generic type to a type that extends Unity.Object and 
the names of the objects will be used for the enumerator. SpriteEnum and 
AudioClipEnum are examples.  

AbstractPairEnum provides a way to separate the naming and the related values. 
The generic arguments are the type of the value and a class that extends 
AbstractPair with the value's type as generic arguments. For example, to make 
ColorEnum, the generics were set to Color and ColorPair, where ColorPair 
extends AbstractPair<Color>. The classes were set up this way to work with 
Unity's serialization.  
