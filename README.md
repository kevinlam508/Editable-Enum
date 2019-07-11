# Unity-Editable-Enum
 
A simple scriptable object to generate and update an enumerator from the Unity inspector.  
Made because of multiple cases of wanting to edit an enumerator without leaving the Unity editor. 
Made in Unity 2018.3.3f1.  

Usage  
-----
Create the enumerator parameters in the create menu and fill in the fields.  
After the paramters are set, press Generate to create the enumerator.  
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
