import { Room, Client } from "colyseus";
import { GameRoomState } from "../states/GameRoomState";

import { HeliSpawner } from "../game/HeliSpawner";
import { HelicopterState } from "../states/HelicopterState";
enum MessageType {
  MOVE = 0,
  SPAWN =1,
  TAKE_DAME = 2,
  GUN = 3,
  SHOT = 4,
}

export class GameRoom extends Room<GameRoomState> {

  spawnRate: number = 2000;

  onCreate (options: any) {
    this.setState(new GameRoomState());

    // can init map here

    //spawn heli
    const heliSpawner = new HeliSpawner(4);
    this.clock.setInterval(()=>{
      const heli: HelicopterState = heliSpawner.spawnHeli();
    this.state.onAddHelicopter(heli);
    }, this.spawnRate);


    //handlers message from clients
    this.onMessage(MessageType.MOVE, (client, data) => {
        this.state.onMovePlayer(client, data);
      });

    this.onMessage(MessageType.TAKE_DAME, (client, data)=>{
      this.state.takeDame(data);
    });

    this.onMessage(MessageType.GUN, (client, data)=>{
      this.state.updateGun(client, data);
    });

    this.onMessage(MessageType.SHOT, (client, data)=>{
      this.broadcast(MessageType.SHOT, client.sessionId);
    });

  }

  onJoin (client: Client, options: any) {
    console.log(client.sessionId, "joined!");

    //game room state add new player
    this.state.onAddPlayer(client);
    

  }

  onLeave (client: Client, consented: boolean) {
    console.log(client.sessionId, "left!");

     //game room state delete left player
     this.state.onRemovePlayer(client);
  }

  onDispose() {
    console.log("room", this.roomId, "disposing...");
  }

}
