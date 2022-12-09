import { Schema, type, MapSchema } from "@colyseus/schema";
export class PlayerState extends Schema{
    @type("string") name: string;
    @type("number") x: number;
    @type("number") y: number;
    @type("boolean") dir: boolean; 
    @type({map: "string"}) data = new MapSchema<string>();
    constructor(x: number, y: number, name?: string) {
        super();
        this.x = x;
        this.y = y;
        this.name = name;
        this.dir = true;
    }
}