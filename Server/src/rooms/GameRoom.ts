import { Room, Client } from "colyseus";
import { GameRoomState } from "../game_states/GameRoomState";

enum MessageType {
  MOVE = 0,
}

export class GameRoom extends Room<GameRoomState> {

  

  onCreate (options: any) {
    this.setState(new GameRoomState());

    // can init map here


    //handlers message from clients
    this.onMessage(MessageType.MOVE, (client, data) => {
        this.state.onPlayerMove(client, data);
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
