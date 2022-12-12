import { Schema, type, MapSchema } from "@colyseus/schema";
import { Client } from "colyseus";
import { PlayerState } from "./PlayerState";
import { HelicopterState } from "./HelicopterState";
export class GameRoomState extends Schema{
 
    @type({map: PlayerState}) players = new MapSchema<PlayerState>();
    @type({map: HelicopterState}) helicopters = new MapSchema<HelicopterState>();
    
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

    takeDame(data: any){
        const heli:HelicopterState =  this.helicopters.get(data.heliId);

        if(heli){
            heli.health-=1;
            if(heli.health<= 0) this.onRemoveHelicopter(data);
        }
    }

    updateGun(client: Client, data: any){
        const player = this.players.get(client.sessionId);
        player.gun.x = data.x;
        player.gun.y = data.y;
        player.gun.z = data.z;
    }

    
       
}