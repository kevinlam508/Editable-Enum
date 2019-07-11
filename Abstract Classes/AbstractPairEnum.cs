using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;						// Serializable


namespace EditableEnum{
	/*
	 * Class to make enumerators for values don't inherit from UnityEngine.Object
	 *    or to make enumerators with specific names
	 * Create two classes that inherit from AbstractPairEnum and AbstractPair that 
	 *    set the concrete type of the value to make a new pair.
	 */
	public abstract class AbstractPairEnum<T, V> : AbstractEnum<V>
			where V : AbstractPair<T>{

		protected override string GetValueName(int index){
	        return values[index].name;
	    }
	}

	public abstract class AbstractPair<T>{
		public string name;
		public T value;
	}
}