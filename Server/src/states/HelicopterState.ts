import { Schema, type, MapSchema } from "@colyseus/schema";
export class HelicopterState extends Schema{
    @type("string") id:string;
    @type("number") health: number;
    @type("number") x: number;
    @type("number") y: number;
    @type("boolean") dir: boolean; 
    @type({map: "string"}) data = new MapSchema<string>();
    constructor(id: string, x: number, y: number, health: number) {
        super();
        this.id = id;
        this.x = x;
        this.y = y;
        this.health = health;
        this.dir = true;
    }
    move(data: any) {
        this.x += data.x;
    }
}