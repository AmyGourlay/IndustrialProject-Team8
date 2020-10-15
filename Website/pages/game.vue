<template>
  <div class="container">
    <div class="tile is-ancestor">
      <div class="tile is-parent is-8">
        <article class="tile is-child box">
          <p class="title" id="questionNumber">Question 000</p>
          <p class="subtitle" id="questionTopic">Topic: ---</p>
          <div class="content">
            <p class="is-large" id="questionBox"></p>
          </div>
        </article>
      </div>
      <div class="tile is-parent">
        <article class="tile is-child box">
          <p class="subtitle">Wondering how your competitors are doing? This is the place for you 👀</p>
          <div class="message">
            <div class="message-header">
              <p>Leaderboard</p>
              <b-button class="button" type="is-primary" @click="refreshLeaderboard">
                <b-icon class="icon" pack="mdi" icon="refresh">
                </b-icon>
              </b-button>
            </div>
            <div class="message-body">
              <b-table
                :data="tableData"
                :columns="tableColumns"
                :perPage="5"
                paginated
                default-sort="score"
                default-sort-direction="desc"
                :paginationSimple="false"
              ></b-table>
            </div>
          </div>
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
  </div> <!-- closing container tag-->
</template>

<script>
export default {
  name: 'Game',
  data() {
    return {
        tableData: [],
        currQuestion: 1,
        userLobbyId: 90909090,
        nickname: "xXx_tr1v1a_G0d_xXx",
        tableColumns: [
            {
                field: 'name',
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
      this.getExistingQs();
    },
    async refreshLeaderboard() {
      this.tableData = await fetch(`/quizApi/Players/inlobby/${this.userLobbyId}`).then((res) => res.json());
      // console.info(this.tableData);
    },
    async getExistingQs() {  // for existing lobbies
      this.lobbyInfo = await fetch(`/quizApi/Lobbies/${this.userLobbyId}`).then((res) => res.json());
      this.tokens[0] = this.lobbyInfo.easyToken;
      this.tokens[1] = this.lobbyInfo.mediumToken;
      this.tokens[2] = this.lobbyInfo.hardToken;
      this.loadQs();
    },
    async getNewQs() { // for new lobbies only
      this.tokens[0] = await fetch('/getQToken/').then((res) => res.json()).then((res) => res.token); // getting the tokens for each of the difficulties
      this.tokens[1] = await fetch('/getQToken/').then((res) => res.json()).then((res) => res.token);
      this.tokens[2] = await fetch('/getQToken/').then((res) => res.json()).then((res) => res.token);
      this.createLobby(); // sending the tokens to the API for storage in the DB.
      this.loadQs();
    },
    async loadQs() {
      this.easyQs = await fetch(`/getQuestions/amount=10&category=9&difficulty=easy&type=multiple&token=${this.tokens[0]}`).then((res) => res.json()).then((res) => res.results); // using the tokens to get the questions via the API
      this.mediumQs = await fetch(`/getQuestions/amount=10&category=9&difficulty=medium&type=multiple&token=${this.tokens[1]}`).then((res) => res.json()).then((res) => res.results);
      this.hardQs = await fetch(`/getQuestions/amount=10&category=9&difficulty=hard&type=multiple&token=${this.tokens[2]}`).then((res) => res.json()).then((res) => res.results);
      console.info(this.easyQs);
      console.info(this.mediumQs);
      console.info(this.hardQs);
      console.info(this.currQuestion - 1);
      document.getElementById("questionNumber").innerHTML = `Question ${this.currQuestion}`;
      document.getElementById("questionTopic").innerHTML = `Topic: `
      if (this.currQuestion <= 10) {
        document.getElementById("questionBox").innerHTML = this.easyQs[this.currQuestion-1].question;
        document.getElementById("questionTopic").innerHTML = `Topic: ${this.easyQs[this.currQuestion-1].category}`
        console.info(this.easyQs[this.currQuestion-1]);
        console.info(this.tokens[0]);
      }
      else if (this.currQuestion <= 20 && this.currQuestion > 10) {
        document.getElementById("questionBox").innerHTML = this.mediumQs[this.currQuestion-1].question;
        document.getElementById("questionTopic").innerHTML = `Topic: ${this.mediumQs[this.currQuestion-1].category}`
        console.info(this.mediumQs[this.currQuestion-1]);
      }
      else if (this.currQuestion <= 30 && this.currQuestion > 20) {
        document.getElementById("questionBox").innerHTML = this.hardQs[this.currQuestion-1].question;
        document.getElementById("questionTopic").innerHTML = `Topic: ${this.hardQs[this.currQuestion-1].category}`
        console.info(this.hardQs[this.currQuestion-1]);
      }
    },
    async createLobby() {
      const date = new Date();
      const newLobby = {
        id: this.userLobbyId,
        easyToken: this.tokens[0],
        mediumToken: this.tokens[1],
        hardToken: this.tokens[2],
        date: date.toISOString(),
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
    document.getElementById("lobbyCode").innerHTML = this.userLobbyId;
    document.getElementById("userNickname").innerHTML = this.nickname;
    this.refreshLeaderboard();
    this.getQsTokens();
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
