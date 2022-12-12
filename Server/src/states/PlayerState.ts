import { Schema, type, MapSchema } from "@colyseus/schema";
import { GunState } from "./GunState";
export class PlayerState extends Schema{
    @type("string") name: string;
    @type("number") x: number;
    @type("number") y: number;
    @type("boolean") dir: boolean; 
    @type(GunState) gun = new GunState();
    constructor(x: number, y: number, name?: string) {
        super();
        this.x = x;
        this.y = y;
        this.name = name;
        this.dir = true;
    }
    move(data: any) {
        this.x += data.x;
    }
}