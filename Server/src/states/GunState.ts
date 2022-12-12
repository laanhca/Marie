import { Schema, type, MapSchema } from "@colyseus/schema";
export class GunState extends Schema{
    @type("number") x: number;
    @type("number") y: number;
    @type("number") z: number;

}