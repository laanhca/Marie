import { Schema, type, MapSchema } from "@colyseus/schema";
export class PlayerState extends Schema{
    @type("string") name: string;
    @type("number") x: number;
    @type("number") y: number;
    @type({map: "string"}) data = new MapSchema<string>();
}