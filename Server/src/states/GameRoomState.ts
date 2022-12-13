import { Schema, type, MapSchema } from "@colyseus/schema";
import { Client } from "colyseus";
import { PlayerState } from "./PlayerState";
import { HelicopterState } from "./HelicopterState";
export class GameRoomState extends Schema{
 
    @type({map: PlayerState}) players = new MapSchema<PlayerState>();
    @type({map: HelicopterState}) helicopters = new MapSchema<HelicopterState>();
    @type("number") serverTime: number = 0.0;

    
    onAddPlayer(client: Client){
        const player: PlayerState = new PlayerState(0, -4.63, client.sessionId);
        this.players.set(client.sessionId, player);
    }

    onRemovePlayer(client: Client){
        this.players.delete(client.sessionId);
    }

    onMovePlayer(client: Client, data: any){
        this.players.get(client.sessionId).move(data);
    }

    onAddHelicopter(heli: HelicopterState){
       
        this.helicopters.set(heli.id, heli);
    }

    onRemoveHelicopter(data: any){
        this.helicopters.delete(data.heliId);
    }

    // onMoveHelicopter(data: any){
    //     this.helicopters.get(data.heliId).move(data.x);
    // }



 

    updatePlayer(client: Client, changeSet: any){
        console.log(changeSet);
        const player = this.players.get(client.sessionId);
        for (let i = 0; i < changeSet.length; i += 2) {
            const property = changeSet[i];
            let updateValue = changeSet[i + 1];
            (player as any)[property] = updateValue;
            
          }
          player.timestamp = this.serverTime;
    }

    
       
}