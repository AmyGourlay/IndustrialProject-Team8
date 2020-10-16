<template>
  <div class="container">
    <div class="tile is-ancestor">
      <div class="tile is-parent is-8">
        <article class="tile is-child box"><!-- QUESTION TILE -->
          <p class="title" id="Streak">Your correct answer streak is...</p>
          <p class="subtitle" id="timer">Next question in 5s</p>
          <div class="content">
            <p class="is-size-3-tablet" id="questionBox"></p>
          </div>
        </article>
      </div>
      <div class="tile is-parent is-vertical"> <!-- SCORE AND LIFELINE TILES -->
        
        <article class="tile is-child box">
          <p class="title" id="playerPosition">2nd</p>
          <p class="subtitle" id="playerScore">Score: 3000</p>
        </article>
      </div>
    </div>
    <div class="tile is-ancestor">
      <div class="tile is-parent is-vertical buttons">
        
      </div>
      <div class="tile is-parent is-vertical buttons">
        
      </div>
      <div class="tile is-parent is-4"> <!-- LEADERBOARD TILE -->
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

  </div> <!-- closing container tag-->
</template>

<script>
export default {
  name: 'Game',
  data() {
    return {
        tableData: [],
        currQuestion: 1,
        currQuestionJSON: null,
        userLobbyId: 90909090,
        nickname: "felicia",
        score: 2300,
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
      easyQs: [],
      mediumQs: [],
      hardQs: []
    }
  },
  methods: {
    updatePlayerScoreAndPos(adjustment) {
      this.score += adjustment;
      document.getElementById("playerScore").innerHTML = `Score: ${this.score}`;
      this.refreshLeaderboard();
      this.tableData.sort(function (a,b) {
        return b.score - a.score;
      });
      let playerPos = 0;
      for (playerPos=0; playerPos < this.tableData.length; playerPos++) {
        console.info("in player loop!");
        if (this.tableData[playerPos].name === this.nickname) {
            break;
        }
      }
      playerPos++;
      let positionElem = document.getElementById("playerPosition");
      console.info(playerPos);
      switch (playerPos) {
        case 1:
          positionElem.innerHTML = "1st";
          break;
        case 2:
          positionElem.innerHTML = "2nd";
          break;
        case 3:
          positionElem.innerHTML = "3rd";
          break;
        default:
          positionElem.innerHTML = `${playerPos}th`;
          break;
      }
      // TODO: Update the player score in the database HERE
    },
    async refreshLeaderboard() {
      this.tableData = await fetch(`/quizApi/Players/inlobby/${this.userLobbyId}`).then((res) => res.json());
      console.info(this.tableData);
    },
  },
  async fetch() {
    this.lobbyInfo = await fetch(`/quizApi/Lobbies/${this.userLobbyId}`).then((res) => res.json());
    console.info(JSON.stringify(this.lobbyInfo));
    document.getElementById("lobbyCode").innerHTML = this.userLobbyId;
    document.getElementById("userNickname").innerHTML = this.nickname;
    this.refreshLeaderboard();
    this.startGame();
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