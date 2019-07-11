using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;					// Editor, AssetDatabase

using System;						// Serializable
using System.IO;					// StreamWriter
using System.Globalization;			// UnityCategory

[CreateAssetMenu(fileName = "New Editable Enum", menuName = "Editable Enumerator")]
[Serializable]
public class EditableEnum : ScriptableObject
{
	public enum AccessType { None, Public, Protected, Internal, ProtectedInternal, Private, PrivateProtected}
	private static string[] accessStrings = {
		" ",
		"public ",
		"protected ",
		"internal ",
		"protected internal ",
		"private ",
		"private protected "
	};

	[Tooltip("The name of the file to put the enumerator in."
    	+ "\nThe file will be completely overriden and will be placed in the same folder as this object.")]
	public string fileName;

    [Tooltip("The namespace to put the enumerator in."
    	+ "\nLeave blank to not have a namespace.")]
    public string namespaceName;

    [Header("Class Parameters")]
    [Tooltip("The class to put the enumerator in."
    	+ "\nLeave blank to not have a namespace.")]
    public string className;
    [Tooltip("Is the class a partial class if listed")]
    public bool isPartial;
    [Tooltip("Access modifier of the class if listed. Can only be none, public, or internal.")]
    public AccessType classAccessType;

    [Header("Enumerator Parameters")]
    [Tooltip("The name of the enumerator.")]
    public string enumeratorName;
    [Tooltip("Access modifier of the enumerator if listed. Can only be none, public, or internal if not in a class")]
    public AccessType enumAccessType;
    [Tooltip("The names of the values in order.")]
    public List<string> valueNames;

    public string GeneratedFilePath{ 
    	get{
	    	string origPath = AssetDatabase.GetAssetPath(this);
	    	string directory = origPath.Substring(0, origPath.LastIndexOf('/'));
	    	string path = directory + "\\" + fileName + ".cs";
	    	return path;
    	}
    }

    public bool WillOverwrite => File.Exists(GeneratedFilePath);

    // filePath: where to put the file
    public void UpdateEnumerator(){
    	if(IsValid()){
	    	string path = GeneratedFilePath;
	    	string code = GenerateCode();
	    	using (StreamWriter outfile = new StreamWriter(path))
            {
            	outfile.Write(code);
            }
            Debug.Log("Wrote file " + path);
            AssetDatabase.Refresh();
    	}
    }

    public bool HasNamespace => namespaceName.Length > 0;
    public bool HasClass => className.Length > 0;

    private string GenerateCode(){
		string code = "";
    	string tabs = "";
    	// open namespace
    	if(HasNamespace){
    		code += "namespace " + namespaceName + "{\n";
    		tabs += "\t";
    	}

		// open class
    	if(HasClass){
    		code += tabs + accessStrings[(int)classAccessType] 
    			+ ((isPartial) ? "partial " : "") + "class " + className + "{\n";
    		tabs += "\t";
    	}

    	// open enum
    	code += tabs + accessStrings[(int)enumAccessType] + "enum " + enumeratorName + "{\n";
    	tabs += "\t";

    	// list values
    	for(int i = 0; i < valueNames.Count - 1; ++i){
    		code += tabs + valueNames[i] + ",\n";
    	}
    	code += tabs + valueNames[valueNames.Count - 1] + "\n";

    	// close enum
    	tabs = tabs.Remove(tabs.Length - 1);
    	code += tabs + "}\n";

		// close class
    	if(HasClass){
    		tabs = tabs.Remove(tabs.Length - 1);
    		code += tabs + "}\n";
    	}

    	// close namespace
    	if(HasNamespace){
    		code += "}\n";
    		tabs = tabs.Remove(tabs.Length - 1);
    	}

    	return code;
    }

    public bool IsPublicAccess(AccessType type){
    	return type == AccessType.None || type == AccessType.Public || type == AccessType.Internal;
    }

