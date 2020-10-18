<template>
<div>
  <GameMusic />
  <section class="section">
  </section>
  <section class="nickname">
     <b-field>
       <b-input placeholder="Enter Nickname" size="is-large" icon="account" rounded></b-input>
     </b-field>
     <div class="joinButton">
        <nuxt-link to="/lobby"><BlackButton title="Join Lobby"></BlackButton></nuxt-link>
      </div>
      <div class="createButton">
        <nuxt-link to="/lobby"><BlackButton title="Create Lobby" @click="createLobby"></BlackButton></nuxt-link>
      </div>
  </section>
</div>
</template>

<script>
import GameMusic from '~/components/GameMusic'
import Header from '~/components/Header'
import BlackButton from '~/components/BlackButton'
export default {
  name: 'HomePage',
  components: {
    Header,
    BlackButton,
    GameMusic,
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

.nickname
{
  width: 950px;
  margin-top: 8%;
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
