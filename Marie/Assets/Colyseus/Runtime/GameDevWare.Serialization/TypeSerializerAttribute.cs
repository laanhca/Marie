﻿/* 
	Copyright (c) 2016 Denis Zykov, GameDevWare.com

	This a part of "Json & MessagePack Serialization" Unity Asset - https://www.assetstore.unity3d.com/#!/content/59918

	THIS SOFTWARE IS DISTRIBUTED "AS-IS" WITHOUT ANY WARRANTIES, CONDITIONS AND 
	REPRESENTATIONS WHETHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION THE 
	IMPLIED WARRANTIES AND CONDITIONS OF MERCHANTABILITY, MERCHANTABLE QUALITY, 
	FITNESS FOR A PARTICULAR PURPOSE, DURABILITY, NON-INFRINGEMENT, PERFORMANCE 
	AND THOSE ARISING BY STATUTE OR FROM CUSTOM OR USAGE OF TRADE OR COURSE OF DEALING.
	
	This source code is distributed via Unity Asset Store, 
	to use it in your project you should accept Terms of Service and EULA 
	https://unity3d.com/ru/legal/as_terms
*/

using System;

// ReSharper disable once CheckNamespace
namespace GameDevWare.Serialization
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = false)]
	public class TypeSerializerAttribute : Attribute
	{
		public Type SerializerType { get; private set; }

		public TypeSerializerAttribute(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");
			if (typeof(TypeSerializer).IsAssignableFrom(type) == false) throw new ArgumentException(string.Format("Type '{0}' should inherit from '{1}'.", type, typeof(TypeSerializer)), "type");

			this.SerializerType = type;
		}
	}
}