    public bool IsValid(){
    	bool res = true;
    	// check file name
		if(fileName.Length == 0){
    		res = false;
    		Debug.LogError("File Name is does not exist");
    	}

    	// check namespace name
    	if(HasNamespace){
    		bool nameValid = true;
    		foreach(string s in namespaceName.Split('.')){
    			if(!IsValidIdentifier(s)){
    				nameValid = false;
    			}
    		}
    		if(!nameValid){
	    		res = false;
	    		Debug.LogError("Namespace Name \"" + namespaceName + "\" is not a valid name");
	    	}
    	}

    	// check class vars
    	if(HasClass && !IsValidIdentifier(className)){
    		res = false;
    		Debug.LogError("Class Name \"" + className + "\" is not a valid name");
    	}
    	// class must be public or internal
    	if(!IsPublicAccess(classAccessType)){
    		res = false;
    		Debug.LogError("Class can only be none, public, or internal access");
    	}

    	// check enumerator vars
    	if(!IsValidIdentifier(enumeratorName)){
    		res = false;
    		Debug.LogError("Enumerator Name \"" + enumeratorName + "\" is not a valid name");
    	}
    	if(!HasClass && !IsPublicAccess(enumAccessType)){
    		res = false;
    		Debug.LogError("Enum not in a class can only be none, public, or internal access");
    	}

    	// check values
    	if(valueNames.Count == 0){
    		res = false;
    		Debug.LogError("There are no value names");
    	}
    	for(int i = 0; i < valueNames.Count; ++i){
    		string val = valueNames[i];
	    	if(!IsValidIdentifier(val)){
	    		res = false;
	    		Debug.LogError("Value Name " + i + " \"" + val + "\" is not a valid name");
	    	}
    	}
    	for(int i = 0; i < valueNames.Count; ++i){
    		for(int j = i + 1; j < valueNames.Count; ++j){
    			if(valueNames[i] == valueNames[j]){
    				res = false;
    				Debug.LogError("Value names " + i + " and " + j + " both are " + valueNames[i]);
    			}
    		}
    	}
    	return res;
    }

    private static string[] keywords = {
    	"abstract" , "as"       , "base"       , "bool"      , "break",
    	"byte"     , "case"     , "catch"      , "char"      , "checked",
    	"class"    , "const"    , "continue"   , "decimal"   , "default",
    	"delegate" , "do"       , "double"     , "else"      , "enum",
    	"event"    , "explicit" , "extern"     , "false"     , "finally",
    	"fixed"    , "float"    , "for"        , "foreach"   , "goto",
    	"if"       , "implicit" , "in"         , "int"       , "interface",
    	"internal" , "is"       , "lock"       , "long"      , "namespace",
    	"new"      , "null"     , "object"     , "operator"  , "out",
    	"override" , "params"   , "private"    , "protected" , "public",
    	"readonly" , "ref"      , "return"     , "sbyte"     , "sealed",
    	"short"    , "sizeof"   , "stackalloc" , "static"    , "string",
    	"struct"   , "switch"   , "this"       , "throw"     , "true",
    	"try"      , "typeof"   , "uint"       , "ulong"     , "unchecked",
    	"unsafe"   , "ushort"   , "using"      , "virtual"   , "void",
    	"volatile" , "while"
    };

    // simple check, does not fully determine if the string is valid
    private bool IsValidIdentifier(string identifier){
    	if(identifier.Length == 0){
    		return false;
    	}

    	// identifier can be @ followed by a keyword
    	if(identifier[0] == '@' && Array.Exists(keywords, identifier.Substring(1).Equals)){
    		return true;
    	}

    	// general identifier rules
    	if (!char.IsLetter(identifier[0]) && identifier[0] != '_'){
	        return false;
	    }
	    for (int i = 1; i < identifier.Length; ++i){
	        if (!ValidMidChar(identifier[i])){
	           return false;
	        }
	    }
	    return true;
    }

    private bool ValidMidChar(char c){
    	UnicodeCategory cat = char.GetUnicodeCategory(c);
    	return char.IsLetterOrDigit(c) || cat == UnicodeCategory.NonSpacingMark 
    		|| cat == UnicodeCategory.SpacingCombiningMark 
    		|| cat == UnicodeCategory.ConnectorPunctuation 
    		|| cat == UnicodeCategory.Format; 
    }
}
