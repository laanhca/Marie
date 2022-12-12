import { Clock } from "colyseus";
import randomFloat from "random-float";
import { Vec3 } from "../helper/vector3";
import { HelicopterState } from "../states/HelicopterState";

export class HeliSpawner{
    spawnPosition : Vec3.Vec;
    randomYPosition: number;
    id: number = 0;
    constructor(randomYPosition:number){
        this.spawnPosition = new Vec3.Vec(-10.9, 0.69, 10);
        this.randomYPosition = randomYPosition;
    }
    spawnHeli(): HelicopterState {
        this.randomYPosition =  this.spawnPosition.y + this.getRandomFloat(0, this.randomYPosition, 5);
        
        const heli: HelicopterState = new HelicopterState(this.id.toString(), this.spawnPosition.x, this.randomYPosition, 3);
        this.id++;
        return heli;
    }
    getRandomFloat(min: any, max: any, decimals: any) {
        const str = (Math.random() * (max - min) + min).toFixed(decimals);
        return parseFloat(str);
      }
    
    
}