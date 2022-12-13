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
  UpdatePlayer = 5,
}

export class GameRoom extends Room<GameRoomState> {


  onCreate (options: any) {
    this.setState(new GameRoomState());

    // Set the frequency of the patch rate
    this.setPatchRate(50);

    // Set the Simulation Interval callback
    this.setSimulationInterval(dt => {
      this.state.serverTime += dt;
    });
    // can init map here

    //custom logic
    this.customLogic();


    // Register message handlers for messages from the client
    this.registerForMessages();
   

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

  //////////////////////////////////////////////////////////////////////////////////


  spawnRate: number = 2000;

  customLogic(){
    //spawn heli
    // const heliSpawner = new HeliSpawner(4);
    // this.clock.setInterval(()=>{
    //   const heli: HelicopterState = heliSpawner.spawnHeli();
    // this.state.onAddHelicopter(heli);
    // }, this.spawnRate);
  }


  registerForMessages(){
    this.onMessage(MessageType.MOVE, (client, data) => {
      this.state.onMovePlayer(client, data);
    });

    this.onMessage(MessageType.TAKE_DAME, (client, data)=>{
    });

    this.onMessage(MessageType.GUN, (client, data)=>{
    });

    this.onMessage(MessageType.SHOT, (client, data)=>{
      console.log('shot: '+ client.sessionId);
      this.broadcast(MessageType.SHOT, client.sessionId);
    });

    this.onMessage(MessageType.UpdatePlayer, (client, changeSet)=>{
      this.state.updatePlayer(client, changeSet);
    });
  }

}
