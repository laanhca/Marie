import { Schema, type, MapSchema } from "@colyseus/schema";
import { Client } from "colyseus";
import { PlayerState } from "./PlayerState";
export class GameRoomState extends Schema{
 
    @type({map: PlayerState}) players = new MapSchema<PlayerState>();
    
    onAddPlayer(client: Client){
        const player: PlayerState = new PlayerState(0, -4.63, client.sessionId);
        this.players.set(client.sessionId, player);
    }

    onRemovePlayer(client: Client){
        this.players.delete(client.sessionId);
    }
}