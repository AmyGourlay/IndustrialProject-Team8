<template>
  <div>
    <!--<GameMusic />-->
    <section class="section">
      <div class="columns is-mobile">
        <!--<Header></Header>-->
      </div>
      <!-- <Button title="How To Play"></Button>
      <Button title="About"></Button> -->
    </section>
    <section class="nickname">
       <b-field>
         <b-input placeholder="Enter Nickname" size="is-large" icon="account" rounded id="nicknameInput"></b-input>
       </b-field>
       <b-field style="visibility:hidden" id="lobbyField">
         <b-input placeholder="Enter Lobby ID" size="is-large" icon="account" rounded id="LobbyIDInput"></b-input>
       </b-field>
       <div class="buttons is-centered lobbyBtns">
        <BlackButton @click.native="getLobbyCode" title="Join Lobby"></BlackButton>
        <BlackButton @click.native="storeNickname" title="Create Lobby"></BlackButton>
       </div>
    </section>
    <div class="container " id="errorNotifContainer">
      <b-notification
              type="is-danger"
              has-icon
              auto-close
              aria-close-label="Close notification"
              v-model="notifDisplay"
              role="alert"
              id="errorNotif">
        </b-notification>

    </div>
  </div>
</template>

<script>
import Header from '~/components/Header'
import BlackButton from '~/components/BlackButton'
import GameMusic from '~/components/GameMusic'
import router from '../router'
/*
const LobbyCodeModal = {
  props: [
    'lobbyId'
  ],
  template: `
        <form action="">
          <div class="modal-card" style="width: auto">
            <header class="modal-card-head">
                <p class="modal-card-title">Enter Lobby ID</p>
                <button
                    type="button"
                    class="delete"
                    @click="$emit('close')"/>
            </header>
            <section class="modal-card-body">
                <b-field label="Lobby Code">
                    <b-input
                        type="lobbyId"
                        :value="lobbyId"
                        placeholder="Lobby code"
                        required
                        id="lobbyCodeIn">
                    </b-input>
                </b-field>
            </section>
            <footer class="modal-card-foot">
                <button class="button" type="button" @click="$emit('close')">Close</button>
                <button class="button is-primary" @click="storeNickname">Join Lobby</button>
            </footer>
          </div>
        </form>
    `
}*/
export default {
  name: 'home',
  components: {
    Header,
    BlackButton
  },
  data() {
    return {
      nickname: null,
      notifDisplay: false,
      playerExistsAlready: false,
      isComponentModalActive: false,
      lobbyId: 0,
    }
  },
  methods: {
    storeNickname() {
      if (document.getElementById("nicknameInput").value == "") { // if there is nothing in the nickname field
        this.notifDisplay = true;   // show the no nickname warning.
      }
      else {  // if there is something in the nickname field, then ask for the lobby ID
        let nicknameVar = `nickname=${document.getElementById("nicknameInput").value};lobbyId=0;playerExists=${this.playerExistsAlready}`;
        this.$router.push({ name: 'lobby', params: {playerInfo: nicknameVar}});
      }
    },

    async runLobbyChecks(lobbyId, nickname) {
      let response = await fetch(`/quizApi/Lobbies/${lobbyId}`);
      console.log("RUNNING LOBBY CHECKS");
      if (response.status == 200) {
        let request = {
          name: nickname,
          lobbyId: parseInt(lobbyId)
        }
        let checkPlayer = await fetch(`/quizApi/Players/getInfo`, {                 // the POST request to get the player info so we can find the question the player is up to
          method: 'POST',
          headers: {
            'Content-Type': 'application/json;charset=utf-8'
          },
          body: JSON.stringify(request)
        });
        if (checkPlayer.status == 200) {  // a player with that name already exists
          console.log("SHE ALREADY IN THE GAME");
          this.playerExistsAlready = true;
          return true;
        }
        else {
          return true;
        }
      }
      else {
        document.getElementById("errorNotif").innerHTML = "The lobby doesn't exist. Please check what you have entered or speak to your game host.";
        this.notifDisplay = true;   // show the no nickname warning.
        return false;
      }

    },

    async getLobbyCode() {
      if (document.getElementById("nicknameInput").value == "") { // if there is nothing in the nickname field
        document.getElementById("errorNotif").innerHTML = "Please enter a nickname in the box before clicking on a button.";
        this.notifDisplay = true;   // show the no nickname warning.
      }
      else {  // if there is something in the nickname field, then ask for the lobby ID
        document.getElementById("lobbyField").style.visibility = "visible";
        let lobbyIDinput = document.getElementById("LobbyIDInput").value;
        let nicknameInput = document.getElementById("nicknameInput").value;
        if (lobbyIDinput.length == 8 && !isNaN(lobbyIDinput) && await this.runLobbyChecks(lobbyIDinput, nicknameInput)) { // check if its an 8 digit number
          let playerString = `nickname=${nicknameInput};lobbyId=${lobbyIDinput};playerExists=${this.playerExistsAlready}`;
          alert(`redirected! ${playerString}`);
          this.$router.push({ name: 'lobby', params: {playerInfo: playerString} });
        }
      }
    }
  }
}
</script>

<style>

#errorNotifContainer {
  margin-top: 2rem;
  left: 15rem;
}

#errorNotif {
  width: 50rem;
}

.lobbyBtns {
  margin-top: 3rem;
}

.nickname
{
  width: 40rem;
  margin-top: 5rem;
  margin-left: auto;
  margin-right: auto;
}
.joinButton
{
  margin-top: 30px;
  text-align: center;
}
.createButton
{
  margin-top: 10px;
  text-align: center;
}

</style>
