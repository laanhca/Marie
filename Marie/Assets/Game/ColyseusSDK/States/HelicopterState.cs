// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.44
// 

using Colyseus.Schema;

public partial class HelicopterState : Schema {
	[Type(0, "string")]
	public string id = default(string);

	[Type(1, "number")]
	public float health = default(float);

	[Type(2, "number")]
	public float x = default(float);

	[Type(3, "number")]
	public float y = default(float);

	[Type(4, "boolean")]
	public bool dir = default(bool);

	[Type(5, "map", typeof(MapSchema<string>), "string")]
	public MapSchema<string> data = new MapSchema<string>();
}

