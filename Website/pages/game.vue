<template>

  <div class="container">
    <!--<section class="section is-small">
      <div class="tile is-ancestor">
        <div class="tile is-parent">
          <div class="tile is-child box">
            <p class="title">Quizz.io 📝</p>
          </div>
        </div>
        <div class="tile is-parent">
          <div class="tile is-child box">
            <p class="title"> xXx_qUizL0rdE_xXx </p>
          </div>
        </div>
        <div class="tile is-parent">
          <article class="tile is-child box">
            <b-button class="is-danger is-large">Leave Game   </b-button>
          </article>
        </div>
      </div>
    </section>
    <section class="section">
      <div class="tile ">
          <article class="tile is-child notification is-primary">
            <p class="title">Question 69</p>
            <p class="subtitle"> Topic - Computers?</p>
            <div class="content">
              birds are government drones
            </div>
          </article>
          <article class="tile is-child is-info notification is-4">
            <p class="title"> MY LAST TRACK JUST WENT VIRAL! </p>
          </article>
        </div>

    </section>-->


    <div class="tile is-ancestor">
      <div class="tile is-parent is-8">
        <article class="tile is-child box">
          <p class="title">Question 69</p>
          <p class="subtitle">Topic: Computers?</p>
          <div class="content">
            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin ornare magna eros, eu pellentesque tortor vestibulum ut. Maecenas non massa sem. Etiam finibus odio quis feugiat facilisis.</p>
          </div>
        </article>
      </div>
      <div class="tile is-parent">
        <article class="tile is-child box">
          <p class="subtitle">Wondering how your competitors are doing? This is the place for you 👀</p>
          <article class="message">
          <div class="message-header">
            <p>Leaderboard</p>
          </div>
          <div class="message-body">
            <b-table :data="tableData" :columns="tableColumns"></b-table>
          </div>
        </article>
        </article>
      </div>
    </div>
    <button @click="getQsTokens">Refresh Data</button>
    <div>
    <h1>Lobby info</h1>
    <p v-if="$fetchState.pending">Fetching lobby details ...</p>
    <p v-else-if="$fetchState.error">
      Error while fetching lobby details: {{ $fetchState.error.message }}
    </p>
    <ul v-else>
      <p>
        {{ this.lobbyInfo }}
      </p>
    </ul>
  </div>
  </div>
</template>

<script>
export default {
  name: 'Game',
  data() {
    return {
        tableData: [],
        userLobbyId: 99990000,
        tableColumns: [
            {
                field: 'nickname',
                label: 'Nickname',
                width: '40',
            },
            {
                field: 'score',
                label: 'Score',
                numeric: true,
                sortable: true,
                centered: true
            }
        ],
      lobbyInfo: [],
      tokens: [],
      easyQs: [],
      mediumQs: [],
      hardQs: []
    }
  },
  methods: {
    getQsTokens() {
      console.log("YAY");
      this.getQuestions();
    },
    async getQuestions() {
      this.tokens[0] = await fetch('/getQToken/').then((res) => res.json()).then((res) => res.token); // getting the tokens for each of the difficulties
      this.tokens[1] = await fetch('/getQToken/').then((res) => res.json()).then((res) => res.token);
      this.tokens[2] = await fetch('/getQToken/').then((res) => res.json()).then((res) => res.token);
      this.easyQs = await fetch('/getQuestions/amount=10&category=9&difficulty=easy&type=multiple').then((res) => res.json()).then((res) => res.results); // using the tokens to get the questions via the API
      this.mediumQs = await fetch('/getQuestions/amount=10&category=9&difficulty=medium&type=multiple').then((res) => res.json()).then((res) => res.results);
      this.hardQs = await fetch('/getQuestions/amount=10&category=9&difficulty=hard&type=multiple').then((res) => res.json()).then((res) => res.results);
      this.createLobby(); // sending the tokens to the API for storage in the DB.
    },

    async createLobby() {
      const newLobby = {
        id: 10101010,
        easyToken: this.tokens[0],
        mediumToken: this.tokens[1],
        hardToken: this.tokens[2],
        requestURL: "amount=10&category=9&type=multiple"
      };
      const response = await fetch('/quizApi/Lobbies', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(newLobby)
      });
    }
  },
  async fetch() {
    this.lobbyInfo = await fetch(`/quizApi/Lobbies/${this.userLobbyId}`).then((res) => res.json());
    console.info(JSON.stringify(this.lobbyInfo));
  },
}

</script>

<style lang="scss">
  $warning: #ffba49;
  $link: #20a39e;
  $info: #a4a9ad;
    $danger: #f9b1b1;
  $primary: #23001e;
</style>
