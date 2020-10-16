<template>
  
    <section class="section is-fullheight">
      <div class="column is-mobile is-centered">
          <article class="tile is-child notification is-black">
            <p class="title" id="playerPosition">2nd</p>
            <p class="subtitle" id="playerScore">Score: 3000</p>
          </article>
          <h1>{{lobbyCode}}</h1>
      </div>
      <div class="column is-mobile is-centered">
        <LobbyTable></LobbyTable>
      </div>
      <div class="columns is-mobile is-centered is-gapless">
        <div class="column is-3">
          <router-link to="/lobby"><BlackButton title="Play Again"></BlackButton></router-link>
        </div>
      </div>
    </section>
  
 
</template>

<script>
import Card from '~/components/Card'
import BlackButton from '~/components/BlackButton'
import LobbyTable from '~/components/LobbyTable'
import LobbyCodeLbl from '~/components/LobbyCodeLbl'
import router from 'router'
export default {
  name: 'lobby',
  components: {
    Card,
    Button
  },
  data() {
    return {
      lobbyInfo: [],
      lobbyCode: []
    }
  },
    
  async fetch() {
      this.lobbyInfo = await fetch(`/quizApi/Lobbies/${this.userLobbyId}`).then((res) => res.json());
      console.info(JSON.stringify(this.lobbyInfo));
      document.getElementById("lobbyCode").innerHTML = this.userLobbyId;
      //document.getElementById("userNickname").innerHTML = this.nickname;
  } 
}