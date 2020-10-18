<template>
  
    <section class="section is-fullheight">
      <div class="column is-mobile is-centered">
          <article class="tile is-child notification is-black">
            <p class="title titlefont is-centered" id="playerPosition">2nd</p>
            <p class="subtitle subfont is-centered" id="playerScore">Score: 3000</p>
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
import Card from '~/components/Card';
import BlackButton from '~/components/BlackButton';
import LobbyTable from '~/components/LobbyTable';
import LobbyCodeLbl from '~/components/LobbyCodeLbl';
export default {

  name: 'lobby',
  components: {
    Card
  },
  data() {
    return {
      lobbyInfo: []
    }
  },
  methods: {
    async createLobby() {
      console.info("In lobby vue!");
      const date = new Date();
      const newLobby = {
        easyQs: "",
        mediumQs: "",
        hardQs: "",
        date: date.toISOString(),
        requestURL: "amount=10&category=9&type=multiple&encode=base64"
      };
      this.lobbyInfo = await fetch('/quizApi/Lobbies', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(newLobby)
      }).then((res) => res.json());
      console.info(this.lobbyInfo);
      document.getElementById("lobbyCode").innerHTML = this.lobbyInfo.id;
    }
  }
}
</script>

<style>
  .is-centered{
    text-align: center;
  }

  .titlefont{
    font-size: 60px;
  }

  .subfont{
    font-size: 40px;
  }
</style>
